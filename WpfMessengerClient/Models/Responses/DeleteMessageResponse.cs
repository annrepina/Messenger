using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Ответ на запрос об удалении сообщения
    /// </summary>
    public class DeleteMessageResponse
    {
        public NetworkResponseStatus Status { get; init; }

        public DeleteMessageResponse(NetworkResponseStatus status)
        {
            Status = status;
        }
    }
}
