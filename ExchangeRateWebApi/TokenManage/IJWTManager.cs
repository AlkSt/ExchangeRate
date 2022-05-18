using AuthenticationData.Models;
using System.Security.Claims;

namespace ExchangeRateWebApi.TokenManage
{
    public interface IJWTManager
    {
        /// <summary>
        /// Метод генерации токена
        /// </summary>
        /// <param name="users">Логин пароль пользователя</param>
        /// <returns>Токен доступа + токен обновления</returns>
        AccessToken GenerateToken(string userName);

        /// <summary>
        /// Получение данных по токену
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
