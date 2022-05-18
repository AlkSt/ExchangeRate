using System.Collections.Generic;

namespace AuthenticationData
{
	/// <summary>
	/// Класс зарегестрированнных пользователей
	/// </summary>
    static class UserRecords
	{
		/// <summary>
		/// Словарь пользователей: ключ - логин, значение - пароль
		/// </summary>
		public static Dictionary<string, string> Users => new Dictionary<string, string>
			{
			{ "user1","password1"},
			{ "user2","password2"},
			{ "user3","password3"},
			};
	}
}
