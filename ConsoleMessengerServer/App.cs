using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Приложение - серверная часть мессенжера
    /// </summary>
    public class App : IDisposable
    {
        //private RequestController _networkMessageHandler;

        /// <summary>
        /// Объект, управляющий работой со связью по сети
        /// </summary>
        private ConnectionController _connectionController;

        public App()
        {
            //_networkMessageHandler = new RequestController();
            _connectionController = new ConnectionController();
            //_connectionController.RequestController = _networkMessageHandler;
            //_networkMessageHandler.ConnectionController = _connectionController;
        }

        public void Dispose()
        {
            //_networkMessageHandler.Dispose();
        }

        public async Task LaunchAsync()
        {
            await _connectionController.RunAsync();
        }

        /// <summary>
        /// Считывать клавишу ESC, которая может закрыть сервер
        /// </summary>
        public void ReadStopKey()
        {
            Console.WriteLine();

            var keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Escape)
                Dispose();
        }
    }
}