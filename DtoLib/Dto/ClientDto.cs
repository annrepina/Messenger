using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса Client
    /// </summary>
    [ProtoContract]
    public class ClientDto
    {
        /// <summary>
        /// Свойство - id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///// <summary>
        ///// Свойство - Аккаунт пользователя
        ///// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        ///// </summary>
        //[ProtoMember(2)]
        //public UserAccountDto UserAccount { get; set; }


    }
}
