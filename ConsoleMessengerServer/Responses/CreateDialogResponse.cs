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

        //public NetworkResponseStatus Status { get; set; }

        public CreateDialogResponse()
        {

        }

        //public CreateDialogResponse(NetworkResponseStatus status)
        //{
        //    Status = status;
        //}

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="dialogId">Идентификатор созданного диалога</param>
        /// <param name="messageId">Идентификатор первого сообщения</param>
        public CreateDialogResponse(int dialogId, int messageId/*, NetworkResponseStatus status*/)
        {
            DialogId = dialogId;
            MessageId = messageId;
            //Status = status;
        }

        //public override string ToString()
        //{
        //    if (Status == NetworkResponseStatus.Successful)
        //        return $"Диалог с Id {DialogId} успешно создан";

        //    return "Диалог не удалось создать";
        //}
    }
}