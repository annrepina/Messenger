using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DtoLib.NetworkServices;
using ConsoleMessengerServer.Net.Interfaces;
using DtoLib.NetworkServices.Interfaces;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Сетевой провайдер на стороне сервера
    /// </summary>
    public class ServerNetworkProvider : IServerNetworProvider
    {
        public event Action<int> Disconnected;

        public event Action<byte[], IServerNetworProvider> BytesReceived;

        /// <summary>
        /// Счетчик, на основе которого, объекту присваивается id
        /// </summary>
        private static int _counter = 0;

        private ITransmitterAsync _transmitter;

        public int Id { get; set; }
        
        /// <summary>
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter
        {
            set
            {
                _transmitter = value;
            }
        }

        public NetworkStream NetworkStream { get; set ; }
        public TcpClient TcpClient { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public ServerNetworkProvider(TcpClient tcpClient)
        {
            Id = ++_counter;
            Transmitter = new Transmitter(this);
            TcpClient = tcpClient;
            NetworkStream = TcpClient.GetStream();
        }

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
                Disconnect();

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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        //public void NotifyBytesReceived(byte[] data)
        //{
        //    //BytesReceived?.Invoke(data, )

        //    //ConnectionController.NotifyBytesReceived(data, this);
        //}

        public async Task SendBytesAsync(byte[] data)
        {
            await _transmitter.SendNetworkMessageAsync(data);
        }

        public void Disconnect()
        {
            Disconnected.Invoke(Id);
            CloseConnection();
        }

        public async Task ReadBytesAsync()
        {
            while (true)
            {
                // буфер для получаемых данных
                byte[] data = await _transmitter.ReceiveBytesAsync();

                BytesReceived?.Invoke(data, this);
            }
        }

        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (TcpClient != null)
                TcpClient.Close();
        }
    }
}