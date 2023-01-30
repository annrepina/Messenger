namespace ConsoleMessengerServer.Net.Interfaces
{
    /// <summary>
    /// Интерфейс, который управляет обработкой запросов отпрвленных клиентами
    /// </summary>
    public interface IRequestController
    {
        /// <summary>
        /// Обрабатывает массив байтов переданных по сети
        /// </summary>
        /// <param name="data">Полученный массив байтов</param>
        /// <param name="networkProviderId">Идентификатор сетевого провайдера</param>
        /// <returns></returns>
        public byte[] ProcessRequest(byte[] data, IServerNetworProvider networkProviderId);
    }
}