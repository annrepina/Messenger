using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Класс, который представляет результат поиска пользователя
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
