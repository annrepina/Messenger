using DtoLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DtoLib.NetworkServices;
using WpfMessengerClient.ViewModels;

namespace WpfMessengerClient.Services 
{
    /// <summary>
    /// Клиент, который подключается к серверу
    /// </summary>
    public class FrontClient : Client
    {
        //// определение типа делегата
        //public delegate void NetworkMessageGot(NetworkMessage message);

        //public event NetworkMessageGot OnNetworkMessageGot;

        /// <summary>
        /// Ip хоста
        /// </summary>
        private const string Host = "127.0.0.1";

        /// <summary>
        /// Порт по которому будет осуществляться передача данных
        /// </summary>
        private const int Port = 8888;

        ///// <summary>
        ///// Поток, по которому будет осуществляться передача данных
        ///// </summary>
        //public static NetworkStream Stream { get; private set; }

        ///// <summary>
        ///// Обеспечивает клиентские соединения для сетевых служб tcp.
        ///// </summary>
        //public static TcpClient TcpClient { get; private set; }

        //public int Id { get; set; }

        ///// <summary>
        ///// Получатель сообщений от сервера
        ///// </summary>
        //public Receiver Receiver { get; private set; }

        ///// <summary>
        ///// Отправитель сообщений на сервер
        ///// </summary>
        //public Sender Sender { get; private set; }

        public bool IsConnected { get; private set; }

        //public INetworkMessageHandler NetworkMessageHandler { get; set; }

        //public NetworkMessage NetworkMessage { get; private set; }

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public FrontClient(INetworkMessageHandler networkMessageHandler) : base(networkMessageHandler)
        {
            //Id = 0;
            TcpClient = new TcpClient();
            //NetworkMessageHandler = networkMessageHandler;
            //Receiver = new Receiver(this);
            //Sender = new Sender(this);
        }

        /// <summary>
        /// Подключиться
        /// </summary>
        public async Task ConnectAsync(NetworkMessage message)
        {
            if (!TcpClient.Connected)
            {
                TcpClient.Connect(Host, Port);
                NetworkStream = TcpClient.GetStream();

                if (message != null)
                {
                    await Sender.SendNetworkMessageAsync(message);


                    //Sender.SendOperarationCode(Receiver.AddingNewUserCode);

                    //Sender.SendMessage(userName);
                }

                await Receiver.ReceiveNetworkMessageAsync();
            }
        }

        //public async Task GetNetworkMessageAsync(NetworkMessage message)
        //{
        //    await Task.Run(() => OnNetworkMessageGot?.Invoke(message));
        //}

        //public override async Task GetNetworkMessageAsync(NetworkMessage message)
        //{
        //    await Task.Run(() => NetworkMessageHandler.ProcessNetworkMessage(message));
        //}
    }

}
