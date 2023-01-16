using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    [ProtoContract]
    public class DeleteDialogRequestDto
    {
        /// <summary>
        /// Идентификатор диалога, который нужно удалить
        /// </summary>
        [ProtoMember(1)]
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор пользователя, удалившего диалог
        /// </summary>
        [ProtoMember(2)]
        public int UserId { get; init; }
    }
}
