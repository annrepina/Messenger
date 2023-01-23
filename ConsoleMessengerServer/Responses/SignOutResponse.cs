using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Ответ на запрос о выходе из программы 
    /// </summary>
    public class SignOutResponse : IResponse
    {
        public NetworkResponseStatus Status { get; set; }

        public SignOutResponse(NetworkResponseStatus status)
        {
            Status = status;
        }
    }
}
