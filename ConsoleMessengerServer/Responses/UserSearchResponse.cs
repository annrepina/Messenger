using ConsoleMessengerServer.Entities;
using CommonLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Результат поиска пользователей в мессенджере
    /// </summary>
    public class UserSearchResponse : Response
    {
        /// <summary>
        /// Пользователи удовлетворяющие поиску
        /// </summary>
        public List<User> RelevantUsers { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserSearchResponse(NetworkResponseStatus status) : base(status)
        {
            RelevantUsers = new List<User>();
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserSearchResponse(List<User> relevantUsers, NetworkResponseStatus status) : base(status)
        {
            RelevantUsers = relevantUsers;
        }
    }
}
