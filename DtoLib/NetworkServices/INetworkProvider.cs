using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    /// <summary>
    /// Интерфейс, который представляет собой сетевого провайдера
    /// </summary>
    public interface INetworkProvider
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter { set; }

        /// <summary>
        /// Сетевой поток, по которому будет осуществляться передача данных
        /// </summary>
        public NetworkStream NetworkStream { get; set; }

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
    }
}
