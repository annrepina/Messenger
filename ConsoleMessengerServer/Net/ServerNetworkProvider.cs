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
    public class ServerNetworkProvider : /*NetworkProvider,*/ IServerNetworProvider
    {
        public event Action<int> Disconnected;

        /// <summary>
        /// Счетчик, на основе которого, объекту присваивается id
        /// </summary>
        private static int _counter = 0;
        private ITransmitterAsync _transmitter;

        /// <summary>
        /// Отвечает за работу с сетью
        /// </summary>
        public IConnectionController ConnectionController { get; set; }
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
        /// <param name="connectionController">Отвечает за работу с сетью</param>
        public ServerNetworkProvider(TcpClient tcpClient, IConnectionController connectionController)
        {
            Id = ++_counter;
            Transmitter = new Transmitter(this);
            TcpClient = tcpClient;
            NetworkStream = TcpClient.GetStream();
            ConnectionController = connectionController;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void NotifyBytesReceived(byte[] data)
        {
            ConnectionController.NotifyBytesReceived(data, this);
        }

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
            //try
            //{
                while (true)
                {
                    // буфер для получаемых данных
                    byte[] data = await _transmitter.ReceiveBytesAsync();

                    NotifyBytesReceived(data);
                }
            //}
            //catch (IOException)
            //{
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
        }
    }
}