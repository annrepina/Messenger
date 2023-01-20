using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter { get; set; }

        /// <summary>
        /// Уведомить о получении массива байтов
        /// </summary>
        /// <param name="data">Массив байтов</param>
        public void NotifyBytesReceived(byte[] data);
    }
}
