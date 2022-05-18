namespace AuthenticationData.Models
{
    /// <summary>
    /// Модель токена.
    /// </summary>
    public class AccessToken
    {
        //JWT токен доступа 
        public string Token { get; set; }

        //Токен обновления
        public string RefreshToken { get; set; }
    }
}
