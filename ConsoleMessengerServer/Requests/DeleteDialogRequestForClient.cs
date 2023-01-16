using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Requests
{
    /// <summary>
    /// Запрос об удалении диалога для клиентского приложения
    /// </summary>
    public class DeleteDialogRequestForClient
    {
        public int DialogId { get; init; }

        public DeleteDialogRequestForClient(int dialogId)
        {
            DialogId = dialogId;
        }
    }
}
