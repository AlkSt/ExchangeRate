using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ExchangeRateData.Models;

namespace ExchangeRateConsole
{
    /// <summary>
    /// Класс выполняет запрос на получение данных
    /// </summary>
    class RequestExecutor
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly string requestUrl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="url">Адрес запроса</param>
        public RequestExecutor(string url)
        {
            requestUrl = url;
        }

        /// <summary>
        /// Запрос на получение курсов валют.
        /// </summary>
        /// <returns>Список дней с курсами валют за каждый</returns>
        public List<DateCourses> GetAllDaysCourses()
        {
            Task<List<DateCourses>> task = Task.Run<List<DateCourses>>(async () => await GetAllDaysCoursesAsync(requestUrl));
            return task.Result;
        }

        /// <summary>
        /// Запрос на получение курсов валют за N дней начиная с текущего
        /// N - сожержится в настройках
        /// </summary>
        /// <param name="requestUrl">Адрес запроса</param>
        /// <returns>Список дней с курсами валют за каждый</returns>
        public static async Task<List<DateCourses>> GetAllDaysCoursesAsync(string requestUrl)
        {
            var courseValList = new List<DateCourses>();
            for (var i = 0; i < UserSettings.Default.CountOfDay; i++)
            {
                var dayString = $"?date_req={DateTime.Now.AddDays(-i).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";
                var response = _client.GetStreamAsync(requestUrl + dayString);
                var coursesOfDay = DesirealizeStreamOfday(await response);
                courseValList.Add(coursesOfDay);
            }
            return courseValList;
        }

        /// <summary>
        /// Десерализует информацию о курсах валют
        /// </summary>
        /// <param name="stream">Поток содержаший информацию о курсах валют в определенную даты</param>
        /// <returns>Элемент DateCourses содержащий курсы валют за один день</returns>
        private static DateCourses DesirealizeStreamOfday(Stream stream)
        {
            using StreamReader reader = new StreamReader(stream, CodePagesEncodingProvider.Instance.GetEncoding(1251));
            return (DateCourses)new XmlSerializer(typeof(DateCourses)).Deserialize(reader);
        }
    }
}
