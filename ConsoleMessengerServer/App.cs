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
        private Server _server; // сервер

        private AppLogic _appLogic;

        public App()
        {
            _appLogic = new AppLogic();
            _server = new Server(_appLogic);
            _appLogic.Server = _server;
        }

        public async Task LaunchAsync()
        {
            try
            {
                await Task.Run(() => _server.ListenIncomingConnectionsAsync());
            }
            catch (Exception ex)
            {
                _server.DisconnectClients();
                Console.WriteLine(ex.Message);
            }
        }
    }
}

