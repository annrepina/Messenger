using System.Net.Sockets;

namespace ConsoleMessengerServer.Net.Interfaces
{
    /// <summary>
    /// Интерфейс, который отвечает за соединение по сети с клиентами
    /// </summary>
    public interface IConnectionController
    {
        /// <summary>
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient);

        /// <summary>
        /// Добавить новую сессию
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="networkProviderId">Id сетевого провайдера через который подключился пользователь</param>
        public void AddNewSession(int userId, int networkProviderId);

        /// <summary>
        /// Транслировать сообщения всем клиентам, на которых авторизован пользователь кроме указанного
        /// </summary>
        /// <param name="messageBytes">Сетевое сообщение представленное массивом байт</param>
        /// <param name="userId">Id пользователя</param>
        /// <param name="networkProviderId">Id сетевого провайдера, которому не нужно отправлять сообщение</param>
        public Task BroadcastToSenderAsync(byte[] messageBytes, int userId, int networkProviderId);

        /// <summary>
        /// Транслировать сообщения всем клиентам, на которых авторизован пользователь-собеседник
        /// </summary>
        /// <param name="messageBytes">Сетевое сообщение представленное массивом байт</param>
        /// <param name="userId">Id пользователя</param>
        public Task BroadcastToInterlocutorAsync(byte[] messageBytes, int userId);

        /// <summary>
        /// Транслировать ошибку клиенту, который отправил запрос
        /// </summary>
        /// <param name="messageBytes">Сетевое сообщение представленное массивом байт</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public Task BroadcastError(byte[] messageBytes, IServerNetworProvider networkProvider);

        /// <summary>
        /// Отключить пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="networkPrividerId">Id сетевого провайдера</param>
        public void DisconnectUser(int userId, int networkPrividerId);
    }
}