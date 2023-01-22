using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib.Serialization;
using ProtoBuf;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object класса Dialog
    /// </summary>
    [ProtoContract]
    public class CreateDialogRequestDto
    {
        ///// <summary>
        ///// Свойство - id
        ///// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        ///// </summary>
        //[ProtoMember(1)]
        //public int Id { get; set; }

        /// <summary>
        /// Свойство - массив данных пользователей, участвующих в диалоге
        /// </summary>
        [ProtoMember(1)]
        public List<int> UsersId { get; set; }

        /// <summary>
        /// Свойство - обозреваемая коллекция сообщений в диалоге
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public List<MessageDto> Messages { get; set; }

        public CreateDialogRequestDto()
        {
            UsersId = new List<int>();
            Messages = new List<MessageDto>();
        }

        public override string ToString()
        {
            return $"Пользователь с Id: {UsersId.First()} хочет создать диалог с пользователем с Id: {UsersId.Last()}";
        }
    }
}