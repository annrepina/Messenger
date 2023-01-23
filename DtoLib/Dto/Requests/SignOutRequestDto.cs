using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object для класса ExitRequest
    /// </summary>
    [ProtoContract]
    public class SignOutRequestDto
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        [ProtoMember(1)]
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"Id пользователя: {UserId}";
        }
    }
}