using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Services
{
    public class ConnectionService
    {
        /// <summary>
        /// Клиент приложения
        /// </summary>
        public Client Client { get; init; }

        /// <summary>
        /// Получатель сообщений от сервера
        /// </summary>
        public Receiver Receiver { get; init; }

        /// <summary>
        /// Отправитель сообщений на сервер
        /// </summary>
        public Sender Sender { get; init; }

        public ConnectionService()
        {
            Client = new Client();
            Receiver = new Receiver(Client);
            Sender = new Sender(Client);
        }
    }
}
