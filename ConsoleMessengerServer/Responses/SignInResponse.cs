using Common.NetworkServices;
using ConsoleMessengerServer.Entities;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Ответ на запрос о входе в мессенджер
    /// </summary>
    public class SignInResponse : Response
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Список диалогов пользователя
        /// </summary>
        public List<Dialog>? Dialogs { get; set; }

        /// <summary>
        /// Контекст ошибки, если статус ответа неудачный
        /// </summary>
        public SignInFailContext Context { get; init; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="status">Статус ответа</param>
        public SignInResponse(NetworkResponseStatus status) : base(status)
        {
            User = null;
            Dialogs = null;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="status">Статус ответа</param>
        /// <param name="сontext">Контекст ошибки, если статус ответа неудачный</param>
        public SignInResponse(NetworkResponseStatus status, SignInFailContext сontext) : base(status)
        {
            Context = сontext;
            Dialogs = new List<Dialog>();
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="dialogs">Список диалогов пользователя</param>
        /// <param name="status">Статус ответа</param>
        public SignInResponse(User user, List<Dialog> dialogs, NetworkResponseStatus status) : base(status)
        {
            User = user;
            Dialogs = dialogs;
        }
    }
}