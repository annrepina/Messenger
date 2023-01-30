using ProtoBuf;

namespace CommonLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет расширенный запрос на удаление сообщения от клиента серверу
    /// </summary>
    [ProtoContract]
    public class ExtendedDeleteMessageRequestDto
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        [ProtoMember(1)]
        public int MessageId { get; init; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        [ProtoMember(2)]
        public int DialogId { get; init; }

        /// <summary>
        /// Пользователь, удаливший сообщение
        /// </summary>
        [ProtoMember(3)]
        public int UserId { get; init; }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Id пользователя, удалившего сообщение: {UserId}. Id сообщения: {MessageId}. Id диалога: {DialogId}.";
        }
    }
}