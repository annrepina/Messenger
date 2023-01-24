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
    /// Data transfer onject ответа на запрос о регистрации
    /// </summary>
    [ProtoContract]
    public class SignInResponseDto
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        [ProtoMember(1)]
        public UserDto? User { get; set; }

        /// <summary>
        /// Список диалогов пользователя
        /// </summary>
        [ProtoMember(2)]
        public List<DialogDto>? Dialogs { get; set; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(3)]
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Контекст ошибки, если статус ответа неудачный
        /// </summary>
        [ProtoMember(4)]
        public SignInFailContext Context { get; init; }

        public SignInResponseDto()
        {
            Dialogs = new List<DialogDto>();
        }
    }
}
