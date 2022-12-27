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
    /// Data transfer object класса UserAccount
    /// </summary>
    [ProtoContract]
    public class UserAccountRegistrationDto
    {
        /// <summary>
        /// Свойство - id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Свойство - объект клааса Person
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Свойство - пароль
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public string Password { get; set; }

        /// <summary>
        /// Свойство - является ли онлайн аккаунт
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(4)]
        public bool IsOnline { get; set; }

        /// <summary>
        /// Свойство - обозреваемая коллекция диалогов у аккаунта
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(5)]
        public ObservableCollection<DialogDto> Dialogs { get; set; }

        /// <summary>
        /// Свойство - список клиентов у аккаунта, те клиенты на которых открыта учетка аккаунта
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(6)]
        public List<ClientDto> Clients { get; set; }

    }
}
