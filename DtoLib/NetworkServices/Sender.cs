using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib.Dto;
using DtoLib.Serialization;

namespace DtoLib.NetworkServices
{
    public class Sender
    {
        /// <summary>
        /// Клиент, который подключается к серверу
        /// </summary>
        public Client Client { get; private set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="client">Клиент</param>
        public Sender(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Отправить сетевое сообщение серверу
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        public async Task SendNetworkMessageAsync(NetworkMessage message)
        {
            byte[] data = new Serializator<NetworkMessage>().Serialize(message);

            await Client.NetworkStream.WriteAsync(data, 0, data.Length);
        }
    }
}
