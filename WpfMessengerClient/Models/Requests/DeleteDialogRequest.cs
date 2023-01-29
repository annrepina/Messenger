using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос об удалении диалога для клиента
    /// </summary>
    public class DeleteDialogRequest
    {
        public int DialogId { get; init; }

        public DeleteDialogRequest(int dialogId)
        {
            DialogId = dialogId;
        }
    }
}
