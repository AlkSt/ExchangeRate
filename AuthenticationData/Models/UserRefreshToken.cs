namespace AuthenticationData.Models
{
	/// <summary>
	/// Модеб для токена обновлени в БД
	/// </summary>
	public class UserRefreshToken
	{
		//Логин пользователя
		public string UserName { get; set; }

		//Токен обновления
		public string RefreshToken { get; set; }

		//Активность токена
		public bool IsActive { get; set; } = true;
	}
}
