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

        public ConnectionController()
        {
            NetworkProvider = new ClientNetworkProvider(this);
            NetworkProvider.Disconnected += OnDisconnected;
            NetworkProvider.BytesReceived += OnBytesReceived;
        }

        public void OnBytesReceived(byte[] bytes)
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

        public void OnDisconnected()
        {
            Disconnected?.Invoke();
        }
    }
}