using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DtoLib.NetworkServices;
using DtoLib.NetworkInterfaces;
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
        public INetworkController NetworkController { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        /// <param name="networkController">Отвечает за работу с сетью</param>
        public ServerNetworkProvider(TcpClient tcpClient, INetworkController networkController)
        {
            TcpClient = tcpClient;
            NetworkStream = TcpClient.GetStream();
            NetworkController = networkController;
        }

        /// <summary>
        /// Обрабатывает сетевые сообщений асинхронно
        /// </summary>
        public async Task ProcessNetworkMessagesAsync()
        {
            try
            {
                await Receiver.ReceiveNetworkMessageAsync();
            }
            catch (Exception)
            {
                Console.WriteLine($"Клиент Id {Id} отключился");
            }
            finally
            {
                NetworkController.DisconnectClient(Id);
                CloseConnection();
            }
        }

        /// <summary>
        /// Метод получения сетевого сообщения
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <returns></returns>
        public override void GetNetworkMessage(NetworkMessage message)
        {
            if(message.Code == NetworkMessageCode.AuthorizationCode || message.Code == NetworkMessageCode.RegistrationCode
                || message.Code == NetworkMessageCode.SearchUserCode)
                NetworkController.ProcessNetworkMessage(message, this);
          
            else
                NetworkController.ProcessNetworkMessage(message);
        }
    }
}