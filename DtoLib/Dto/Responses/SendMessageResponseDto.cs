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
    /// Data transfer object для класса представляющего ответ на запрос об отправке сообщения
    /// </summary>
    [ProtoContract]
    public class SendMessageResponseDto 
    {
        /// <summary>
        /// Идентификатор отправленного сообщения
        /// </summary>
        [ProtoMember(1)]
        public int MessageId { get; set; }

        [ProtoMember(2)]
        public NetworkResponseStatus Status { get; set; }
    }
}