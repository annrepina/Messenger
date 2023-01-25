using DtoLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Services;

namespace WpfMessengerClient.NetworkServices
{
    public class ConnectionController : IConnectionController
    {
        public event Action Disconnected;

        public INetworkMessageHandler NetworkMessageHandler { get; set; }

        public IClientNetworkProvider NetworkProvider { get; set; }

        public void NotifyBytesReceived(byte[] bytes)
        {
            NetworkMessageHandler.ProcessData(bytes);
        }

        public async Task SendRequestAsync(byte[] bytes)
        {
            await NetworkProvider.SendBytesAsync(bytes);
        }

        public void CloseConnection()
        {
            NetworkProvider.CloseConnection();
        }

        public ConnectionController()
        {
            NetworkProvider = new ClientNetworkProvider(this);
            NetworkProvider.Disconnected += OnDisconnected;
        }

        public void OnDisconnected()
        {
            Disconnected?.Invoke();
        }
    }
}