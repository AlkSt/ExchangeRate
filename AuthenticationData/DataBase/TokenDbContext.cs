using System;
using AuthenticationData.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationData.DataBase
{
    /// <summary>
    /// Контекс для работы с БД токенов
    /// </summary>
    public class TokenDbContext : DbContext
    {
        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }

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

            modelBuilder.Entity<UserRefreshToken>(entity =>
            {
                entity.HasKey(e => e.UserName);
                entity.Property(e => e.RefreshToken).IsRequired();
            });

        }
    }
}
