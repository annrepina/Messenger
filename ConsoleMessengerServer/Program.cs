using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer
{
    public class Program
    {
        //static Server server; // сервер
        //static Thread listenThread; // потока для прослушивания
        static void Main(string[] args)
        {
            App app = new App();

            Task.Run(() => app.LaunchAsync());

            Console.ReadKey();
        }
    }
}