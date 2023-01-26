using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices.Interfaces
{
    /// <summary>
    /// Интерфейс, который представляет собой сетевого провайдера
    /// </summary>
    public interface INetworkProvider
    {
        //public event Action<byte[], INetworkProvider> BytesReceived;

        /// <summary>
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter { set; }

        /// <summary>
        /// Сетевой поток, по которому будет осуществляться передача данных
        /// </summary>
        public NetworkStream? NetworkStream { get; set; }

        /// <summary>
        /// Обеспечивает клиентские соединения для сетевых служб tcp.
        /// </summary>
        public TcpClient? TcpClient { get; set; }

        /// <summary>
        /// Уведомить о получении массива байтов
        /// </summary>
        /// <param name="data">Массив байтов</param>
        public void NotifyBytesReceived(byte[] data);

        /// <summary>
        /// Отправить массив байтов
        /// </summary>
        /// <param name="data"></param>
        public Task SendBytesAsync(byte[] data);

        public Task ReadBytesAsync();

        public void Disconnect();

        public void CloseConnection();
    }
}