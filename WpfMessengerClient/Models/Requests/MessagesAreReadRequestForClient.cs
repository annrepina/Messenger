using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    public class MessagesAreReadRequestForClient
    {
        public List<int> MessagesId { get; set; }

        public int DialogId { get; set; }

        public MessagesAreReadRequestForClient()
        {
            MessagesId = new List<int>();
        }

        public MessagesAreReadRequestForClient(List<int> messagesId, int dialogId)
        {
            MessagesId = messagesId;
            DialogId = dialogId;
        }      
    }
}
