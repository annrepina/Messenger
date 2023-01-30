using Common.NetworkServices;
using Common.NetworkServices.Interfaces;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Класс представлят ответ на запрос
    /// </summary>
    public class Response : IResponse
    {
        /// <summary>
        /// Статус ответа
        /// </summary>
        public NetworkResponseStatus Status { get; set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="status">Статус ответа</param>
        public Response(NetworkResponseStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Response()
        {
        }
    }
}