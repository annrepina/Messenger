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
    /// Клиент, который подключается к серверу
    /// </summary>
    public class BackClient : Client
    {
        public INetworkHandler NetworkHandler { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public BackClient(TcpClient tcpClient) : base()
        {
            TcpClient = tcpClient;
            //_server = server;
            NetworkStream = TcpClient.GetStream();
            //UserAccount = ;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public BackClient(TcpClient tcpClient, /*Server server, */INetworkHandler networkHandler)
        {
            TcpClient = tcpClient;
            NetworkStream = TcpClient.GetStream();
            NetworkHandler = networkHandler;
        }

        /// <summary>
        /// Обработать данные
        /// </summary>
        public async Task ProcessDataAsync()
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                NetworkHandler.RemoveClient(Id);
                CloseConnection();
            }
        }



        ///// <summary>
        ///// Получить имя пользователя
        ///// </summary>
        //private void GetUserName()
        //{
        //    string message = GetMessage();

        //    Name = message;

        //    //message = $"{Name} вошел/вошла в чат";

        //    // отправить всем сообщение о том, что добавляется новый человек
        //    _server.BroadcastOperationCode(AddingNewUserCode, Id);

        //    // Отправить всем имя этого человека
        //    _server.BroadcastMessage(Name, Id);

        //    Console.WriteLine($"{Name} вошел/вошла в чат");
        //    //Console.WriteLine(message);
        //}

        /// <summary>
        /// Закрытие подлючения
        /// </summary>
        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (TcpClient != null)
            {
                TcpClient.Close();
            }

        }

        public override async Task GetNetworkMessageAsync(NetworkMessage message)
        {
            if(message.CurrentCode == NetworkMessage.OperationCode.AuthorizationCode || message.CurrentCode == NetworkMessage.OperationCode.RegistrationCode)
                await Task.Run(() => NetworkHandler.ProcessNetworkMessage(message, Id));
          
            else
                await Task.Run(() => NetworkHandler.ProcessNetworkMessage(message));
        }
    }
}