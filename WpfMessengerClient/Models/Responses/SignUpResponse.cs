using Common.NetworkServices;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Класс, который представляет ответ на запрос о регистрации в мессенджере
    /// </summary>
    public class SignUpResponse : Response
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; init; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="status">Статус ответа</param>
        public SignUpResponse(int userId, NetworkResponseStatus status) : base(status)
        {
            UserId = userId;
            Status = status;
        }
    }
}