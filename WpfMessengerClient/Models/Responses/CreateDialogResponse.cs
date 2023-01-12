using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Класс, который представляет ответ на запрос о создании нового диалога
    /// </summary>
    public class CreateDialogResponse
    {
        /// <summary>
        /// Идентификатор созданного диалога
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор первого сообщения
        /// </summary>
        public int MessageId { get; init; }

        ///// <summary>
        ///// Время отправки сообщения
        ///// </summary>
        //public DateTime? MessageSendingDateTime { get; init; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="dialogId">Идентификатор созданного диалога</param>
        /// <param name="messageId">Идентификатор первого сообщения</param>
        public CreateDialogResponse(int dialogId, int messageId)
        {
            DialogId = dialogId;
            MessageId = messageId;
        }
    }
}