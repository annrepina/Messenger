using CommonLib.NetworkServices.Interfaces;
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
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient);

        /// <summary>
        /// Оповестить о получении массива байтов 
        /// </summary>
        /// <param name="bytes">Массив полученных байтов</param>
        /// <param name="networkProvider">Id сетевого провайдера</param>
        public void NotifyBytesReceived(byte[] bytes, IServerNetworProvider networkProvider);

        public void AddNewSession(int userId, int networkProviderId);

        public Task BroadcastToSenderAsync(byte[] messageBytes, int userId, int networkProviderId);

        public Task BroadcastToInterlocutorAsync(byte[] messageBytes, int userId);

        public Task BroadcastError(byte[] messageBytes, IServerNetworProvider networkProvider);

        public void DisconnectUser(int userId, int networkPrividerId);
    }
}