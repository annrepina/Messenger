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
    public class SignInResponse
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Список диалогов пользователя
        /// </summary>
        public List<Dialog> Dialogs { get; set; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Контекст ошибки, если статус ответа неудачный
        /// </summary>
        public SignInFailContext Context { get; init; }

        public SignInResponse(NetworkResponseStatus status, SignInFailContext сontext)
        {
            Status = status;
            Context = сontext;
            Dialogs = new List<Dialog>();
        }

        public SignInResponse(User user, List<Dialog> dialogs, NetworkResponseStatus status)
        {
            User = user;
            Dialogs = dialogs;
            Status = status;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}