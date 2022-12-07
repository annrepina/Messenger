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
        public Client _client;

        /// <summary>
        /// Получатель сообщений от сервера
        /// </summary>
        public Receiver _receiver;

        /// <summary>
        /// Отправитель сообщений на сервер
        /// </summary>
        public Sender _sender;

        public ConnectionService()
        {
            _client = new Client();
            _receiver = new Receiver(_client);
            _sender = new Sender(_client);
        }
    }
}
