using AuthenticationData.Models;

namespace AuthenticationData.DataBase
{
    public interface ITokenServiceInteractor
	{
		/// <summary>
		/// Проверка наличия указанного пользователя.
		/// </summary>
		/// <param name="users">Логин+пароль</param>
		/// <returns></returns>
		bool IsValidUser(User users);

		/// <summary>
		/// Добавляение токена обновления для пользователя в БД.
		/// </summary>
		/// <param name="userRefreshToken"></param>
		/// <returns></returns>
		void InsertUserRefreshToken(UserRefreshToken userRefreshToken);

		/// <summary>
		/// Обновление токена обновления для пользователя в БД.
		/// </summary>
		/// <param name="username">Имя пользователя</param>
		/// <param name="oldRfreshToken">Старый токен обновления</param>
		/// <param name="newRfreshToken">Новый токен обновления</param>
		void UpdateUserRefreshToken(string username, string oldRfreshToken, string newRfreshToken);

		/// <summary>
		/// Получение сохраненного токена пользователя
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Данные о токене обновления пользователя</returns>
		UserRefreshToken GetSavedRefreshToken(string userName);

	}
}
