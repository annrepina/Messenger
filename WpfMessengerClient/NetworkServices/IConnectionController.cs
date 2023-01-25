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
        INetworkMessageHandler NetworkMessageHandler { get; set; }

        INetworkProvider NetworkProvider { get; set; }

        public void NotifyBytesReceived(byte[] bytes);

        public void SendRequest(byte[] bytes);
    }

}
