using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer
{
    public class App
    {
        private AppLogic _appLogic;

        public App()
        {
            _appLogic = new AppLogic();
        }

        public async Task LaunchAsync()
        {
            await _appLogic.StartListeningConnectionsAsync();
        }
    }
}

