using DtoLib.NetworkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net.Interfaces
{
    /// <summary>
    /// Интерфейс, который управляет работой с сетью
    /// </summary>
    public interface INetworkController : IServerNetworkMessageHandler
    {
        /// <summary>
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient);

        /// <summary>
        /// Отключить всех клиентов
        /// </summary>
        public void DisconnectClients();

        /// <summary>
        /// Отключить конкретного клиента
        /// </summary>
        /// <param name="clientId">Id клиента</param>
        public void DisconnectClient(int clientId);
    }
}