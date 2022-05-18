using AuthenticationData.Models;
using System.Linq;

namespace AuthenticationData.DataBase
{
	public class TokenServiceInteractor : ITokenServiceInteractor
	{
		/// <summary>
		/// Добавляение токена обновления для пользователя в БД.
		/// </summary>
		/// <param name="userRefreshToken"></param>
		/// <returns></returns>
		public void InsertUserRefreshToken(UserRefreshToken userRefreshToken)
		{

			using (var context = new TokenDbContext())
			{
				context.Database.EnsureCreated();

				if (context.UserRefreshToken.Find(userRefreshToken.UserName) is UserRefreshToken savedRefreshToken)
				{
					//Обновление если пользователь уже получал токен
					savedRefreshToken.RefreshToken = userRefreshToken.RefreshToken;
				}
				else
				{
					//Добавленеие токена нового пользователя
					context.UserRefreshToken.Add(userRefreshToken);
				}
				context.SaveChanges();
			}
		}
		/// <summary>
		/// Обновление токена обновления для пользователя в БД.
		/// </summary>
		/// <param name="username">Имя пользователя</param>
		/// <param name="oldRfreshToken">Старый токен обновления</param>
		/// <param name="newRfreshToken">Новый токен обновления</param>
		public void UpdateUserRefreshToken(string username, string oldRfreshToken, string newRfreshToken)
		{
			using (var context = new TokenDbContext())
			{
				var item = context.UserRefreshToken.FirstOrDefault(x => x.UserName == username && x.RefreshToken == oldRfreshToken);
				if (item != null)
				{
					item.RefreshToken = newRfreshToken;
					context.SaveChanges();
				}
			}
		}

		/// <summary>
		/// Получение сохраненного токена пользователя
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Данные о токене обновления пользователя</returns>
		public UserRefreshToken GetSavedRefreshToken(string userName)
		{
			using (var context = new TokenDbContext())
			{
				return context.UserRefreshToken.FirstOrDefault(x => x.UserName == userName && x.IsActive == true);
			}
		}

		/// <summary>
		/// Проверка наличия указанного пользователя
		/// </summary>
		/// <param name="users">Логин+пароль</param>
		/// <returns></returns>
		public bool IsValidUser(User users) => UserRecords.Users.Any(x => x.Key == users.Name && x.Value == users.Password);

	}
}
