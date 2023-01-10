using ConsoleMessengerServer.Net;
using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Класс - аггрегатор всех соединений одного пользователя
    /// </summary>
    public class UserProxy
    {
        /// <summary>
        /// Список сетевых провайдеров, которые подключены к серверу 
        /// и в которых выполнен вход в учетную запись пользователя
        /// </summary>
        private List<ServerNetworkProvider> _connections;

        /// <summary>
        /// Свойство - идентификатор
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public UserProxy(int id)
        {
            Id = id;
            _connections = new List<ServerNetworkProvider>();
        }

        /// <summary>
        /// Добавить соединение
        /// </summary>
        /// <param name="serverNetworkProvider">Сетевой провайдер, на котором произошло подключение</param>
        public void AddConnection(ServerNetworkProvider serverNetworkProvider)
        {
            _connections.Add(serverNetworkProvider);
        }

        /// <summary>
        /// Транслировать асинхронно сетевое сообщение всем сетевым провайдерам на которых подключен пользователь
        /// </summary>
        /// <param name="networkMessage"></param>
        public async Task BroadcastNetworkMessageAsync(NetworkMessage networkMessage)
        {
            foreach (ServerNetworkProvider serverNetworkProvider in _connections)
            {
                await serverNetworkProvider.Sender.SendNetworkMessageAsync(networkMessage);
            }
        }

        /// <summary>
        /// Закрыть все соединения
        /// </summary>
        public void CloseAll()
        {
            foreach (var provider in _connections)
            {
                provider.CloseConnection();
            }
        }
    }
}
