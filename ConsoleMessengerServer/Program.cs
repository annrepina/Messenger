//#define DEBUG

namespace ConsoleMessengerServer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            App app = new App();

            await app.LaunchAsync();
        }
    }
}