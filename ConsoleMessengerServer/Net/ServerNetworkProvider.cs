using Common.NetworkServices;
using Common.NetworkServices.Interfaces;
using ConsoleMessengerServer.Net.Interfaces;
using System.Net.Sockets;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Класс, который представляет собой сетевого провайдера в серверном приложении
    /// Отвечает за подключение к сети и обмен данными в сети
    /// </summary>
    public class ServerNetworkProvider : IServerNetworProvider
    {
        #region События

        /// <summary>
        /// Событие отключения
        /// </summary>
        public event Action<int> Disconnected;

        /// <summary>
        /// Событие получение запроса в виде массива байт отклиента
        /// </summary>
        public event Action<byte[], IServerNetworProvider> BytesReceived;

        #endregion События

        #region Приватные поля

        /// <summary>
        /// Счетчик, на основе которого, объекту присваивается id
        /// </summary>
        private static int _counter = 0;

        /// <summary>
        /// Трансмиттер который отвечает за пересылку байтов по сети асинхронно
        /// </summary>
        private ITransmitterAsync _transmitter;

        /// <summary>
        /// Обеспечивает клиентские подключения для сетевых служб TCP
        /// </summary>
        private TcpClient _tcpClient;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter { set => _transmitter = value; }

        /// <summary>
        /// Предоставляет базовый поток данных для доступа к сети
        /// </summary>
        public NetworkStream NetworkStream { get; init; }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public ServerNetworkProvider(TcpClient tcpClient)
        {
            Id = ++_counter;
            Transmitter = new Transmitter(this);
            _tcpClient = tcpClient;
            NetworkStream = _tcpClient.GetStream();
        }

        #endregion Конструкторы

        #region Методы взаимодействия с сетью

        /// <summary>
        /// Обрабатывает сетевые сообщений асинхронно
        /// </summary>
        public async Task ProcessNetworkMessagesAsync()
        {
            try
            {
                await ReadBytesAsync();
            }
            catch (IOException)
            {
                NotifyDisconnected();

                var dateTime = DateTime.Now;

                Console.WriteLine($"[{dateTime.ToString("dd.MM.yyyy HH:mm:ss")}] Клиент Id:{Id} отключился.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Отправляет сетевое сообщение в виде массива байт асинхронно
        /// </summary>
        /// <param name="networkMessageBytes">Сетевое сообщение в виде массива байт</param>
        public async Task SendBytesAsync(byte[] networkMessageBytes)
        {
            await _transmitter.SendNetworkMessageAsync(networkMessageBytes);
        }

        /// <summary>
        /// Уведомляет об отключении перед тем как отключиться
        /// </summary>
        public void NotifyDisconnected()
        {
            Disconnected.Invoke(Id);
            CloseConnection();
        }

        /// <summary>
        /// Считывает запросы от клинта в виде массива байт
        /// </summary>
        public async Task ReadBytesAsync()
        {
            while (true)
            {
                byte[] data = await _transmitter.ReceiveBytesAsync();

                BytesReceived?.Invoke(data, this);
            }
        }

        /// <summary>
        /// Закрывает соединение
        /// </summary>
        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (_tcpClient != null)
                _tcpClient.Close();
        }

        #endregion Методы взаимодействия с сетью
    }
}