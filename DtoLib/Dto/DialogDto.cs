using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib.Serialization;
using ProtoBuf;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса Dialog
    /// </summary>
    [ProtoContract]
    public class DialogDto
    {
        /// <summary>
        /// Свойство - id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Свойство - массив данных пользователей, участвующих в диалоге
        /// </summary>
        [ProtoMember(2)]
        public List<UserDto> Users { get; set; }

        /// <summary>
        /// Свойство - обозреваемая коллекция сообщений в диалоге
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public ObservableCollection<MessageDto> Messages { get; set; }
    }
}