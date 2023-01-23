using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос на выход из мессенджера
    /// </summary>
    public class SignOutRequest
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int UserId { get; init; }

        public SignOutRequest(int userId)
        {
            UserId = userId;
        }
    }
}