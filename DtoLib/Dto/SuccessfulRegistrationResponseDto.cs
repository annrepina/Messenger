using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto
{
    /// <summary>
    /// Dto для данных представляющих ответ на успешную регистрацию пользователя в мессенджере
    /// </summary>
    [ProtoContract]
    public class SuccessfulRegistrationResponseDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [ProtoMember(1)]
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор сетевого провайдера, на котором была произведена регистрация
        /// </summary>
        [ProtoMember(2)]
        public int NetworkProviderId { get; set; }
    }
}
