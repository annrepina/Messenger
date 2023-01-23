using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient
{
    /// <summary>
    /// Интерфейс - обработчик сетевого сообщения
    /// </summary>
    public interface INetworkMessageHandler
    {
        IConnectionController ConnectionController { get; set; }

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="data"></param>
        public void ProcessData(byte[] data);
    }
}