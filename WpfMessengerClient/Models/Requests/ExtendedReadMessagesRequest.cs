using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос на прочтение сообщения
    /// </summary>
    public class ExtendedReadMessagesRequest
    {
        public List<int> MessagesId { get; set; }

        /// <summary>
        /// Id пользователя прочитавшего сообщения
        /// </summary>
        public int UserId { get; set; }

        public int DialogId { get; set; }

        public ExtendedReadMessagesRequest(List<int> messagesId, int userId, int dialogId)
        {
            MessagesId = messagesId;
            UserId = userId;
            DialogId = dialogId;
        }
    }
}
