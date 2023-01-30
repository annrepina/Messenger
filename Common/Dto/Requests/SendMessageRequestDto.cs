using ProtoBuf;

namespace Common.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет запрос на отправку сообщения пользователем
    /// </summary>
    [ProtoContract]
    public class SendMessageRequestDto
    {
        /// <summary>
        /// Сообщение
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public MessageDto Message { get; init; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public int DialogId { get; init; }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Id диалога: {DialogId}. Id отправителя: {Message.UserSender.Id}. Текст сообщения: {Message.Text}";
        }
    }
}