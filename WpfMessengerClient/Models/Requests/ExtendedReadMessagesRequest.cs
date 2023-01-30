using System.Collections.Generic;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Расширенный запрос на прочтение сообщения от клиента к серверу
    /// </summary>
    public class ExtendedReadMessagesRequest
    {
        /// <summary>
        /// Список Id прочитанных сообщений 
        /// </summary>
        public List<int> MessagesId { get; set; }

        /// <summary>
        /// Id пользователя прочитавшего сообщения
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Id диалога которому принадлежат сообщения
        /// </summary>
        public int DialogId { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="messagesId">Список Id прочитанных сообщений </param>
        /// <param name="userId">Id пользователя прочитавшего сообщения</param>
        /// <param name="dialogId">Id диалога которому принадлежат сообщения</param>
        public ExtendedReadMessagesRequest(List<int> messagesId, int userId, int dialogId)
        {
            MessagesId = messagesId;
            UserId = userId;
            DialogId = dialogId;
        }
    }
}