using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Ответ на запрос об удалении диалога
    /// </summary>
    public class DeleteDialogResponse
    {
        public NetworkResponseStatus Status { get; init; }

        public DeleteDialogResponse(NetworkResponseStatus status)
        {
            Status = status;
        }
    }
}
