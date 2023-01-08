using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс - сущность данных пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Пользователь онлайн?
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Список диалогов
        /// </summary>
        public List<Dialog> Dialogs { get; set; }

        /// <summary>
        /// Список сетевых провайдеров
        /// </summary>
        public List<ServerNetworkProviderEntity> NetworkProviders { get; init; }

        /// <summary>
        /// Список отправленных сообщений
        /// </summary>
        public List<Message> SentMessages { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public User()
        {
            Dialogs = new List<Dialog>();
            NetworkProviders = new List<ServerNetworkProviderEntity>();
            SentMessages = new List<Message>();
        }
    }
}