using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.NetworkServices.Interfaces
{
    /// <summary>
    /// Интерфейс, который представляет собой сетевого провайдера
    /// Отвечает за подключение к сети и обмен данными в сети
    /// </summary>
    public interface INetworkProvider
    {
        /// <summary>
        /// Отвечает за пересылку массивов байт между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter { set; }

        /// <summary>
        /// Сетевой поток, по которому будет осуществляться передача данных
        /// </summary>
        public NetworkStream? NetworkStream { get; }

        /// <summary>
        /// Отправить массив байт по сетевому потоку асинхронно
        /// </summary>
        /// <param name="data">Отправляемые данные</param>
        public Task SendBytesAsync(byte[] data);

        /// <summary>
        /// Уведомить об отключении от сети
        /// </summary>
        public void NotifyDisconnected();

        /// <summary>
        /// Закрыть соединение с сетью
        /// </summary>
        public void CloseConnection();
    }
}