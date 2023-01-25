using DtoLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.NetworkServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConnectionController
    {
        public event Action Disconnected;

        INetworkMessageHandler NetworkMessageHandler { get; set; }

        IClientNetworkProvider NetworkProvider { get; set; }

        public void NotifyBytesReceived(byte[] bytes);

        public Task SendRequestAsync(byte[] bytes);

        public void CloseConnection();

        public void OnDisconnected();
    }

}
