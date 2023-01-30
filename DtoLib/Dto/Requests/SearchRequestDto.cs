using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет информацию, необходимую для поиска пользователя в мессенджере
    /// </summary>
    [ProtoContract]
    public class SearchRequestDto
    {
        /// <summary>
        /// Свойство - имя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// Свойство - номер телефона
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Name))
                return $"Телефон: {PhoneNumber}.";

            else if (String.IsNullOrEmpty(PhoneNumber))
                return $"Имя: {Name}.";

            return $"Имя: {Name}. Телефон: {PhoneNumber}.";
        }
    }
}