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
        private NetworkMessageHandler _networkMessageHandler;

        /// <summary>
        /// Объект, управляющий работой со связью по сети
        /// </summary>
        private ConnectionController _connectionController;

        public App()
        {
            _networkMessageHandler = new NetworkMessageHandler();
            _connectionController = new ConnectionController();
            _connectionController.ServerNetworkMessageHandler = _networkMessageHandler;
            _networkMessageHandler.ConnectionController = _connectionController;
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