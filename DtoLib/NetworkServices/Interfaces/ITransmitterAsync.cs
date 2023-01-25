using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices.Interfaces
{
    /// <summary>
    /// Интерфейс который отвечает за пересылку байтов по сети асинхронно
    /// </summary>
    public interface ITransmitterAsync
    {
        /// <summary>
        /// Отправить сетевое сообщение серверу асинхронно
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение в виде байтов</param>
        public Task SendNetworkMessageAsync(byte[] networkMessage);

        public Task<byte[]> ReceiveBytesAsync();
    }
}
