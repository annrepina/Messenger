using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    /// <summary>
    /// Клиент, который подключается к серверу
    /// </summary>
    public class Client
    {
        //// определение типа делегата
        //public delegate Task NetworkMessageGot(NetworkMessage message);

        //public event NetworkMessageGot OnNetworkMessageGot;

        ///// <summary>
        ///// Ip хоста
        ///// </summary>
        //private const string Host = "127.0.0.1";

        ///// <summary>
        ///// Порт по которому будет осуществляться передача данных
        ///// </summary>
        //private const int Port = 8888;

        /// <summary>
        /// Поток, по которому будет осуществляться передача данных
        /// </summary>
        public static NetworkStream NetworkStream { get; set; }

        /// <summary>
        /// Обеспечивает клиентские соединения для сетевых служб tcp.
        /// </summary>
        public static TcpClient TcpClient { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// Получатель сообщений от сервера
        /// </summary>
        public Receiver Receiver { get; private set; }

        /// <summary>
        /// Отправитель сообщений на сервер
        /// </summary>
        public Sender Sender { get; private set; }

        public INetworkMessageHandler NetworkMessageHandler { get; set; }

        //public bool IsConnected { get; private set; }

        //public NetworkMessage NetworkMessage { get; private set; }

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public Client(INetworkMessageHandler networkMessageHandler)
        {
            Id = 0;
            //TcpClient = new TcpClient();
            Receiver = new Receiver(this);
            Sender = new Sender(this);
            NetworkMessageHandler = networkMessageHandler;
        }

        ///// <summary>
        ///// Подключиться
        ///// </summary>
        //public async Task ConnectAsync(NetworkMessage message)
        //{
        //    if (!TcpClient.Connected)
        //    {
        //        TcpClient.Connect(Host, Port);
        //        NetworkStream = TcpClient.GetStream();

        //        if (message != null)
        //        {
        //            await Sender.SendNetworkMessageAsync(message);


        //            //Sender.SendOperarationCode(Receiver.AddingNewUserCode);

        //            //Sender.SendMessage(userName);
        //        }

        //        await Task.Run(() => Receiver.ReceiveNetworkMessageAsync());
        //    }
        //}

        //public virtual async Task GetNetworkMessageAsync(NetworkMessage message)
        //{
        //    await Task.Run(() => OnNetworkMessageGot?.Invoke(message));
        //}

        public async Task GetNetworkMessageAsync(NetworkMessage message)
        {
            await Task.Run(() => NetworkMessageHandler.ProcessNetworkMessage(message));
        }
    }
}
