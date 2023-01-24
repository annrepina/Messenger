using DtoLib.NetworkServices;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Responses
{
    [ProtoContract]
    public class ResponseDto
    {
        [ProtoMember(1)]
        public NetworkResponseStatus Status { get; set; }
    }
}