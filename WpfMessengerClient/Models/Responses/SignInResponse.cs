using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Ответ на запрос о входе в мессенджер
    /// </summary>
    public class SignInResponse
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
        /// Статус ответа
        /// </summary>
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Контекст ошибки, если статус ответа неудачный
        /// </summary>
        public SignInFailContext Context { get; init; }


        public SignInResponse(User? user, List<Dialog>? dialogs, NetworkResponseStatus status, SignInFailContext context)
        {
            User = user;
            Dialogs = dialogs;
            Status = status;
            Context = context;
        }
    }
}
