using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Представляет ответ на запрос об отправке сообщения для пользователя, который его отправил.
    /// Подтверждает, что сообщение отправлено.
    /// Обертка для идентификатора отправленного сообщения
    /// </summary>
    public class SendMessageResponse
    {
        /// <summary>
        /// Идентификатор отправленного сообщения
        /// </summary>
        public int MessageId { get; init; }

        //public NetworkResponseStatus Status { get; init; }

        public SendMessageResponse(int messageId)
        {
            MessageId = messageId;
        }
    }
}
