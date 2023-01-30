namespace Common.NetworkServices.Interfaces
{
    /// <summary>
    /// Интерфейс - трансмиттер который отвечает за пересылку байтов по сети асинхронно
    /// </summary>
    public interface ITransmitterAsync
    {
        /// <summary>
        /// Отправить сетевое сообщение серверу асинхронно
        /// </summary>
        /// <param name="networkMessageBytes">Сетевое сообщение в виде байтов</param>
        public Task SendNetworkMessageAsync(byte[] networkMessageBytes);

        /// <summary>
        /// Получить массив байт из потока асинхронно
        /// </summary>
        /// <returns>Массив полученных байт</returns>
        public Task<byte[]> ReceiveBytesAsync();
    }
}