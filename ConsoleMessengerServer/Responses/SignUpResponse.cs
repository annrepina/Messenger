using DtoLib.NetworkServices;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Класс, который представляет данные представляющихе ответ на успешную регистрацию пользователя в мессенджере
    /// </summary>
    public class SignUpResponse
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор сетевого провайдера, на котором была произведена регистрация
        /// </summary>
        public int NetworkProviderId { get; set; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SignUpResponse(int userId, int networkProviderId, NetworkResponseStatus status) : this(status)
        {
            UserId = userId;
            NetworkProviderId = networkProviderId;
        }

        public SignUpResponse(NetworkResponseStatus status)
        {
            Status = status;
        }
    }
}