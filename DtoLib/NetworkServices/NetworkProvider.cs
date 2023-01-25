using DtoLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    /// <summary>
    /// Сетевой провайдер, который подключается к серверу
    /// </summary>
    public abstract class NetworkProvider : INetworkProvider
    {
        /// <summary>
        /// Счетчик, на основе которого, объекту присваивается id
        /// </summary>
        private static int _counter = 0;

        protected ITransmitterAsync _transmitter;

        //public event Action<int> Disconnected;

        #region Свойства

        /// <summary>
        /// Сетевой поток, по которому будет осуществляться передача данных
        /// </summary>
        public NetworkStream NetworkStream { get; set; }

        /// <summary>
        /// Обеспечивает клиентские соединения для сетевых служб tcp.
        /// </summary>
        public TcpClient TcpClient { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Отвечает за пересылку байтов между клиентом и сервером.
        /// </summary>
        public ITransmitterAsync Transmitter 
        { 
            set
            {
                _transmitter = value;
            }
        }

        #endregion Свойства 

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkProvider()
        {
            Id = ++_counter;
            Transmitter = new Transmitter(this);
            NetworkStream = null;
            TcpClient = null;
        }

        public event Action<int> Disconnected;

        /// <summary>
        /// Закрыть подлючения
        /// </summary>
        public void CloseConnection()
        {
            if (NetworkStream != null)
                NetworkStream.Close();

            if (TcpClient != null)
                TcpClient.Close();
        }

        /// <summary>
        /// Метод получения сетевого сообщения
        /// </summary>
        /// <param name="data">Массив получаемых байтов</param>
        /// <returns></returns>
        public abstract void NotifyBytesReceived(byte[] data);

        public abstract Task SendBytesAsync(byte[] data);

        public virtual void Disconnect()
        {
            Disconnected.Invoke(Id);
            //CloseConnection();
        }

        /// <summary>
        /// Получить сетевое сообщение асинхронно
        /// </summary>
        public async Task ReadBytes()
        {
            try
            {
                while (true)
                {
                    // буфер для получаемых данных
                    byte[] data = await _transmitter.ReceiveBytesAsync();

                    NotifyBytesReceived(data);
                }
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}