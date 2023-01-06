using DtoLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

//namespace WpfMessengerClient.Services
//{
//    /// <summary>
//    /// Получает сообщения от сервера
//    /// </summary>
//    public class Receiver
//    {
//        ///// <summary>
//        ///// Новый пользователь добавлен
//        ///// </summary>
//        //public event Action NewUserAddedEvent;

//        ///// <summary>
//        ///// Пользователь отключен
//        ///// </summary>
//        //public event Action UserDisconnectedEvent;

//        ///// <summary>
//        ///// Сообщение отправлено
//        ///// </summary>
//        //public event Action MessageSendedEvent;

//        //#region Константы

//        ///// <summary>
//        ///// Код добавления нового клиента
//        ///// </summary>
//        //public const byte AddingNewUserCode = 1;

//        ///// <summary>
//        ///// Код удаления клиента
//        ///// </summary>
//        //public const byte DisconnectingUserCode = 2;

//        ///// <summary>
//        ///// Код отравки сообщений
//        ///// </summary>
//        //public const byte SendingMessageCode = 3;

//        //#endregion Константы

//        /// <summary>
//        /// Клиент, который подключается к серверу
//        /// </summary>
//        public FrontClient NetworkProvider { get; private set; }

//        //public NetworkMessage NetworkMessage { get; set; }

//        /// <summary>
//        /// Конструктор с параметром
//        /// </summary>
//        /// <param name="client">Клиент</param>
//        public Receiver(FrontClient client)
//        {
//            NetworkProvider = client;
//        }

//        ///// <summary>
//        ///// Получить код операции
//        ///// </summary>
//        ///// <returns></returns>
//        //public byte ReceiveOperationCode()
//        //{
//        //    byte operationCode;

//        //    do
//        //    {
//        //        try
//        //        {
//        //            // буфер для получаемых данных
//        //            operationCode = (byte)NetworkProvider.Stream.ReadByte();
//        //            break;
//        //        }
//        //        catch
//        //        {
//        //            MessageBox.Show("Подкючение прервано!");
//        //            Disconnect();
//        //        }

//        //    } while (true);

//        //    return operationCode;
//        //}

//        /// <summary>
//        /// Получить сообщение
//        /// </summary>
//        /// <returns></returns>
//        public async Task<byte[]> ReceiveBytesAsync()
//        {
//            //// буфер для получаемых данных
//            byte[] data = new byte[256];

//            try
//            {
//                int bytes = 0;

//                do
//                {
//                    bytes = await FrontClient.Stream.ReadAsync(data, 0, data.Length); 

//                } while (FrontClient.Stream.DataAvailable);

//            } 
//            catch (Exception)
//            {
//                MessageBox.Show("Соединение прервано");
//                Disconnect();
//            }

//            return data;
//        }


//        ///// <summary>
//        ///// Получить сообщение
//        ///// </summary>
//        ///// <returns></returns>
//        //public string ReceiveMessage()
//        //{
//        //    string message = "";

//        //    //while (true)
//        //    //{
//        //    try
//        //    {
//        //        // буфер для получаемых данных
//        //        byte[] data = new byte[256];

//        //        StringBuilder stringBuilder = new StringBuilder();
//        //        int bytes = 0;

//        //        do
//        //        {
//        //            bytes = NetworkProvider.Stream.Read(data, 0, data.Length);
//        //            stringBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));

//        //        } while (NetworkProvider.Stream.DataAvailable);

//        //        message = stringBuilder.ToString();
//        //    }
//        //    catch (Exception)
//        //    {
//        //        MessageBox.Show("Соединение прервано");
//        //        Disconnect();
//        //    }
//        //    //}

//        //    return message;
//        //}



//        /// <summary>
//        /// Получить сетевое сообщение асинхронно
//        /// </summary>
//        public async Task ReceiveNetworkMessageAsync()
//        {
//            while (true)
//            {
//                try
//                {
//                    // буфер для получаемых данных
//                    byte[] data = await ReceiveBytesAsync();

//                    Deserializer<NetworkMessage> deserializer = new Deserializer<NetworkMessage>();

//                    NetworkMessage networkMessage = deserializer.Deserialize(data); 

//                    await NetworkProvider.GetNetworkMessageAsync(networkMessage);
//                }
//                catch (Exception)
//                {
//                    break;
//                }
//            }
//        }

//        ///// <summary>
//        ///// Получать пакеты данных
//        ///// </summary>
//        //public void ReceiveDataPackages()
//        //{
//        //    Task.Run(() =>
//        //    {
//        //        while (true)
//        //        {
//        //            var operationCode = ReceiveOperationCode();

//        //            LaunchOperation(operationCode);
//        //        }
//        //    }
//        //    );
//        //}

//        ///// <summary>
//        ///// Запустить операцию
//        ///// </summary>
//        ///// <param name="operationCode">Код операции</param>
//        //public void LaunchOperation(byte operationCode)
//        //{
//        //    switch (operationCode)
//        //    {
//        //        case AddingNewUserCode:
//        //            NewUserAddedEvent?.Invoke();

//        //            break;

//        //        case DisconnectingUserCode:
//        //            UserDisconnectedEvent?.Invoke();

//        //            break;

//        //        case SendingMessageCode:
//        //            MessageSendedEvent?.Invoke();

//        //            break;

//        //        default:
//        //            break;
//        //    }
//        //}

//        /// <summary>
//        /// Прервать подключение
//        /// </summary>
//        public void Disconnect()
//        {
//            // Отключение потока
//            if (FrontClient.Stream != null)
//                FrontClient.Stream.Close();

//            // Отключение клиента
//            if (FrontClient.TcpClient != null)
//                FrontClient.TcpClient.Close();

//            //завершение процесса
//            Environment.Exit(0);
//        }
//    }
//}
