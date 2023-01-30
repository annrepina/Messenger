using ConsoleMessengerServer.Net.Interfaces;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Класс - агрегатор всех соединений одного пользователя
    /// </summary>
    public class UserProxy
    {
        /// <summary>
        /// Событие - удалено последнее соединение
        /// </summary>
        public event Action<int> LastConnectionRemoved;

        /// <summary>
        /// Список сетевых провайдеров, которые подключены к серверу 
        /// и в которых выполнен вход в учетную запись пользователя
        /// </summary>
        private List<IServerNetworProvider> _networkProviders;

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
            _networkProviders = new List<IServerNetworProvider>();
        }

        /// <summary>
        /// Добавить соединение
        /// </summary>
        /// <param name="serverNetworkProvider">Сетевой провайдер, на котором произошло подключение</param>
        public void AddConnection(IServerNetworProvider serverNetworkProvider)
        {
            _networkProviders.Add(serverNetworkProvider);
            serverNetworkProvider.Disconnected += RemoveConnection;
        }

        /// <summary>
        /// Удалить соединение с сетевым провайдером
        /// </summary>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        public void RemoveConnection(int networkProviderId)
        {
            var provider = _networkProviders.First(pr => pr.Id == networkProviderId);

            _networkProviders.Remove(provider);
        }

        /// <summary>
        /// Транслировать асинхронно сетевое сообщение всем сетевым провайдерам на которых подключен пользователь
        /// </summary>
        /// <param name="messageBytes">Сетевое сообщение в виде массива байт</param>
        public async Task BroadcastNetworkMessageAsync(byte[] messageBytes)
        {
            foreach (ServerNetworkProvider serverNetworkProvider in _networkProviders)
            {
                await serverNetworkProvider.SendBytesAsync(messageBytes);
            }
        }

        /// <summary>
        /// Транслировать асинхронно сетевое сообщение всем сетевым провайдерам на которых подключен пользователь, кроме того, который передан в метод
        /// </summary>
        /// <param name="messageBytes">Сетевое сообщение в виде массива байт</param>
        /// <param name="networkProviderId">Id cетевого провайдера</param>
        /// <returns></returns>
        public async Task BroadcastNetworkMessageAsync(byte[] messageBytes, int networkProviderId)
        {
            foreach (ServerNetworkProvider serverNetworkProvider in _networkProviders)
            {
                if (serverNetworkProvider.Id != networkProviderId)
                    await serverNetworkProvider.SendBytesAsync(messageBytes);
            }
        }
    }
}