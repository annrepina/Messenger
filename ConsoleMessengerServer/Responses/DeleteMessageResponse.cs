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
    public class DeleteMessageResponse : IResponse
    {
        public NetworkResponseStatus Status { get; set; }

        public DeleteMessageResponse(NetworkResponseStatus status)
        {
            Status = status;
        }
    } 
}