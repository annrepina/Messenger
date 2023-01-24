using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    [ProtoContract]
    public class MessagesAreReadRequestForClientDto
    {
        [ProtoMember(1)]
        public List<int> MessagesId { get; set; }

        [ProtoMember(2)]
        public int DialogId { get; set; }
    }
}
