using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// запрос о прочтении сообщения для клиента
    /// </summary>
    public class ReadMessagesRequest
    {
        public List<int> MessagesId { get; set; }

        public int DialogId { get; set; }

        public ReadMessagesRequest()
        {
            MessagesId = new List<int>();
        }

        public ReadMessagesRequest(List<int> messagesId, int dialogId)
        {
            MessagesId = messagesId;
            DialogId = dialogId;
        }      
    }
}
