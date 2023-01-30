using CommonLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Представляет ответ на запрос об отправке сообщения для пользователя, который его отправил.
    /// Подтверждает, что сообщение отправлено.
    /// Обертка для идентификатора отправленного сообщения
    /// </summary>
    public class SendMessageResponse : Response
    {
        /// <summary>
        /// Идентификатор отправленного сообщения
        /// </summary>
        public int MessageId { get; init; }

        public SendMessageResponse(NetworkResponseStatus status) : base(status)
        {
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="id">Идентификатор сообщения</param>
        public SendMessageResponse(int id, NetworkResponseStatus status) : base(status)
        {
            MessageId = id;
        }
    }
}