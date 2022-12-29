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
        public INetworkHandler INetworkHandler { get; init; }

        /// <summary>
        /// Прослушиватель TCP подключений от клиентов
        /// </summary>
        private TcpListener _tcpListener;

        /// <summary>
        /// Порт
        /// </summary>
        private int _port;

        ///// <summary>
        ///// Словарь, который содержит пары ключ - id Клиента и сам клиент
        ///// </summary>
        //private Dictionary<int, BackClient> _clients;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Server(INetworkHandler iNetworkHandler)
        {
            _port = 8888;

            _tcpListener = new TcpListener(IPAddress.Any, _port);

            INetworkHandler = iNetworkHandler;

            //_clients = new Dictionary<int, BackClient>();
        }

        ///// <summary>
        ///// Добавить клиента
        ///// </summary>
        ///// <param name="client">Клиент</param>
        //public void AddClient(BackClient client)
        //{
        //    _clients.Add(client);
        //}

        ///// <summary>
        ///// Удалить клиента
        ///// </summary>
        ///// <param name="clientId">Id клиента</param>
        //public void RemoveClient(int clientId)
        //{

        //    if (_clients != null && _clients.Count > 0)
        //    {
        //        // получаем по id подключение
        //        //BackClient? client = _clients.FirstOrDefault(c => c.Id == clientId);

        //        //if (client != null)
        //        //{
        //        //    _clients.Remove(client);
        //        //    client.CloseConnection();
        //        //}

        //        _clients.Remove(clientId);
        //    }
        //}

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

                    await INetworkHandler.RunNewBackClientAsync(tcpClient);
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

        //public async Task SendMessageToViewModel(NetworkMessage message, int clientId)
        //{
        //    await _clients[clientId].Sender.SendNetworkMessageAsync(message);

        //    //switch(message.CurrentCode)
        //    //{
        //    //    case NetworkMessage.OperationCode.RegistrationCode:
        //    //        {
        //    //            UserAccountDto userAccountDto = new Deserializer<UserAccountDto>().Deserialize(message.Data);


        //    //            userAccountDto.Person.Name = "КУКУ епта";
        //    //            //await userAccountDto.CurrentClient.Sender.SendNetworkMessageAsync(message);

        //    //            await _clients[0].Sender.SendNetworkMessageAsync(message);



        //    //        }
        //    //        break;


        //    //}
        //}

        ///// <summary>
        ///// Трансляция сообщения подлюченным клиентам
        ///// </summary>
        ///// <param name="message">Сообщение</param>
        ///// <param name="id">Id клиента, который отправил данное сообщение</param>
        //public async Task BroadcastMessageAsync(string message, int id)
        //{
        //    byte[] data = Encoding.UTF8.GetBytes(message);

        //    foreach (var client in _clients)
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
        //    foreach (var client in _clients)
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
            INetworkHandler.DisconnectClients();

            /// Остановка сервера
            _tcpListener.Stop();

            //завершение процесса
            Environment.Exit(0);
        }
    }
}
