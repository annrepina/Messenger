//#define DEBUG
using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //using(App app = new App())
            App app = new App();

            //Task.Run(() => app.ReadStopKey());

            await app.LaunchAsync();            
        }
    }
}