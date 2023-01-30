using CommonLib.NetworkServices;
using CommonLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    public class Response : IResponse
    {
        public NetworkResponseStatus Status { get; set; }

        public Response(NetworkResponseStatus status)
        {
            Status = status;
        }

        public Response()
        {
            //Status = NetworkResponseStatus.Successful;
        }
    }
}
