using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using DtoLib;
using DtoLib.Dto;
using DtoLib.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Сервер, который принимает подлючения клиентов
    /// </summary>
    public class Server
    {
        private INetworkController NetworkHandler;

        /// <summary>
        /// Прослушиватель TCP подключений от клиентов
        /// </summary>
        private TcpListener _tcpListener;

        /// <summary>
        /// Порт
        /// </summary>
        private int _port;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Server(INetworkController iNetworkHandler)
        {
            _port = 8888;

            _tcpListener = new TcpListener(IPAddress.Any, _port);

            NetworkHandler = iNetworkHandler;
        }

        /// <summary>
        /// Прослушивание входящих подключений
        /// </summary>
        public async Task ListenIncomingConnectionsAsync()
        {
            _tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();

                NetworkHandler.InitializeNewConnection(tcpClient);
            }
        }

        /// <summary>
        /// Остановка сервера
        /// </summary>
        public void Stop()
        {
            _tcpListener.Stop();
        }
    }
}