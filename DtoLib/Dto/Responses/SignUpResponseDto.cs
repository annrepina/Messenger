using DtoLib.NetworkServices;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Responses
{
    /// <summary>
    /// Dto для данных представляющих ответ на успешную регистрацию пользователя в мессенджере
    /// </summary>
    [ProtoContract]
    public class SignUpResponseDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [ProtoMember(1)]
        public int UserId { get; init; }

        /// <summary>
        /// Идентификатор сетевого провайдера, на котором была произведена регистрация
        /// </summary>
        [ProtoMember(2)]
        public int NetworkProviderId { get; init; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(3)]
        public NetworkResponseStatus Status { get; init; }
    }
}
