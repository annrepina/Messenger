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
        /// Счетчик, на основе которого, объекту присваивается id
        /// </summary>
        private static int _counter = 0;

        #region Свойства

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
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public Transmitter Transmitter { get; private set; }

        ///// <summary>
        ///// Отправитель сообщений на сервер
        ///// </summary>
        //public Sender Sender { get; private set; }

        #endregion Свойства 

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkProvider()
        {
            Id = ++_counter;
            Transmitter = new Transmitter(this);
            NetworkStream = null;
            TcpClient = null;
        }

        /// <summary>
        /// Закрыть подлючения
        /// </summary>
        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (TcpClient != null)
                TcpClient.Close();
        }

        /// <summary>
        /// Метод получения сетевого сообщения
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <returns></returns>
        public abstract void GetNetworkMessage(NetworkMessage message);
    }
}