using Common.NetworkServices;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Класс представляет ответ на запрос о регистрации пользователя в мессенджере
    /// </summary>
    public class SignUpResponse : Response
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="status">Статус ответа</param>
        public SignUpResponse(int userId, NetworkResponseStatus status) : base(status)
        {
            UserId = userId;
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="status">Статус ответа</param>
        public SignUpResponse(NetworkResponseStatus status) : base(status)
        {
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SignUpResponse()
        {

        }
    }
}