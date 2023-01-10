using DtoLib.NetworkInterfaces;
using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net.Interfaces
{
    /// <summary>
    /// Интерфейс, который обрабатывает сетевые сообщения на стороне клиента
    /// </summary>
    public interface IServerNetworkMessageHandler : INetworkMessageHandler
    {
        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер на стороне сервера</param>
        public Task ProcessNetworkMessage(NetworkMessage message, ServerNetworkProvider serverNetworkProvider);
    }
}
