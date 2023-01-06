using DtoLib.NetworkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Интерфейс, который управляет работой с сетью
    /// </summary>
    public interface INetworkController : IServerNetworkMessageHandler
    {
        public Task RunNewBackClientAsync(TcpClient tcpClient);

        public void DisconnectClients();

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="clientId">Id клиента</param>
        public void RemoveClient(int clientId);
    }
}
