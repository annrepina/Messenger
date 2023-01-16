using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object класса MessageRequest
    /// </summary>
    [ProtoContract]
    public class SendMessageRequestDto
    {
        /// <summary>
        /// Сообщение
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public MessageDto Message { get; set; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public int DialogId { get; set; }
    }
}