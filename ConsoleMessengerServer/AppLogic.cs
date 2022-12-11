using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib;
using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer
{
    public class AppLogic : INetworkMessageHandler
    {
        //public Server Server { get; set; }

        public delegate Task NetworkMessageSent(NetworkMessage message);

        public event NetworkMessageSent OnNetworkMessageSent;

        public AppLogic()
        {

        }

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            switch(message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    {
                        OnNetworkMessageSent?.Invoke(message);
                    }
                    break;
            }
        }
    }
}
