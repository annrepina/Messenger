using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object для класса DeleteMessageRequestForClient
    /// </summary>
    [ProtoContract]
    public class DeleteMessageRequestForClientDto
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [ProtoMember(1)]
        public int MessageId { get; set; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        [ProtoMember(2)]
        public int DialogId { get; set; }
    }
}