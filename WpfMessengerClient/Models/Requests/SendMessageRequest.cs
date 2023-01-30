using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос от клиента для сервера на отправку сообщения 
    /// </summary>
    public class SendMessageRequest 
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public Message Message { get; init; }

        /// <summary>
        /// Id диалога, в котором существует сообщение
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="dialogId">Id диалога</param>
        public SendMessageRequest(Message message, int dialogId)
        {
            Message = message;
            DialogId = dialogId;
        }
    }
}