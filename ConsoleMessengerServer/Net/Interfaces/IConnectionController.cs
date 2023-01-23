using DtoLib.NetworkServices;
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
    public interface IConnectionController
    {
        /// <summary>
        /// Обработчик получения сетевых сообщений
        /// </summary>
        public IServerNetworkMessageHandler ServerNetworkMessageHandler { set; }

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

        /// <summary>
        /// Оповестить о получении массива байтов 
        /// </summary>
        /// <param name="bytes">Массив полученных байтов</param>
        /// <param name="networkProvider">Id сетевого провайдера</param>
        public void NotifyBytesReceived(byte[] bytes, INetworkProvider networkProvider);

        public void AddNewSession(int userId, int networkProviderId);

        public Task BroadcastNetworkMessageToSenderAsync(byte[] messageBytes, int userId, int networkProviderId);

        public Task BroadcastNetworkMessageToInterlocutorAsync(byte[] messageBytes, int userId);

        public bool TryDisconnectUser(int userId, int networkPrividerId);
    }
}