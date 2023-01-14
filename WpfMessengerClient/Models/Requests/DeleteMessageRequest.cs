using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Данные, представляющие запрос на удаление сообщения
    /// </summary>
    public class DeleteMessageRequest
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public Message Message { get; set; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        public int DialogId { get; set; }

        /// <summary>
        /// Пользователь, удаливший сообщение
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DeleteMessageRequest()
        {
            Message = new Message();
            DialogId = 0;
            UserId = 0;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="dialogId">Идентификатор диалога</param>
        public DeleteMessageRequest(Message message, int dialogId, int userId)
        {
            Message = message;
            DialogId = dialogId;
            UserId = userId;
        }
    }
}