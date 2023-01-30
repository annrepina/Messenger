namespace CommonLib.NetworkServices.Interfaces
{
    /// <summary>
    /// Интерфейс, который представляет из себя ответ на запрос
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Статус ответа
        /// </summary>
        public NetworkResponseStatus Status { get; set; }
    }
}