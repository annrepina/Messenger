using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DtoLib.NetworkServices;
using DtoLib;

namespace ConsoleMessengerServer.Net
{
    /// <summary>
    /// Клиент, который подключается к серверу
    /// </summary>
    public class BackClient : Client
    {
        //#region Константы

        ///// <summary>
        ///// Код добавления нового клиента
        ///// </summary>
        //public const byte AddingNewUserCode = 1;

        ///// <summary>
        ///// Код удаления клиента
        ///// </summary>
        //public const byte DisconnectingUserCode = 2;

        ///// <summary>
        ///// Код отравки сообщений
        ///// </summary>
        //public const byte SendingMessageCode = 3;

        //#endregion Константы



        ///// <summary>
        ///// Id
        ///// </summary>
        //public int Id { get; set; } // ЕСТЬ

        ///// <summary>
        ///// Предоставляет базовый поток данных для доступа к сети
        ///// </summary>
        //public NetworkStream NetworkStream { get; private set; } // ЕСТЬ

        ///// <summary>
        ///// TCP клиент
        ///// </summary>
        //private TcpClient _tcpClient; //ИЕСТЬ

        /// <summary>
        /// Сервер
        /// </summary>
        private Server _server; // НЕТ

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        /// <param name="server">Сервер</param>
        public BackClient(TcpClient tcpClient, Server server, INetworkMessageHandler networkMessageHandler) : base(networkMessageHandler)
        {
            //Id = Guid.NewGuid().ToString();
            //Id = 0;
            //Name = "";
            TcpClient = tcpClient;
            _server = server;
            //_server.AddClient(this);
            NetworkStream = TcpClient.GetStream();
        }

        /// <summary>
        /// Обработать данные
        /// </summary>
        public async Task ProcessDataAsync()
        {
            try
            {
                //GetUserName();

                //string message = "";

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        await Receiver.ReceiveNetworkMessageAsync();

                        ///*message = */
                        //GetMessageAsync();
                        //message = $"  : {message}";
                        //Console.WriteLine(message);

                        //_server.BroadcastOperationCode(SendingMessageCode, Id);
                        //_server.BroadcastMessage(message, Id);
                    }
                    catch (Exception)
                    {
                        //message = $"{Name}: покинул/покинула чат";
                        //Console.WriteLine(message);

                        //_server.BroadcastOperationCode(DisconnectingUserCode, Id);
                        //_server.BroadcastMessage(Name, Id);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                _server.RemoveClient(Id);
                CloseConnection();
            }
        }



        ///// <summary>
        ///// Получить имя пользователя
        ///// </summary>
        //private void GetUserName()
        //{
        //    string message = GetMessage();

        //    Name = message;

        //    //message = $"{Name} вошел/вошла в чат";

        //    // отправить всем сообщение о том, что добавляется новый человек
        //    _server.BroadcastOperationCode(AddingNewUserCode, Id);

        //    // Отправить всем имя этого человека
        //    _server.BroadcastMessage(Name, Id);

        //    Console.WriteLine($"{Name} вошел/вошла в чат");
        //    //Console.WriteLine(message);
        //}

        ///// <summary>
        ///// Получить сообщение
        ///// </summary>
        //private async Task GetMessageAsync()
        //{
        //    string message = "";

        //    // Буфер для получения даты
        //    byte[] data = new byte[256];

        //    StringBuilder stringBuilder = new StringBuilder();

        //    int bytes = 0;

        //    do
        //    {
        //        bytes = await System.Net.Sockets.NetworkStream.ReadAsync(data, 0, data.Length);

        //        stringBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));

        //    } while (System.Net.Sockets.NetworkStream.DataAvailable);

        //    message = stringBuilder.ToString();

        //    //return message;
        //}

        /// <summary>
        /// Закрытие подлючения
        /// </summary>
        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (TcpClient != null)
                TcpClient.Close();
        }
    }
}