using ConsoleMessengerServer.Entities;
using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        ///// <summary>
        ///// Статус ответа
        ///// </summary>
        //public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Контекст ошибки, если статус ответа неудачный
        /// </summary>
        public SignInFailContext Context { get; init; }

        public SignInResponse(NetworkResponseStatus status) : base(status)
        {
            User = null;
            Dialogs = null;
        }

        public SignInResponse(NetworkResponseStatus status, SignInFailContext сontext) : base(status)
        {
            //Status = status;
            Context = сontext;
            Dialogs = new List<Dialog>();
        }

        public SignInResponse(User user, List<Dialog> dialogs, NetworkResponseStatus status) : base(status)
        {
            User = user;
            Dialogs = dialogs;
            //Status = status;
        }
    }
}