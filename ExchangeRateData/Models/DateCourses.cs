using System;
using System.Collections.Generic;

namespace ExchangeRateData.Models
{
    /// <summary>
    /// Класс курсов валют за день
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute("ValCurs")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DateCourses
    {
        /// Дата строкой (для сериализации)
        [System.Xml.Serialization.XmlAttributeAttribute("Date")]
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.Text.Json.Serialization.JsonPropertyName("Date")]
        public string DateString { get; set; }

        /// Дата (для БД)
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime Date
        {
            get
            {
                return DateTime.Parse(DateString);
            }
            set
            {
                DateString = value.ToString("dd/MM/yyyy");
            }
        }

        /// Наименование 
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }

        /// Курсы валют
        [System.Xml.Serialization.XmlElementAttribute("Valute")]
        public List<CurrencyCourse> Valute { get; set; }


        public DateCourses()
        {
            this.Valute = new List<CurrencyCourse>();
        }

    }
}
