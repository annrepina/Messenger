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
    /// Data transfer object класса, который представляет ответ на запрос о создании нового диалога
    /// </summary>
    [ProtoContract]
    public class CreateDialogResponseDto
    {
        /// <summary>
        /// Идентификатор созданного диалога
        /// </summary>
        [ProtoMember(1)]
        public int DialogId { get; set; }

        /// <summary>
        /// Идентификатор первого сообщения
        /// </summary>
        [ProtoMember(2)]
        public int MessageId { get; set; }

        //[ProtoMember(3)]
        //public NetworkResponseStatus Status { get; init; }
    }
}