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
    /// Сетевой провайдер, который подключается к серверу
    /// </summary>
    public abstract class NetworkProvider
    {
        /// <summary>
        /// Сетевой поток, по которому будет осуществляться передача данных
        /// </summary>
        public NetworkStream NetworkStream { get; set; }

        /// <summary>
        /// Обеспечивает клиентские соединения для сетевых служб tcp.
        /// </summary>
        public TcpClient TcpClient { get; set; }

        /// <summary>
        /// Идентификатор
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

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkProvider()
        {
            Id = 0;
            Receiver = new Receiver(this);
            Sender = new Sender(this);
            NetworkStream = null;
            TcpClient = null;
        }

        /// <summary>
        /// Асинхронный метод получения сетевого сообщения
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <returns></returns>
        public abstract Task GetNetworkMessageAsync(NetworkMessage message);
    }
}