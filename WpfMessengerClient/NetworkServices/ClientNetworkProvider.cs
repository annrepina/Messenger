using DtoLib.NetworkServices;
using DtoLib.NetworkServices.Interfaces;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.NetworkServices;

namespace WpfMessengerClient.Services
{
    /// <summary>
    /// Сетевой провайдер на стороне клиента
    /// </summary>
    public class ClientNetworkProvider : /*NetworkProvider, */IClientNetworkProvider
    {
        public event Action Disconnected;
        public event Action<byte[]> BytesReceived;

        private ITransmitterAsync _transmitter;

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

        #region Свойства

        /// <summary>
        /// Свойство - провайдер подключен к серверу?
        /// </summary>
        public bool IsConnected { get; set; }

        public IConnectionController ConnectionController { get; set; }

        /// <summary>
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter { set => _transmitter = value; }

        public NetworkStream NetworkStream { get; set; }
        public TcpClient TcpClient { get; set; }

        #endregion Свойства 

        #region Конструкторы

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public ClientNetworkProvider(IConnectionController connectionController) : base()
        {
            Transmitter = new Transmitter(this);
            TcpClient = new TcpClient();
            IsConnected = false;
            ConnectionController = connectionController;
        }

        #endregion Конструкторы

        #region Методы связанные с сетью

        /// <summary>
        /// Подключиться к серверу асинхронно 
        /// </summary>
        public async Task ConnectAsync(/*byte[] messageBytes*/)
        {
            try
            {
                if (!IsConnected)
                {
                    TcpClient.Connect(Host, Port);
                    NetworkStream = TcpClient.GetStream();
                    IsConnected = true;

                    //if (messageBytes != null)
                    //{
                    //    await _transmitter.SendNetworkMessageAsync(messageBytes);
                    //}

                    /*await */
                    Task.Run(() => ReadBytesAsync());
                }
            }
            catch (IOException)
            {
                Disconnect();
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show("Подключение прервано...");
                throw;
            }
        }

        //public void OnBytesReceived(byte[] data)
        //{
        //    ConnectionController.OnBytesReceived(data);
        //}

        public async Task SendBytesAsync(byte[] data)
        {
            try
            {
                if (!IsConnected)
                    ConnectAsync();

                await _transmitter.SendNetworkMessageAsync(data);
            }
            catch (IOException)
            {
                Disconnect();
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void Disconnect()
        {
            Disconnected.Invoke();
            //ConnectionController.NotifyDisconnect();
            CloseConnection();
        }

        public async Task ReadBytesAsync()
        {
            //try
            //{
            while (true)
            {
                byte[] data = await _transmitter.ReceiveBytesAsync();

                BytesReceived?.Invoke(data);
                //OnBytesReceived(data);
            }
            //}
            //catch (IOException)
            //{
            //    Disconnect();
            //    throw;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (TcpClient != null)
                TcpClient.Close();

            IsConnected = false;
        }

        #endregion Методы связанные с сетью
    }
}