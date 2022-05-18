using System;

namespace ExchangeRateData.DataBase
{
    public interface IChangeCourseInteractor<T>
    {
        /// <summary>
        /// Добавление записи или обновление уже имеющейся
        /// </summary>
        /// <param name="course">Запись за день</param>
        void InsertCourse(T course);

        /// <summary>
        /// Удаление устаревших записей
        /// </summary>
        /// <param name="lastDay">Последний актуальный день</param>
        void DeleteOldCourses(DateTime lastDay);
    }
}
