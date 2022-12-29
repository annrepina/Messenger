using DtoLib.NetworkInterfaces;
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
    public abstract class Client
    {
        /// <summary>
        /// Поток, по которому будет осуществляться передача данных
        /// </summary>
        public static NetworkStream NetworkStream { get; set; }

        /// <summary>
        /// Обеспечивает клиентские соединения для сетевых служб tcp.
        /// </summary>
        public static TcpClient TcpClient { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Получатель сообщений от сервера
        /// </summary>
        public Receiver Receiver { get; private set; }

        /// <summary>
        /// Отправитель сообщений на сервер
        /// </summary>
        public Sender Sender { get; private set; }

        //public INetworkMessageHandler NetworkMessageHandler { get; set; }

        public Client()
        {
            Id = 0;
            Receiver = new Receiver(this);
            Sender = new Sender(this);
            //NetworkMessageHandler = null;
        }

        ///// <summary>
        ///// Констурктор по умолчанию
        ///// </summary>
        //public Client(INetworkMessageHandler networkMessageHandler) : this()
        //{
        //    NetworkMessageHandler = networkMessageHandler;
        //}

        public abstract Task GetNetworkMessageAsync(NetworkMessage message);
        //{ 
            //if(NetworkMessageHandler != null)
            //{
            //    if(message.CurrentCode == NetworkMessage.OperationCode.RegistrationCode || message.CurrentCode == NetworkMessage.OperationCode.AuthorizationCode)

            //    await Task.Run(() => NetworkMessageHandler.ProcessNetworkMessage(message, Id));
            //}


        //}
    }
}
