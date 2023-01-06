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

        //public NetworkMessage NetworkMessage { get; set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="client">Клиент</param>
        public Receiver(NetworkProvider networkProvider)
        {
            NetworkProvider = networkProvider;
        }

        /// <summary>
        /// Получить сообщение
        /// </summary>
        /// <returns></returns>
        public virtual async Task<byte[]> ReceiveBytesAsync()
        {
            //// буфер для получаемых данных
            byte[] data = new byte[256];

            int bytes = 0;

            try
            {
                do
                {
                    bytes = await NetworkProvider.NetworkStream.ReadAsync(data, 0, data.Length);

                    //bytes = NetworkProvider.NetworkStream.Read(data, 0, data.Length);

                } while (NetworkProvider.NetworkStream.DataAvailable);

            }
            catch (Exception)
            {
                //MessageBox.Show("Соединение прервано");
                Disconnect();
            }

            byte[] cutData = new byte[bytes];

            var list = data.ToList();

            list.RemoveRange(bytes, 256 - bytes);

            list.CopyTo(cutData);

            return cutData;
        }

        /// <summary>
        /// Получить сетевое сообщение асинхронно
        /// </summary>
        public async Task ReceiveNetworkMessageAsync()
        {
            while (true)
            {
                try
                {
                    // буфер для получаемых данных
                    byte[] data = await ReceiveBytesAsync();

                    NetworkMessage networkMessage = Deserializer.Deserialize<NetworkMessage>(data);

                    await NetworkProvider.GetNetworkMessageAsync(networkMessage);
                }
                catch (Exception)
                {
                    throw;
                }
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

            //завершение процесса
            Environment.Exit(0);
        }
    }
}
