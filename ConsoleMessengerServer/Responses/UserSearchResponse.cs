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
    /// Результат поиска пользователей в мессенджере
    /// </summary>
    public class UserSearchResponse
    {
        /// <summary>
        /// Пользователи удовлетворяющие поиску
        /// </summary>
        public List<User> RelevantUsers { get; set; }

        /// <summary>
        /// Статус ответа на сетевой запрос
        /// </summary>
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserSearchResponse()
        {
            RelevantUsers = new List<User>();
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserSearchResponse(List<User> relevantUsers, NetworkResponseStatus status)
        {
            RelevantUsers = relevantUsers;
            Status = status;
        }
    }
}
