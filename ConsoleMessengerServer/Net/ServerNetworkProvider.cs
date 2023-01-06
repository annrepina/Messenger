using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DtoLib.NetworkServices;
using DtoLib.NetworkInterfaces;
using DtoLib;

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
        /// Начать обработку сетевых сообщений асинхронно
        /// </summary>
        public async Task StartProcessingNetworkMessagesAsync()
        {
            try
            {
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        await Receiver.ReceiveNetworkMessageAsync();
                    }
                    catch (Exception)
                    {
                        break;                      
                    }
                }
            }

            finally
            {
                NetworkController.RemoveClient(Id);
                CloseConnection();
            }
        }

        /// <summary>
        /// Закрыть подлючения
        /// </summary>
        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (TcpClient != null)
                TcpClient.Close();
        }

        /// <summary>
        /// Асинхронный метод получения сетевого сообщения
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <returns></returns>
        public override async Task GetNetworkMessageAsync(NetworkMessage message)
        {
            if(message.CurrentCode == NetworkMessage.OperationCode.AuthorizationCode || message.CurrentCode == NetworkMessage.OperationCode.RegistrationCode)
                await NetworkController.ProcessNetworkMessageAsync(message, Id);
          
            else
                await NetworkController.ProcessNetworkMessageAsync(message);
        }
    }
}