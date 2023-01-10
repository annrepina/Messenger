using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkInterfaces
{
    /// <summary>
    /// Интерфейс - обработчик сетевого сообщения
    /// </summary>
    public interface INetworkMessageHandler
    {
        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="message"></param>
        public void ProcessNetworkMessage(NetworkMessage message);
    }
}