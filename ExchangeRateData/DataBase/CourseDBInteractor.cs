using ExchangeRateData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExchangeRateData.DataBase
{
    public class CourseDBInteractor : IGetCoursesInteractor<DateCourses, CurrencyCourse>, IChangeCourseInteractor<DateCourses>
    {
        /// <summary>
        /// Добавление курсов за день или обновление уже имеющегося
        /// </summary>
        /// <param name="course">Курс за день</param>
        public void InsertCourse(DateCourses course)
        {
            using (var context = new CourseDBContext())
            {
                context.Database.EnsureCreated();

                var existCur = context.DateCourses.Find(course.Date);

                if (existCur != null)
                {
                    //Замена записи
                    existCur = course;
                }
                else
                {
                    //Добавление записи
                    context.DateCourses.Add(course);
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление устаревших записей о курсах валют за день
        /// </summary>
        /// <param name="lastDay">Последний актуальный день</param>
        public void DeleteOldCourses(DateTime lastDay)
        {
            using (var context = new CourseDBContext())
            {
                //Cписок устаревших
                var listOldVal = context.DateCourses.Where(i => i.Date.CompareTo(lastDay) < 0);

                foreach (var item in listOldVal)
                {
                    context.DateCourses.Remove(item);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Получение всех курсов валют за день с возможностью фильтрации
        /// </summary>
        /// <param name="selectorByDate">Фильтр по дате</param>
        /// <param name="selectorValiteByName">Фильтр по сокращенному имени</param>
        /// <param name="page">Странца</param>
        /// <returns>Список отфильтрованных курсов валют за день</returns>
        public async Task<List<DateCourses>> GetCoursesAsync(Expression<Func<DateCourses, bool>> selectorByDate,
            Expression<Func<DateCourses, IEnumerable<CurrencyCourse>>> selectorValiteByName, int page)
        {
            using (var context = new CourseDBContext())
            {
                int pageCount = page != 0 ? DBSettings.Default.PageCount : context.DateCourses.Count();
                int pageSkip = page != 0 ? pageCount - 1 * DBSettings.Default.PageCount: 0;
                var records = context.DateCourses
                    .Where(selectorByDate)
                    .Include(selectorValiteByName)
                    .Skip(pageSkip)
                    .Take(pageCount);

                return await records.ToListAsync();
            }
        }

        /// <summary>
        /// Получение последнего имеющегося курса валюты по идентификатору
        /// </summary>
        /// <param name="CurrenciesID">Идентификатор валюты</param>
        /// <returns>Последний имеющийся курс валюты</returns>
        public async Task<DateCourses> GetCourseByNumberAsync(string CurrenciesID)
        {
            using (var context = new CourseDBContext())
            {
                var record = context.DateCourses
                    .OrderByDescending(p => p.Date)
                    .Include(b => b.Valute.Where(i => i.ID == CurrenciesID));

                return await record.FirstOrDefaultAsync();
            }
        }
    }
}
