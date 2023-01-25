using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DtoLib.NetworkServices;
using ConsoleMessengerServer.Net.Interfaces;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Сетевой провайдер на стороне сервера
    /// </summary>
    public class ServerNetworkProvider : NetworkProvider
    {
        /// <summary>
        /// Отвечает за работу с сетью
        /// </summary>
        public IConnectionController ConnectionController { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        /// <param name="connectionController">Отвечает за работу с сетью</param>
        public ServerNetworkProvider(TcpClient tcpClient, IConnectionController connectionController)
        {
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
                await ReadBytes();
            }
            catch (IOException)
            {
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
        public override void NotifyBytesReceived(byte[] data)
        {
            ConnectionController.NotifyBytesReceived(data, this);
        }

        public override async Task SendBytesAsync(byte[] data)
        {
            await _transmitter.SendNetworkMessageAsync(data);
        }
    }
}