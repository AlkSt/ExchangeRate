using ExchangeRateData.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExchangeRateData.DataBase
{
    /// <summary>
    /// Контекс для работы с БД курсов валют
    /// </summary>
    public class CourseDBContext : DbContext
    {
        public DbSet<DateCourses> DateCourses { get; set; }

        public DbSet<CurrencyCourse> CurrencyCourse { get; set; }

        /// <summary>
        /// Конфигурация БД
        /// </summary>
        /// <param name="optionsBuilder">Конструктор настроек</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                $"server={DBSettings.Default.Server};" +
                $"user={DBSettings.Default.User};" +
                $"password={DBSettings.Default.Password};" +
                $"database={DBSettings.Default.Name};",
                new MySqlServerVersion(new Version(8, 0, 11)));
        }

        /// <summary>
        /// Настройка свойствв в модели.
        /// </summary>
        /// <param name="modelBuilder">Конструктор модели</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DateCourses>(entity =>
            {
                entity.HasKey(e => e.Date);
            });

            modelBuilder.Entity<CurrencyCourse>(entity =>
            {
                entity.HasKey(e => new { e.ID, e.DateCoursesDate });
                entity.Property(e => e.Value).IsRequired();
                entity.HasOne(d => d.DateCourses)
                  .WithMany(p => p.Valute)
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
