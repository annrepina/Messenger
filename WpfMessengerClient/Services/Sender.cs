using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChatClient.Services
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
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            Client.Stream.Write(data, 0, data.Length);  
        }

        /// <summary>
        /// Отправить код операции
        /// </summary>
        /// <param name="operationCode">Код операции</param>
        public void SendOperarationCode(byte operationCode)
        {
            Client.Stream.WriteByte(operationCode);
        }
    }
}
