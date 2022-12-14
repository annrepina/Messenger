using DtoLib.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    /// <summary>
    /// Получает сообщения от сервера
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// Клиент, который подключается к серверу
        /// </summary>
        public NetworkProvider NetworkProvider { get; private set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public Receiver(NetworkProvider networkProvider)
        {
            NetworkProvider = networkProvider;
        }

        /// <summary>
        /// Получить сообщение
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ReceiveBytesAsync()
        {
            //// буфер для получаемых данных
            byte[] data = new byte[1024];

            int bytes = 0;

            do
            {
                bytes = await NetworkProvider.NetworkStream.ReadAsync(data, 0, data.Length);

            } while (NetworkProvider.NetworkStream.DataAvailable);

            byte[] cutData = new byte[bytes];

            var list = data.ToList();

            list.RemoveRange(bytes, 1024 - bytes);

            list.CopyTo(cutData);

            return cutData;
        }

        /// <summary>
        /// Получить сетевое сообщение асинхронно
        /// </summary>
        public async Task ReceiveNetworkMessageAsync()
        {
            try
            {
                while (true)
                {
                    // буфер для получаемых данных
                    byte[] data = await ReceiveBytesAsync();

                    NetworkMessage networkMessage = Deserializer.Deserialize<NetworkMessage>(data);

                    NetworkProvider.GetNetworkMessage(networkMessage);
                }
            }
            catch (Exception ex)
            {
                var s = ex.ToString();
                throw;
            }

        }

        /// <summary>
        /// Прервать подключение
        /// </summary>
        public void Disconnect()
        {
            // Отключение потока
            if (NetworkProvider.NetworkStream != null)
                NetworkProvider.NetworkStream.Close();

            // Отключение клиента
            if (NetworkProvider.TcpClient != null)
                NetworkProvider.TcpClient.Close();
        }
    }
}
