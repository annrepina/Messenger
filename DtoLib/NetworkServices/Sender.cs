using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib.Dto;
using DtoLib.Serialization;

namespace DtoLib.NetworkServices
{
    //public class Sender
    //{
    //    /// <summary>
    //    /// Клиент, который подключается к серверу
    //    /// </summary>
    //    public NetworkProvider NetworkProvider { get; private set; }

    //    /// <summary>
    //    /// Конструктор с параметром
    //    /// </summary>
    //    /// <param name="client">Клиент</param>
    //    public Sender(NetworkProvider networkProvider)
    //    {
    //        NetworkProvider = networkProvider;
    //    }

    //    /// <summary>
    //    /// Отправить сетевое сообщение серверу асинхронно
    //    /// </summary>
    //    /// <param name="message">Сетевое сообщение</param>
    //    public async Task SendNetworkMessageAsync(NetworkMessage message)
    //    {
    //        try
    //        {
    //            byte[] data = SerializationHelper.Serialize(message);

    //            Int32 bytesNumber = data.Length;
    //            //Int32 bytesNumber = 2147483647;

    //            //byte[] length = BitConverter.GetBytes(bytesNumber);

    //            byte[] length = SerializationHelper.Serialize((Int32)bytesNumber);



    //            byte[] messageWithLength = new byte[data.Length + length.Length];

    //            length.CopyTo(messageWithLength, 0);
    //            data.CopyTo(messageWithLength, length.Length);

    //            await NetworkProvider.NetworkStream.WriteAsync(messageWithLength, 0, messageWithLength.Length);
    //        }
    //        catch (Exception ex )
    //        {
    //            Console.WriteLine(ex.Message);
    //            throw;
    //        }
    //    }
    //}
}
