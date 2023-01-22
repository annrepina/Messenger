using DtoLib.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    /// <summary>
    /// Класс, который отвечает за пересылку байтов между клиентом и сервером.
    /// </summary>
    public class Transmitter : ITransmitterAsync
    {
        /// <summary>
        /// Сетевой провайдер, который подключается к серверу
        /// </summary>
        public INetworkProvider NetworkProvider { get; private set; }


        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public Transmitter(INetworkProvider networkProvider)
        {
            NetworkProvider = networkProvider;
        }

        /// <summary>
        /// Получить сообщение
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ReceiveBytesAsync()
        {
            byte[] lengthBuffer = new byte[4];
            List<byte> bytesList = new List<byte>();
            int bytes = 0;

            bytes = await NetworkProvider.NetworkStream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);

            if (bytes == 0)
                throw new Exception("Удаленный хост разорвал соединение.");

            int length = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] data = new byte[length];

            do
            {
                bytes = await NetworkProvider.NetworkStream.ReadAsync(data, 0, data.Length);

                for(int i = 0; i < bytes; ++i)
                {
                    bytesList.Add(data[i]);
                }

            } while (bytesList.Count < length);

            data = bytesList.ToArray();

            return data;
        }

        /// <summary>
        /// Получить сетевое сообщение асинхронно
        /// </summary>
        public async Task RunReceivingBytesInLoop()
        {
            try
            {
                while (true)
                {
                    // буфер для получаемых данных
                    byte[] data = await ReceiveBytesAsync();

                    //NetworkMessage networkMessage = SerializationHelper.Deserialize<NetworkMessage>(data);
                    //NetworkProvider.GetNetworkMessage(networkMessage);
                    NetworkProvider.NotifyBytesReceived(data);
                }
            }
            catch (Exception ex)
            {
                var s = ex.ToString();
                throw;
            }
        }

        /// <summary>
        /// Отправить сетевое сообщение серверу асинхронно
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение в виде байтов</param>
        public async Task SendNetworkMessageAsync(byte[] networkMessage)
        {
            try
            {
                Int32 bytesNumber = networkMessage.Length;

                byte[] length = BitConverter.GetBytes(bytesNumber);

                byte[] messageWithLength = new byte[networkMessage.Length + length.Length];

                length.CopyTo(messageWithLength, 0);
                networkMessage.CopyTo(messageWithLength, length.Length);

                await NetworkProvider.NetworkStream.WriteAsync(messageWithLength, 0, messageWithLength.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}