using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    [ProtoContract]
    public class MessagesAreReadRequestDto
    {
        [ProtoMember(1)]
        public List<int> MessagesId { get; set; }

        /// <summary>
        /// Id пользователя прочитавшего сообщения
        /// </summary>
        [ProtoMember(2)]
        public int UserId { get; set; }

        /// <summary>
        /// Id диалога, которому принадлежат сообщения
        /// </summary>
        [ProtoMember(3)]
        public int DialogId { get; set; }

        public override string ToString()
        {
            return $"Количество прочитанных сообщений: {MessagesId.Count}.";
        }
    }
}
