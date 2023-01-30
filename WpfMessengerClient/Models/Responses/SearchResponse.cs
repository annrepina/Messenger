using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Класс, который представляет ответ на запрос поиска пользователя среди зарегестированных в мессенджере
    /// </summary>
    public class SearchResponse : Response
    {
        /// <summary>
        /// Пользователи удовлетворяющие поиску
        /// </summary>
        public List<User> RelevantUsers { get; init; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="relevantUsers">Пользователи удовлетворяющие поиску</param>
        /// <param name="status">Статус ответа</param>
        public SearchResponse(List<User> relevantUsers, NetworkResponseStatus status) : base(status)
        {
            RelevantUsers = relevantUsers;
        }
    }
}