using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    public class DeleteDialogRequestForClient
    {
        public int DialogId { get; init; }

        public DeleteDialogRequestForClient(int dialogId)
        {
            DialogId = dialogId;
        }
    }
}
