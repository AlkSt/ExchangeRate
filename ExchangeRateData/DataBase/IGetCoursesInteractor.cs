using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExchangeRateData.DataBase
{
    public interface IGetCoursesInteractor<T, G>
    {
        /// <summary>
        /// Получение всех записей с возможностью фильтрации
        /// </summary>
        /// <param name="selectorByDate">Фильтр по дате</param>
        /// <param name="selectorValiteByName">Фильтр по сокращенному имени</param>
        /// <param name="page">Странца</param>
        /// <returns>Список отфильтрованных записей</returns>
        Task<List<T>> GetCoursesAsync(Expression<Func<T, bool>> selectorByDate, Expression<Func<T, IEnumerable<G>>> selectorValiteByName, int page);

        /// <summary>
        /// Получение последней имеющейся записи по идентификатору
        /// </summary>
        /// <param name="CurrenciesID">Идентификатор</param>
        /// <returns>Последняя имеющаяся запись</returns>
        Task<T> GetCourseByNumberAsync(string CurrenciesID);
    }
}
