using AuthenticationData.DataBase;
using AuthenticationData.Models;
using ExchangeRateWebApi.TokenManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateWebApi.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IJWTManager _jWTManager;
		private readonly ITokenServiceInteractor _tokenServiceInteractor;

		public UserController(IJWTManager jWTManager, ITokenServiceInteractor tokenServiceInteractor)
		{
			this._jWTManager = jWTManager;
			this._tokenServiceInteractor = tokenServiceInteractor;
		}

		/// <summary>
		/// Аутентификация и выдача токена
		/// </summary>
		/// <param name="usersdata">Логин + пароль</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		[Route("authenticate")]
		public IActionResult Authenticate(User usersData)
		{
			//Проверка существования пользователя
			var validUser = _tokenServiceInteractor.IsValidUser(usersData);

			if (!validUser)
			{
				return Unauthorized("Невреное имя пользователя или пароль.");
			}

			//Генеранция токена
			var token = _jWTManager.GenerateToken(usersData.Name);

			if (token == null)
			{
				return Unauthorized("Ошибка при генерации токена.");
			}

			// Сохранение в БД токенов
			var refreshToken = new UserRefreshToken
			{
				RefreshToken = token.RefreshToken,
				UserName = usersData.Name
			};

			_tokenServiceInteractor.InsertUserRefreshToken(refreshToken);

			//Возврат успешно сгенерированного токена
			return Ok(token);
		}

		/// <summary>
		/// Обновление токена
		/// </summary>
		/// <param name="token">Токен + токен обновления</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		[Route("refresh")]
		public IActionResult Refresh(AccessToken token)
		{
			//Получение данных из токена
			var principal = _jWTManager.GetPrincipalFromExpiredToken(token.Token);
			var userName = principal.Identity?.Name;

			//Получение сохраненного токена пользователя из БД
			var savedToken = _tokenServiceInteractor.GetSavedRefreshToken(userName);

			if (savedToken?.RefreshToken != token.RefreshToken)
			{
				return Unauthorized("Введены не верные данные.");
			}

			//Генераwия нового токена
			var newToken = _jWTManager.GenerateToken(userName);

			if (newToken == null)
			{
				return Unauthorized("Ошибка при генерации токена.");
			}

			// Обновление в БД токенов
			_tokenServiceInteractor.UpdateUserRefreshToken(userName, token.RefreshToken, newToken.RefreshToken);

			//Возврат успешно сгенерированного токена
			return Ok(newToken);
		}
	}
}
