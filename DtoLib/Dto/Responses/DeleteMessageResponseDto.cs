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
    /// Data transfer object для класса DeleteMessageResponse
    /// </summary>
    [ProtoContract]
    public class DeleteMessageResponseDto
    {
        [ProtoMember(1)]
        public NetworkResponseStatus Status { get; set; }
    }
}