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
    public class Transmitter
    {
        /// <summary>
        /// Сетевой провайдер, который подключается к серверу
        /// </summary>
        public NetworkProvider NetworkProvider { get; private set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public Transmitter(NetworkProvider networkProvider)
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
            //int length = SerializationHelper.Deserialize<int>(lengthBuffer);

            byte[] data = new byte[length];

            do
            {
                bytes = await NetworkProvider.NetworkStream.ReadAsync(data, 0, data.Length);

                for(int i = 0; i < bytes; ++i)
                {
                    bytesList.Add(data[i]);
                }

            } while (bytesList.Count < length /*&& NetworkProvider.NetworkStream.DataAvailable*/);

            data = bytesList.ToArray();

            return data;





            ////// буфер для получаемых данных
            //byte[] data = new byte[2048];

            //int bytes = 0;

            //do
            //{
            //    bytes = await NetworkProvider.NetworkStream.ReadAsync(data, 0, data.Length);

            //} while (NetworkProvider.NetworkStream.DataAvailable);

            //byte[] cutData = new byte[bytes];

            //var list = data.ToList();

            //list.RemoveRange(bytes, 2048 - bytes);

            //list.CopyTo(cutData);

            //return cutData;
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

                    NetworkMessage networkMessage = SerializationHelper.Deserialize<NetworkMessage>(data);

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
        /// Отправить сетевое сообщение серверу асинхронно
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение в виде байтов</param>
        public async Task SendNetworkMessageAsync(byte[] networkMessage)
        {
            try
            {
                //byte[] data = SerializationHelper.Serialize(message);

                Int32 bytesNumber = networkMessage.Length;
                //Int32 bytesNumber = 2147483647;

                byte[] length = BitConverter.GetBytes(bytesNumber);

                //byte[] length = SerializationHelper.Serialize((Int32)bytesNumber);



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

        //public async Task SendNetworkMessageAsync(NetworkMessage message)
        //{
        //    try
        //    {
        //        byte[] data = SerializationHelper.Serialize(message);

        //        Int32 bytesNumber = data.Length;
        //        //Int32 bytesNumber = 2147483647;

        //        //byte[] length = BitConverter.GetBytes(bytesNumber);

        //        byte[] length = SerializationHelper.Serialize((Int32)bytesNumber);



        //        byte[] messageWithLength = new byte[data.Length + length.Length];

        //        length.CopyTo(messageWithLength, 0);
        //        data.CopyTo(messageWithLength, length.Length);

        //        await NetworkProvider.NetworkStream.WriteAsync(messageWithLength, 0, messageWithLength.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}


    }
}