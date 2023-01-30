using System.Collections.Generic;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос от сервера для клиента о прочтении сообщения 
    /// </summary>
    public class ReadMessagesRequest
    {
        /// <summary>
        /// Список Id прочитанных сообщений 
        /// </summary>
        public List<int> MessagesId { get; set; }

        /// <summary>
        /// Id диалога которому принадлежат сообщения
        /// </summary>
        public int DialogId { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ReadMessagesRequest()
        {
            MessagesId = new List<int>();
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="messagesId">Список Id прочитанных сообщений </param>
        /// <param name="dialogId">Id диалога которому принадлежат сообщения</param>
        public ReadMessagesRequest(List<int> messagesId, int dialogId)
        {
            MessagesId = messagesId;
            DialogId = dialogId;
        }
    }
}