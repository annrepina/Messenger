using DtoLib.NetworkServices;
using System.Collections.Generic;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Ответ на запрос о входе в мессенджер
    /// </summary>
    public class SignInResponse : Response
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public User? User { get; init; }

        /// <summary>
        /// Список диалогов пользователя
        /// </summary>
        public List<Dialog>? Dialogs { get; init; }

        /// <summary>
        /// Контекст ошибки, если статус ответа неудачный
        /// </summary>
        public SignInFailContext Context { get; init; }

        /// <summary>
        /// Конструктор с парамаетрами
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="dialogs">Диалоги пользователя</param>
        /// <param name="status">Статус ответа</param>
        /// <param name="context">Констекст ошибки, в случае неудачного входа</param>
        public SignInResponse(User? user, List<Dialog>? dialogs, NetworkResponseStatus status, SignInFailContext context) : base(status)
        {
            User = user;
            Dialogs = dialogs;
            Context = context;
        }
    }
}