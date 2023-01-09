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
        private AppLogic _appLogic;

        public App()
        {
            _appLogic = new AppLogic();
        }

        public void Dispose()
        {
            _appLogic.Dispose();
        }

        public async Task LaunchAsync()
        {
            //_appLogic.DeleteNetworkProvidersFromDb();

            await _appLogic.StartListeningConnectionsAsync();
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

