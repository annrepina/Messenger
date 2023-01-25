using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Класс, который представляет ответ на запрос о создании нового диалога
    /// </summary>
    public class CreateDialogResponse : Response
    {
        /// <summary>
        /// Идентификатор созданного диалога
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор первого сообщения
        /// </summary>
        public int MessageId { get; init; }

        public CreateDialogResponse() : base()
        {
        }

        public CreateDialogResponse(NetworkResponseStatus status) : base(status)
        {
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="dialogId">Идентификатор созданного диалога</param>
        /// <param name="messageId">Идентификатор первого сообщения</param>
        public CreateDialogResponse(int dialogId, int messageId, NetworkResponseStatus status) : base(status)
        {
            DialogId = dialogId;
            MessageId = messageId;
        }
    }
}