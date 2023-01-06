using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
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
            try
            {
                _tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();

                    await NetworkHandler.RunNewBackClientAsync(tcpClient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DisconnectClients();
            }
        }

        ///// <summary>
        ///// Трансляция сообщения подлюченным клиентам
        ///// </summary>
        ///// <param name="message">Сообщение</param>
        ///// <param name="id">Id клиента, который отправил данное сообщение</param>
        //public async Task BroadcastMessageAsync(string message, int id)
        //{
        //    byte[] data = Encoding.UTF8.GetBytes(message);

        //    foreach (var client in _ServerNetworkProviders)
        //    {
        //        // если id клиента не равно id отправляющего
        //        if (client.Id != id)
        //        {
        //            // передача данных
        //            await client.NetworkStream.WriteAsync(data, 0, data.Length);
        //        }
        //    }
        //}
        //public void BroadcastOperationCode(byte operationCode, int id)
        //{
        //    foreach (var client in _ServerNetworkProviders)
        //    {
        //        // если id клиента не равно id отправляющего
        //        if (client.Id != id)
        //        {
        //            // передача данных
        //            client.NetworkStream.WriteByte(operationCode);
        //        }
        //    }
        //}

        /// <summary>
        /// Отключение всех клиентов
        /// </summary>
        public void DisconnectClients()
        {
            NetworkHandler.DisconnectClients();

            /// Остановка сервера
            _tcpListener.Stop();

            //завершение процесса
            Environment.Exit(0);
        }
    }
}