using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    [ProtoContract]
    public class MessageIsReadRequestDto
    {
        [ProtoMember(1)]
        public List<int> MessagesId { get; set; }
    }
}
