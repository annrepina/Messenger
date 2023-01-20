using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object для запроса на вход в мессенджер
    /// </summary>
    [ProtoContract]
    public class SignInRequestDto
    {
        /// <summary>
        /// Свойство - номер телефона
        /// </summary>
        [ProtoMember(1)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Свойство - пароль
        /// </summary>
        [ProtoMember(2)]
        public string Password { get; set; }

        public override string ToString()
        {
            return $"Телефон: {PhoneNumber}.";
        }
    }
}
