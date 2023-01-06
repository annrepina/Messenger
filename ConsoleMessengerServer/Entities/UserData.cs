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
    public class UserData
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; } 

        /// <summary>
        /// Человек
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// Идентификатор человека
        /// </summary>
        public int PersonId { get; set; }

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
    }
}