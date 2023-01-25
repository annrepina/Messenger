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

        [ProtoMember(2)]
        public NetworkResponseStatus Status { get; set; }
    }
}
