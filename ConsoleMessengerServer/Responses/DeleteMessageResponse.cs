using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Ответ на запрос на удаление сообщения
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