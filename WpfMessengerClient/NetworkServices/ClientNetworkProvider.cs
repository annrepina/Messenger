#define DebugExceptions
using CommonLib.NetworkServices;
using CommonLib.NetworkServices.Interfaces;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.NetworkMessageProcessing;
using WpfMessengerClient.NetworkServices.Interfaces;

namespace WpfMessengerClient.Services
{
    /// <summary>
    /// Класс, который представляет собой сетевого провайдера в клиентском приложении
    /// Отвечает за подключение к сети и обмен данными в сети
    /// </summary>
    public class ClientNetworkProvider : IClientNetworkProvider
    {
        #region События

        /// <summary>
        /// Событие отключения от сети
        /// </summary>
        public event Action Disconnected;

        #endregion События

        #region Константы

        /// <summary>
        /// Ip хоста
        /// </summary>
        private const string Host = "127.0.0.1";

        /// <summary>
        /// Порт по которому будет осуществляться передача данных
        /// </summary>
        private const int Port = 8888;

        #endregion Константы

        #region Приватные поля

        /// <summary>
        /// Обеспечивает клиентские подключения для сетевых служб TCP
        /// </summary>
        private TcpClient _tcpClient;

        /// <inheritdoc cref="Transmitter"/>
        private ITransmitterAsync _transmitter;

        /// <summary>
        /// Свойство - провайдер подключен к серверу?
        /// </summary>
        private bool _isConnected;

        /// <inheritdoc cref="NetworkMessageHandler"/>
        private INetworkMessageHandler _networkMessageHandler;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Трансмиттер - отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter { set => _transmitter = value; }

        /// <summary>
        /// Предоставляет базовый поток данных для доступа к сети
        /// </summary>
        public NetworkStream NetworkStream { get; private set; }

        /// <summary>
        /// Обработчик сетевых сообщений
        /// </summary>
        public INetworkMessageHandler NetworkMessageHandler { set => _networkMessageHandler = value; }

        #endregion Свойства 

        #region Конструкторы

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public ClientNetworkProvider(INetworkMessageHandler networkMessageHandler) : base()
        {
            Transmitter = new Transmitter(this);
            _tcpClient = new TcpClient();
            _isConnected = false;
            NetworkMessageHandler = networkMessageHandler;
        }

        #endregion Конструкторы

        #region Методы связанные с сетью

        /// <summary>
        /// Подключиться к серверу асинхронно 
        /// </summary>
        private async Task ConnectAsync()
        {
            try
            {
                _tcpClient.Connect(Host, Port);
                NetworkStream = _tcpClient.GetStream();
                _isConnected = true;

                await Task.Run(() => ReadBytesAsync());

            }
            catch (Exception ex)
            {

#if DebugExceptions

                MessageBox.Show(ex.Message);
#endif
                NotifyDisconnected();
            }
        }

        /// <summary>
        /// Отправить массив байт асинхронно серверу
        /// </summary>
        /// <param name="data">Отправляемые данные </param>
        public async Task SendBytesAsync(byte[] data)
        {
            try
            {
                if (!_isConnected)
                    ConnectAsync();

                await _transmitter.SendNetworkMessageAsync(data);
            }
            catch (Exception ex)
            {
                NotifyDisconnected();

#if DebugExceptions

                MessageBox.Show(ex.Message);
#endif
            }
        }

        /// <summary>
        /// Уведомить об отключении от сети
        /// </summary>
        public void NotifyDisconnected()
        {
            Disconnected.Invoke();
            CloseConnection();
        }

        /// <summary>
        /// Читать в бесконечном цикле байты по сети
        /// </summary>
        /// <returns></returns>
        private async Task ReadBytesAsync()
        {
            try
            {
                while (true)
                {
                    byte[] data = await _transmitter.ReceiveBytesAsync();

                    _networkMessageHandler?.ProcessNetworkMessage(data);
                }
            }
            catch (Exception ex)
            {
                NotifyDisconnected();

#if DebugExceptions

                MessageBox.Show(ex.Message);
#endif
            }
        }

        /// <summary>
        /// Закрыть соединение с сетью
        /// </summary>
        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (_tcpClient != null)
                _tcpClient.Close();

            _isConnected = false;
        }

        #endregion Методы связанные с сетью
    }
}