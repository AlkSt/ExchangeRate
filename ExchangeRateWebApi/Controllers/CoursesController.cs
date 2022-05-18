using ExchangeRateData.DataBase;
using ExchangeRateData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExchangeRateWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IGetCoursesInteractor<DateCourses, CurrencyCourse> _getCoursesInteractor;

        public CoursesController(IGetCoursesInteractor<DateCourses, CurrencyCourse> getCoursesInteractor)
        {
            this._getCoursesInteractor = getCoursesInteractor;
        }

        /// <summary>
        /// Получение данных о валютах с фильтацией
        /// </summary>
        /// <param name="date">Фильтр даты</param>
        /// <param name="name">Фильтр сокращенного имени</param>
        /// <param name="page">Фильтр номер страницы</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Сurrencies")]
        public async Task<IActionResult> Get(DateTime? date, string name, int? page)
        {
            Expression<Func<DateCourses, bool>> selectorByDate =
                date != null ? b => b.Date == date : b => b.Name != string.Empty;

            Expression<Func<DateCourses, IEnumerable<CurrencyCourse>>> selectorValiteByName =
                b => b.Valute.Where(name != null ? p => p.CharCode == name : p => p.CharCode != string.Empty);

            try
            {
                var courseList = await _getCoursesInteractor.GetCoursesAsync(selectorByDate, selectorValiteByName, page ?? 0);

                if (courseList.Count == 0)
                    return NotFound("Данные по запросу отсутствуют.");

                return Ok(courseList);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Получение последних данных по курсу валюты
        /// </summary>
        /// <param name="id">Идентификатор валюты</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Сurrency/{Id}")]
        public async Task<IActionResult> GetOneAsync(string id)
        {
            try
            {
                var course = await _getCoursesInteractor.GetCourseByNumberAsync(id);

                if (course.Valute.Count == 0)
                    return NotFound("Данные по запросу отсутствуют.");

                return Ok(course);

            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

    }
}
