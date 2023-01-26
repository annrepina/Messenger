using DtoLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net.Interfaces
{
    public interface IServerNetworProvider : INetworkProvider
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Событие отключения
        /// </summary>
        public event Action<int> Disconnected;

        public event Action<byte[], IServerNetworProvider> BytesReceived;
    }
}
