using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer
{
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
            _appLogic.DeleteNetworkProvidersFromDb();

            await _appLogic.StartListeningConnectionsAsync();
        }
    }
}

