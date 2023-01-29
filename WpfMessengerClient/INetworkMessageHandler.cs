namespace WpfMessengerClient
{
    /// <summary>
    /// Интерфейс - обработчик сетевого сообщения, который представлен массивом байт
    /// </summary>
    public interface INetworkMessageHandler
    {
        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="networkMessageBytes">Массив байт, представляющий сетевое сообщение</param>
        public void ProcessNetworkMessage(byte[] networkMessageBytes);
    }
}