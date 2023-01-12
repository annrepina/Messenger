using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Класс, который представляет ответ на запрос поиска пользователя с удачным результатом 
    /// </summary>
    public class UserSearchResponse
    {
        /// <summary>
        /// Пользователи удовлетворяющие поиску
        /// </summary>
        public List<User> RelevantUsers { get; init; }

        /// <summary>
        /// Статус ответа на сетевой запрос
        /// </summary>
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        public UserSearchResponse(List<User> relevantUsers, NetworkResponseStatus status)
        {
            RelevantUsers = relevantUsers;
            Status = status;
        }
    }
}
