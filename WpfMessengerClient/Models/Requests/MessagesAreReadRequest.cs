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
    public class MessagesAreReadRequest
    {
        //public int MessageId { get; set; }
        public List<int> MessagesId { get; set; }

        public MessagesAreReadRequest(List<int> messagesId)
        {
            MessagesId = messagesId;
        }
    }
}
