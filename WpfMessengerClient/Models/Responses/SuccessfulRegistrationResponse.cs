using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Класс, который представляет данные представляющихе ответ на успешную регистрацию пользователя в мессенджере
    /// </summary>
    public class SuccessfulRegistrationResponse
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; init; }

        /// <summary>
        /// Идентификатор сетевого провайдера, на котором была произведена регистрация
        /// </summary>
        public int NetworkProviderId { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SuccessfulRegistrationResponse(int userId, int networkProviderId)
        {
            UserId = userId;
            NetworkProviderId = networkProviderId;
        }
    }
}
