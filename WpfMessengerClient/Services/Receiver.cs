using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMessengerClient.Services
{
    /// <summary>
    /// Получает сообщения от сервера
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// Новый пользователь добавлен
        /// </summary>
        public event Action NewUserAddedEvent;

        /// <summary>
        /// Пользователь отключен
        /// </summary>
        public event Action UserDisconnectedEvent;

        /// <summary>
        /// Сообщение отправлено
        /// </summary>
        public event Action MessageSendedEvent;

        #region Константы

        /// <summary>
        /// Код добавления нового клиента
        /// </summary>
        public const byte AddingNewUserCode = 1;

        /// <summary>
        /// Код удаления клиента
        /// </summary>
        public const byte DisconnectingUserCode = 2;

        /// <summary>
        /// Код отравки сообщений
        /// </summary>
        public const byte SendingMessageCode = 3;

        #endregion Константы

        /// <summary>
        /// Клиент, который подключается к серверу
        /// </summary>
        public Client Client { get; private set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="client">Клиент</param>
        public Receiver(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Получить код операции
        /// </summary>
        /// <returns></returns>
        public byte ReceiveOperationCode()
        {
            byte operationCode;

            do
            {
                try
                {
                    // буфер для получаемых данных
                    operationCode = (byte)Client.Stream.ReadByte();
                    break;
                }
                catch
                {
                    MessageBox.Show("Подкючение прервано!");
                    Disconnect();
                }

            } while (true);

            return operationCode;
        }

        /// <summary>
        /// Получить сообщение
        /// </summary>
        /// <returns></returns>
        public string ReceiveMessage()
        {
            string message = "";

            //while (true)
            //{
            try
            {
                // буфер для получаемых данных
                byte[] data = new byte[256];

                StringBuilder stringBuilder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = Client.Stream.Read(data, 0, data.Length);
                    stringBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));

                } while (Client.Stream.DataAvailable);

                message = stringBuilder.ToString();
            } 
            catch (Exception)
            {
                MessageBox.Show("Соединение прервано");
                Disconnect();
            }
            //}

            return message;
        }

        /// <summary>
        /// Получать пакеты данных
        /// </summary>
        public void ReceiveDataPackages()
        {
            Task.Run(() =>
                {
                    while (true)
                    {
                        var operationCode = ReceiveOperationCode();

                        LaunchOperation(operationCode);
                    }
                }
            );
        }

        /// <summary>
        /// Запустить операцию
        /// </summary>
        /// <param name="operationCode">Код операции</param>
        public void LaunchOperation(byte operationCode)
        {
            switch (operationCode)
            {
                case AddingNewUserCode:
                    NewUserAddedEvent?.Invoke();

                    break;

                case DisconnectingUserCode:
                    UserDisconnectedEvent?.Invoke();

                    break;

                case SendingMessageCode:
                    MessageSendedEvent?.Invoke();

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Прервать подключение
        /// </summary>
        public void Disconnect()
        {
            // Отключение потока
            if (Client.Stream != null)
                Client.Stream.Close();

            // Отключение клиента
            if (Client.TcpClient != null)
                Client.TcpClient.Close();

            //завершение процесса
            Environment.Exit(0);
        }
    }
}
