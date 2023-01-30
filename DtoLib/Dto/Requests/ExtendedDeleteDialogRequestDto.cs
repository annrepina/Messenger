using ProtoBuf;

namespace CommonLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет собой расширенный запрос от клиента серверу на удаление диалога
    /// </summary>
    [ProtoContract]
    public class ExtendedDeleteDialogRequestDto
    {
        /// <summary>
        /// Идентификатор диалога, который нужно удалить
        /// </summary>
        [ProtoMember(1)]
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор пользователя, удалившего диалог
        /// </summary>
        [ProtoMember(2)]
        public int UserId { get; init; }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Пользователь - Id: {UserId} отправил запрос на удаление диалога - Id: {DialogId}.";
        }
    }
}