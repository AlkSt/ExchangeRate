using System;

namespace ExchangeRateConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Получение данных
            var requestExecutor = new RequestExecutor("http://www.cbr.ru/scripts/XML_daily.asp");
            var valCourseList = requestExecutor.GetAllDaysCourses();
            Console.WriteLine("Данные о курсах валют получены из сервиса.\n");

            //Обновление данных в БД
            var manager = new DBManager();
            manager.UpdateCourses(valCourseList);
            Console.WriteLine("Данные  о курсах валют в БД обновлены.\n");
        }
    }
}
