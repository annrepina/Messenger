using Common.NetworkServices.Interfaces;

namespace ConsoleMessengerServer.Net.Interfaces
{
    /// <summary>
    /// Интерфейс, который представляет собой сетевого провайдера на стороне сервера
    /// Отвечает за подключение к сети и обмен данными в сети
    /// </summary>
    public interface IServerNetworProvider : INetworkProvider
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Событие отключения
        /// </summary>
        public event Action<int> Disconnected;

        /// <summary>
        /// Событие получение запроса в виде массива байт отклиента
        /// </summary>
        public event Action<byte[], IServerNetworProvider> BytesReceived;
    }
}