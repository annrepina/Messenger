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
using DtoLib.Serialization;

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

        public IConnectionController ConnectionController { get; set; }

        #endregion Свойства 

        #region Конструкторы

        /// <summary>
        /// Констурктор по умолчанию
        /// </summary>
        public ClientNetworkProvider(IConnectionController connectionController) : base()
        {
            TcpClient = new TcpClient();
            IsConnected = false;
            ConnectionController = connectionController;
        }

        #endregion Конструкторы

        #region Методы связанные с сетью

        /// <summary>
        /// Подключиться к серверу асинхронно 
        /// </summary>
        public async Task ConnectAsync(byte[] messageBytes)
        {
            try
            {
                if (!TcpClient.Connected)
                {
                    TcpClient.Connect(Host, Port);
                    NetworkStream = TcpClient.GetStream();
                    IsConnected = true;

                    if (messageBytes != null)
                    {
                        await _transmitter.SendNetworkMessageAsync(messageBytes);
                    }

                    await Task.Run(() => _transmitter.RunReceivingBytesInLoop());
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
                IsConnected = false;
            }
        }

        public override void NotifyBytesReceived(byte[] data)
        {
            ConnectionController.NotifyBytesReceived(data);
        }

        public override async Task SendBytesAsync(byte[] data)
        {
            if (!IsConnected)
                await ConnectAsync(data);

            else
                await _transmitter.SendNetworkMessageAsync(data);
        }

        #endregion Методы связанные с сетью
    }
}