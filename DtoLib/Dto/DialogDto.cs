using ProtoBuf;

namespace CommonLib.Dto
{
    /// <summary>
    /// Data transfer object представляет собой диалог между пользователями
    /// </summary>
    [ProtoContract]
    public class DialogDto
    {
        /// <summary>
        /// Свойство - id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; init; }

        /// <summary>
        /// Свойство - список данных пользователей, участвующих в диалоге
        /// </summary>
        [ProtoMember(2)]
        public List<UserDto> Users { get; init; }

        /// <summary>
        /// Свойство - список сообщений в диалоге
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public List<MessageDto> Messages { get; init; }

        /// <summary>
        /// Конструктор по уомлчанию
        /// </summary>
        public DialogDto()
        {
            Users = new List<UserDto>();
            Messages = new List<MessageDto>();
        }
    }
}