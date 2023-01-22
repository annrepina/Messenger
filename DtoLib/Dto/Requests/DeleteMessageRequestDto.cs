using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object класса Dialog
    /// </summary>
    [ProtoContract]
    public class DeleteMessageRequestDto
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        [ProtoMember(1)]
        public int MessageId { get; set; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        [ProtoMember(2)]
        public int DialogId { get; set; }

        /// <summary>
        /// Пользователь, удаливший сообщение
        /// </summary>
        [ProtoMember(3)]
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"Id пользователя, удалившего сообщение: {UserId}. Id сообщения: {MessageId}. Id диалога: {DialogId}.";
        }
    }
}