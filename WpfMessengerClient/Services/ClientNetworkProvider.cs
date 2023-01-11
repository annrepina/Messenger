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
    /// Сетевой провайдер на стороне клиента
    /// </summary>
    public class ClientNetworkProvider : NetworkProvider
    {
        #region Константы

        /// <summary>
        /// Ip хоста
        /// </summary>
        private const string Host = "127.0.0.1";

        /// <summary>
        /// Порт по которому будет осуществляться передача данных
        /// </summary>
        private const int Port = 8888;

        #endregion Константы

        #region Свойства

        /// <summary>
        /// Свойство - провайдер подключен к серверу?
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Свойство - обработчик сетевых сообщений
        /// </summary>
        public INetworkMessageHandler NetworkMessageHandler { get; set; }

        #endregion Свойства 

        #region Конструкторы

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public ClientNetworkProvider(INetworkMessageHandler networkMessageHandler) : base()
        {
            TcpClient = new TcpClient();
            NetworkMessageHandler = networkMessageHandler;
        }

        #endregion Конструкторы

        #region Методы связанные с сетью

        /// <summary>
        /// Подключиться к серверу асинхронно 
        /// </summary>
        public async Task ConnectAsync(NetworkMessage message)
        {
            try
            {
                if (!TcpClient.Connected)
                {
                    TcpClient.Connect(Host, Port);
                    NetworkStream = TcpClient.GetStream();

                    if (message != null)
                    {
                        await Sender.SendNetworkMessageAsync(message);
                    }

                    await Task.Run(() => Receiver.ReceiveNetworkMessageAsync());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Подключение прервано...");
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Асинхронный метод получения сетевого сообщения
        /// </summary>
        /// <param _name="message">Сетевое сообщение</param>
        /// <returns></returns>
        public override void GetNetworkMessage(NetworkMessage message)
        {
            NetworkMessageHandler.ProcessNetworkMessage(message);
        }

        #endregion Методы связанные с сетью
    }
}