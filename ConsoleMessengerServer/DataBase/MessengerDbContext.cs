using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.DataBase
{
    /// <summary>
    /// Представляет сеанс с базой данный и предоставляет API для взаимодействия с базой данных
    /// </summary>
    public class MessengerDbContext : DbContext
    {
        /// <summary>
        /// Коллекция для сущности User, которая будет являться таблицей в базе данных
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Коллекция для сущности Dialog, которая будет являться таблицей в базе данных
        /// </summary>
        public DbSet<Dialog> Dialogs { get; set; }

        /// <summary>
        /// Коллекция для сущности Message, которая будет являться таблицей в базе данных
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MessengerDbContext()
        {
            //Database.EnsureDeleted();
        }

        /// <summary>
        /// Метод, который вызывается при создании экземпляра DbContext
        /// Задает конфигурацию для контекста базы данных
        /// </summary>
        /// <param name="optionsBuilder">Предоставляет API для конфигурации контекста базы данных</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            // получаем конфигурацию из файла appSettings.json
            builder.AddJsonFile("dataBaseConnectionSettings.json");

            // создаем конфигурацию
            var config = builder.Build();

            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString, builder => builder.EnableRetryOnFailure());

            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DialogConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
        }
    }
}