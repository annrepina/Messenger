using DtoLib.NetworkServices;
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
    public class RegistrationResponse
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
        /// Статус ответа на сетевой запрос
        /// </summary>
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public RegistrationResponse(int userId, int networkProviderId, NetworkResponseStatus status)
        {
            UserId = userId;
            NetworkProviderId = networkProviderId;
            Status = status;
        }
    }
}
