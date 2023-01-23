using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Services;

namespace WpfMessengerClient
{
    public class ConnectionController : IConnectionController
    {
        public INetworkMessageHandler NetworkMessageHandler { get; set; }

        public INetworkProvider NetworkProvider { get; set; }

        public void NotifyBytesReceived(byte[] bytes)
        {
            NetworkMessageHandler.ProcessData(bytes);
        }

        public void SendRequest(byte[] bytes)
        {
            NetworkProvider.SendBytesAsync(bytes);
        }

        public ConnectionController()
        {
            NetworkProvider = new ClientNetworkProvider(this);
        }
    }
}