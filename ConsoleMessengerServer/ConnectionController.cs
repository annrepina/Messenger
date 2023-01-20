using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Net.Interfaces;
using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Класс, который отвечает за соединение
    /// </summary>
    public class ConnectionController : IConnectionController
    {
        /// <summary>
        /// Словарь, который содержит пары: ключ - id агрегатора соединений пользователя 
        /// и значение - самого агрегатора
        /// </summary>
        private Dictionary<int, UserProxy> _userProxyList;

        /// <summary>
        /// Словарь, который временно хранит подключенных сетевых провайдеров, к которым пока не привязаны пользователи.
        /// Key: Id сетевого провайдера.
        /// Value - объект сетевого провайдера
        /// </summary>
        private Dictionary<int, ServerNetworkProvider> _networkProvidersBuffer;

        /// <inheritdoc cref="ServerNetworkMessageHandler"/>
        private IServerNetworkMessageHandler _serverNetworkMessageHandler;

        /// <summary>
        /// Сервер, который прослушивает входящие подключения
        /// </summary>
        public Server Server { get; set; }
        
        /// <summary>
        /// Обработчик сетевого сообщения
        /// </summary>
        public IServerNetworkMessageHandler ServerNetworkMessageHandler 
        { 
            set
            {
                _serverNetworkMessageHandler = value;
            }
        }

        public ConnectionController()
        {
            Server = new Server(this);
            _userProxyList = new Dictionary<int, UserProxy>();
            _networkProvidersBuffer = new Dictionary<int, ServerNetworkProvider>();
            //ServerNetworkMessageHandler = serverNetworkMessageHandler;
        }

        /// <summary>
        /// Асинхронный метод - начать работу контроллера
        /// Бывший метод NetworkMessageHandler - StartListeningConnectionsAsync
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            try
            {
                await Server.ListenIncomingConnectionsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //todo: Возможно убрать эту реализацию вообще
        /// <summary>
        /// Отключить конкретного клиента
        /// </summary>
        /// <param name="clientId">Идентификатор клиента</param>
        public void DisconnectClient(int clientId)
        {
            if (_userProxyList != null && _userProxyList.Count > 0)
            {
                _userProxyList.Remove(clientId);
            }
        }

        //todo Возможно убрать этот метод вообще
        public void DisconnectClients()
        {
            foreach (var client in _userProxyList.Values)
            {
                client.CloseAll();
            }
        }

        /// <summary>
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient)
        {
            ServerNetworkProvider networkProvider = new ServerNetworkProvider(tcpClient, this);

            _networkProvidersBuffer.Add(networkProvider.Id, networkProvider);

            Console.WriteLine($"Клиент {networkProvider.Id} подключился ");

            Task.Run(() => networkProvider.ProcessNetworkMessagesAsync());
        }        

        public Task ProcessNetworkMessage(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            throw new NotImplementedException();
        }

        public void NotifyBytesReceived(byte[] bytes, int networkProviderId)
        {
            _serverNetworkMessageHandler.ProcessDataAsync(bytes, networkProviderId);

            
        }

        //todo Реализовать вызов метода
        public void Dispose()
        {
            //DisconnectClients();

            Server.Stop();
        }

        ///// <summary>
        ///// Создать агрегатора всех подключений определенного пользователя
        ///// </summary>
        ///// <param name="userId">Идентификатор пользователя</param>
        ///// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        //public void AddUserProxy(int userId, ServerNetworkProvider serverNetworkProvider)
        //{
        //    if (_userProxyList.ContainsKey(userId))
        //    {
        //        _userProxyList[userId].AddConnection(serverNetworkProvider);
        //    }
        //    else
        //    {
        //        UserProxy userProxy = new UserProxy(userId);
        //        userProxy.AddConnection(serverNetworkProvider);

        //        _userProxyList.Add(userId, userProxy);
        //    }
        //}

        public void AddNewSession(int userId, int networkProviderId)
        {
            if (_userProxyList.ContainsKey(userId))
            {
                _userProxyList[userId].AddConnection(_networkProvidersBuffer[networkProviderId]);
            }
            else
            {
                UserProxy userProxy = new UserProxy(userId);
                userProxy.AddConnection(_networkProvidersBuffer[networkProviderId]);

                _userProxyList.Add(userId, userProxy);
            }

            _networkProvidersBuffer.Remove(networkProviderId);
        }

        public void SendResponseToVerifiedUser(byte[] response, int userId, int networkProviderId)
        {
            _userProxyList[userId].SendResponseAsync(response, networkProviderId);
        }

        public void SendResponseToNetworkProvider(byte[] response, int networkProviderId)
        {
            _networkProvidersBuffer[networkProviderId].Transmitter.SendNetworkMessageAsync(response);
        }
    }
}