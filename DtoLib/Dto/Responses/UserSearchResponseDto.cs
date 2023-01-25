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
    /// Data transfer object класса, который представляет собой результат поиска пользователей
    /// </summary>
    [ProtoContract]
    public class UserSearchResponseDto
    {
        /// <summary>
        /// Пользователи, удовлетворяющие поиску
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public List<UserDto> RelevantUsers { get; init; }

        [ProtoMember(2)]
        public NetworkResponseStatus Status { get; set; }

        public UserSearchResponseDto()
        {
            RelevantUsers = new List<UserDto>();
        }
    }
}
