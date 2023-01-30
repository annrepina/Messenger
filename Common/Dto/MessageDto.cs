using ProtoBuf;

namespace Common.Dto
{
    /// <summary>
    /// Data transfer object представляет сообщение, которое отправил пользователь
    /// </summary>
    [ProtoContract]
    public class MessageDto
    {
        /// <summary>
        /// Id
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; init; }

        /// <summary>
        /// Текст
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string Text { get; init; }

        /// <summary>
        /// Пользователь-отправитель сообщения
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public UserDto UserSender { get; init; }

        /// <summary>
        /// Прочитано ли сообщение?
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(4)]
        public bool IsRead { get; init; }

        /// <summary>
        /// Дата и время отправки сообщения
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(5)]
        public DateTime DateTime { get; init; }
    }
}