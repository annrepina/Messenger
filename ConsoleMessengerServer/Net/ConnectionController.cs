using ConsoleMessengerServer.Net.Interfaces;
using DtoLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net
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
        private Dictionary<int, IServerNetworProvider> _networkProvidersBuffer;


        private IRequestController _requestController;

        /// <summary>
        /// Сервер, который прослушивает входящие подключения
        /// </summary>
        public Server Server { get; set; }

        public ConnectionController()
        {
            Server = new Server(this);
            _requestController = new RequestController(this);
            _userProxyList = new Dictionary<int, UserProxy>();
            _networkProvidersBuffer = new Dictionary<int, IServerNetworProvider>();
        }

        /// <summary>
        /// Асинхронный метод - начать работу контроллера
        /// Бывший метод RequestController - StartListeningConnectionsAsync
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

        /// <summary>
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient)
        {
            ServerNetworkProvider networkProvider = new ServerNetworkProvider(tcpClient/*, this*/);
            networkProvider.BytesReceived += ProcessRequest;

            _networkProvidersBuffer.Add(networkProvider.Id, networkProvider);

            Console.WriteLine($"Клиент {networkProvider.Id} подключился ");

            Task.Run(() => networkProvider.ProcessNetworkMessagesAsync());
        }

        private void ProcessRequest(byte[] data, IServerNetworProvider networkProvider)
        {
            byte[] response = _requestController.ProcessRequest(data, networkProvider);

            networkProvider.SendBytesAsync(response);
        }

        public void NotifyBytesReceived(byte[] bytes, IServerNetworProvider NetworkProvider)
        {
            byte[] response = _requestController.ProcessRequest(bytes, NetworkProvider);

            NetworkProvider.SendBytesAsync(response);
        }

        public async Task BroadcastToSenderAsync(byte[] messageBytes, int userId, int networkMessageId)
        {
            await _userProxyList[userId].BroadcastNetworkMessageAsync(messageBytes, networkMessageId);
        }

        public async Task BroadcastToInterlocutorAsync(byte[] messageBytes, int interlocutorId)
        {
            if (_userProxyList.ContainsKey(interlocutorId))
                await _userProxyList[interlocutorId].BroadcastNetworkMessageAsync(messageBytes);
        }

        //todo Реализовать вызов метода
        public void Dispose()
        {
            Server.Stop();
        }

        public void AddNewSession(int userId, int networkProviderId)
        {
            if (_userProxyList.ContainsKey(userId))
            {
                _userProxyList[userId].AddConnection(_networkProvidersBuffer[networkProviderId]);
            }
            else
            {
                UserProxy userProxy = new UserProxy(userId);
                userProxy.LastConnectionRemoved += OnUserProxyLastConnectioRemoved;
                userProxy.AddConnection(_networkProvidersBuffer[networkProviderId]);

                _userProxyList.Add(userId, userProxy);
            }

            _networkProvidersBuffer.Remove(networkProviderId);
        }

        private void OnUserProxyLastConnectioRemoved(int userProxyId)
        {
            _userProxyList[userProxyId].LastConnectionRemoved -= OnUserProxyLastConnectioRemoved;
            _userProxyList.Remove(userProxyId);
        }

        public void DisconnectUser(int userId, int networkProviderId)
        {
            _userProxyList[userId].RemoveConnection(networkProviderId);
        }

        public async Task BroadcastError(byte[] messageBytes, IServerNetworProvider networkProvider)
        {
            await networkProvider.SendBytesAsync(messageBytes);
        }
    }
}