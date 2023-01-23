using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс - сущность сообщения
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Свойство - идентификатор сообщения
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Свойство - текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Свойство - данные о пользователе - отправителе сообщения
        /// </summary>
        public User UserSender { get; set; }

        /// <summary>
        /// Свойство - идентификатор данных о пользователе - отправителе сообщения
        /// </summary>
        public int UserSenderId { get; set; }

        /// <summary>
        /// Свойство - диалог в котором существует сообщение
        /// </summary>
        public Dialog Dialog { get; set; }

        /// <summary>
        /// Свойство - идентификатор диалога в котором существует сообщение
        /// </summary>
        public int DialogId { get; set; }

        ///// <summary>
        ///// Свойство - прочитано сообщение?
        ///// </summary>
        //public bool IsRead { get; set; }

        /// <summary>
        /// Свойство - дата и время отправки сообщения
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}