using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateData.Models
{
    /// <summary>
    /// Класс курса валюты
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute("ValCursValute")]
    public partial class CurrencyCourse
    {
        //Связь с курсом
        [System.Text.Json.Serialization.JsonIgnore]
        public DateCourses DateCourses { get; set; }

        //Внешний ключ для таблицы дат
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime DateCoursesDate { get; set; }

        /// Идентификатор валюты
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID { get; set; }

        /// Числовой код
        public ushort NumCode { get; set; }

        /// Символный код
        public string CharCode { get; set; }

        /// Номинал
        public uint Nominal { get; set; }

        /// Наименование валюты
        public string Name { get; set; }

        /// Курс
        public string Value { get; set; }

        public CurrencyCourse() { }
    }
}
