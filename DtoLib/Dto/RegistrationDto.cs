using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object который представляет информацию, необходимую для регистрации пользователя в мессенджере
    /// </summary>
    [ProtoContract]
    public class RegistrationDto
    {
        /// <summary>
        /// Свойство - объект класса, который представляет человека
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Свойство - пароль
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string Password { get; set; }

        /// <summary>
        /// Свойство - имя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Телефон: {PhoneNumber}. Имя: {Name}";
        }
    }
}