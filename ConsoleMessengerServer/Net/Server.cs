﻿using ConsoleMessengerServer.Net.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Сервер, который принимает подлючения клиентов
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Отвечает за соединение по сети с клиентами
        /// </summary>
        private IConnectionController _connectionController;

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
        public Server(IConnectionController connectionController)
        {
            _port = 8888;

            _tcpListener = new TcpListener(IPAddress.Any, _port);

            _connectionController = connectionController;
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

                _connectionController.InitializeNewConnection(tcpClient);
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