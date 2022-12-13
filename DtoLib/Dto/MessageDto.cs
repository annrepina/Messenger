using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib.Interfaces;
using DtoLib.Serialization;
using ProtoBuf;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса Message
    /// </summary>
    [ProtoContract]
    public class MessageDto /*: Serializable, IDeserializableDto*/
    {
        /// <summary>
        /// Свойство - id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Свойство - текст
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string Text { get; set; }

        /// <summary>
        /// Свойство - отправляющий сообщение аккаунт пользователя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public UserAccountDto SendingUserAccount { get; set; }

        ///// <summary>
        ///// Свойство - получающий сообщение аккаунт пользователя
        ///// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        ///// </summary>
        //[ProtoMember(4)]
        //public UserAccountDto ReceivingUserAccount { get; set; }

        [ProtoMember(4)]
        public DialogDto Dialog { get; set; }

        /// <summary>
        /// Свойство - прочитано ли сообщение 
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(5)]
        public bool IsRead { get; set; }

        /// <summary>
        /// Свойство - дата и время отправки сообщения
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(6)]
        public DateTime? DateTime { get; set; }
    }
}
