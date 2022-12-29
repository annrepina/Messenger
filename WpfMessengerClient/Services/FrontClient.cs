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
using WpfMessengerClient.Models;
using DtoLib.NetworkInterfaces;

namespace WpfMessengerClient.Services
{
    /// <summary>
    /// Клиент, который подключается к серверу
    /// </summary>
    public class FrontClient : Client
    {
        /// <summary>
        /// Ip хоста
        /// </summary>
        private const string Host = "127.0.0.1";

        /// <summary>
        /// Порт по которому будет осуществляться передача данных
        /// </summary>
        private const int Port = 8888;

        public bool IsConnected { get; private set; }

        public INetworkMessageHandler NetworkMessageHandler { get; set; }

        public FrontClient() : base()
        {
            TcpClient = new TcpClient();
        }

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public FrontClient(INetworkMessageHandler networkMessageHandler)
        {
            TcpClient = new TcpClient();
            NetworkMessageHandler = networkMessageHandler;
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
                }

                await Receiver.ReceiveNetworkMessageAsync();
            }
        }

        public override async Task GetNetworkMessageAsync(NetworkMessage message)
        {
            await Task.Run(() => NetworkMessageHandler.ProcessNetworkMessage(message));
        }
    }

}
