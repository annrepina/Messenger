using ConsoleMessengerServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Результат поиска пользователей в мессенджере
    /// </summary>
    public class UserSearchResult
    {
        /// <summary>
        /// Пользователи удовлетворяющие поиску
        /// </summary>
        public List<User> RelevantUsers { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserSearchResult(List<User> relevantUsers)
        {
            RelevantUsers = relevantUsers;
        }
    }
}
