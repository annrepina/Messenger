using ConsoleMessengerServer.Net;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Приложение - серверная часть мессенжера
    /// </summary>
    public class App
    {
        /// <summary>
        /// Отвечает за соединение по сети с клиентами
        /// </summary>
        private ConnectionController _connectionController;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public App()
        {
            _connectionController = new ConnectionController();
        }

        /// <summary>
        /// Запустить приложение асинхронно
        /// </summary>
        public async Task LaunchAsync()
        {
            await _connectionController.RunAsync();
        }
    }
}