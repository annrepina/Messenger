using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса, который представляет собой результат поиска пользователей
    /// </summary>
    [ProtoContract]
    public class UserSearchResultDto
    {
        /// <summary>
        /// Пользователи, удовлетворяющие поиску
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        List<UserDto> RelevantUsers { get; set; }
    }
}
