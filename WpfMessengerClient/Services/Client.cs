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

namespace WpfMessengerClient.Services
{
    /// <summary>
    /// Клиент, который подключается к серверу
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Ip хоста
        /// </summary>
        private const string Host = "127.0.0.1";

        /// <summary>
        /// Порт по которому будет осуществляться передача данных
        /// </summary>
        private const int Port = 8888;

        /// <summary>
        /// Поток, по которому будет осуществляться передача данных
        /// </summary>
        public static NetworkStream Stream { get; private set; }

        /// <summary>
        /// Обеспечивает клиентские соединения для сетевых служб tcp.
        /// </summary>
        public static TcpClient TcpClient { get; private set; }

        /// <summary>
        /// Получатель сообщений от сервера
        /// </summary>
        public Receiver Receiver { get; private set; }

        /// <summary>
        /// Отправитель сообщений на сервер
        /// </summary>
        public Sender Sender { get; private set; }

        public bool IsConnected { get; private set; }

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public Client()
        {
            TcpClient = new TcpClient();
            Receiver = new Receiver(this);
            Sender = new Sender(this);
        }

        /// <summary>
        /// Подключиться
        /// </summary>
        public void Connect(NetworkMessage message)
        {
            if (!TcpClient.Connected)
            {
                TcpClient.Connect(Host, Port);
                Stream = TcpClient.GetStream();

                if (message != null)
                {
                    Sender.SendNetworkMessage(message);


                    //Sender.SendOperarationCode(Receiver.AddingNewUserCode);

                    //Sender.SendMessage(userName);
                }
                
                Receiver.ReceiveDataPackages();
            }
        }

        ///// <summary>
        ///// Подключиться
        ///// </summary>
        //public void Connect(string userName)
        //{
        //    if (!TcpClient.Connected)
        //    {
        //        TcpClient.Connect(Host, Port);
        //        Stream = TcpClient.GetStream();

        //        if (!string.IsNullOrEmpty(userName))
        //        {
        //            Sender.SendOperarationCode(Receiver.AddingNewUserCode);

        //            Sender.SendMessage(userName);
        //        }

        //        Receiver.ReceiveDataPackages();
        //    }
        //}

        //bool IsConnected()
        //{
        //    return true;
        //}
    }
}
