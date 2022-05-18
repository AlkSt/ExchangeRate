using AuthenticationData.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExchangeRateWebApi.TokenManage
{
	/// <summary>
	/// Класс управляющий выдачей токенов
	/// </summary>
	public class JWTManager : IJWTManager
	{

		private readonly IConfiguration _iconfiguration;
		public JWTManager(IConfiguration iconfiguration)
		{
			this._iconfiguration = iconfiguration;
		}

		/// <summary>
		/// Метод генерации токена
		/// </summary>
		/// <param name="users">Логин пароль пользователя</param>
		/// <returns>Токен доступа + токен обновления</returns>
		public AccessToken GenerateToken(string userName)
		{
			try
			{
				//Указание параметров токена
				var tokenHandler = new JwtSecurityTokenHandler();
				var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.Name, userName)
					}),
					Expires = DateTime.Now.AddMinutes(5),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
				};
				//Создание токена
				var token = tokenHandler.CreateToken(tokenDescriptor);
				var refreshToken = GenerateRefreshToken();
				return new AccessToken { Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Генерация токена обновления
		/// </summary>
		/// <returns></returns>
		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		/// <summary>
		/// Получение данных по токену
		/// </summary>
		/// <param name="token">Токен</param>
		/// <returns></returns>
		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var Key = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Key),
				ClockSkew = TimeSpan.Zero
			};

			//Получение данных из токена
			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

			//Проверка  полученого токена
			JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new SecurityTokenException("Некорректный токен.");
			}

			return principal;
		}
	}
}
