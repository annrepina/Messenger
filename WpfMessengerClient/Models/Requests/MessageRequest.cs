using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос который содержит в себе сообщение и диалог, к которому оно принадлежит
    /// </summary>
    public class MessageRequest 
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
        /// Конструктор по умолчанию
        /// </summary>
        public MessageRequest()
        {
            Message = new Message();
            DialogId = 0;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="dialogId">Идентификатор диалога</param>
        public MessageRequest(Message message, int dialogId)
        {
            Message = message;
            DialogId = dialogId;
        }
    }
}