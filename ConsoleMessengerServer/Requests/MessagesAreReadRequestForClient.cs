using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Requests
{
    /// <summary>
    /// Запрос на то, чтобы уведомить клиентское приложение о том, что сообщение прочитаны пользователем на другом клиентсвом приложении
    /// </summary>
    public class MessagesAreReadRequestForClient
    {
        public List<int> MessagesId { get; set; }

        public int DialogId { get; set; }

        public MessagesAreReadRequestForClient(List<int> messagesId, int dialogId)
        {
            MessagesId = messagesId;
            DialogId = dialogId;
        }
    }
}
