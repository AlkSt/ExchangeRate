using System;
using System.Collections.Generic;
using ExchangeRateData.DataBase;
using ExchangeRateData.Models;

namespace ExchangeRateConsole
{
    /// <summary>
    /// Класс для взаимодействия с БД валют.
    /// </summary>
    class DBManager
    {
        private readonly IChangeCourseInteractor<DateCourses> changeInteractor;
        public DBManager()
        {
            changeInteractor = new CourseDBInteractor();
        }

        /// <summary>
        /// Актуализирует данные о курсах валют в БД.
        /// </summary>
        /// <param name="coursesList">список с курсами за день</param>
        public void UpdateCourses(List<DateCourses> coursesList)
        {
            //Вносим новые записи
            foreach (var course in coursesList)
            {
                changeInteractor.InsertCourse(course);
            }

            //Получаем последний день
            var lastDay = DateTime.Now.AddDays(-1 * UserSettings.Default.CountOfDay).Date;
            //Удаляем все старые записи
            changeInteractor.DeleteOldCourses(lastDay);
        }
    }
}
