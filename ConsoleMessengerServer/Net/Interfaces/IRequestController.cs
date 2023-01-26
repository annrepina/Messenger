using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net.Interfaces
{
    /// <summary>
    /// Интерфейс, который обрабатывает сетевые сообщения на стороне клиента
    /// </summary>
    public interface IRequestController
    {
        /// <summary>
        /// Обрабатывает массив байтов переданных по сети
        /// </summary>
        /// <param name="data">Полученный массив байтов</param>
        /// <param name="networkProviderId">Идентификатор отправителя</param>
        /// <returns></returns>
        public byte[] ProcessRequest(byte[] data, IServerNetworProvider networkProviderId);
    }
}