using ProtoBuf;

namespace CommonLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет собой запрос на создание диалога
    /// </summary>
    [ProtoContract]
    public class CreateDialogRequestDto
    {
        /// <summary>
        /// Свойство - массив пользователей, участвующих в диалоге
        /// </summary>
        [ProtoMember(1)]
        public List<int> UsersId { get; init; }

        /// <summary>
        /// Обозреваемая коллекция сообщений в диалоге
        /// </summary>
        [ProtoMember(2)]
        public List<MessageDto> Messages { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public CreateDialogRequestDto()
        {
            UsersId = new List<int>();
            Messages = new List<MessageDto>();
        }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Пользователь с Id: {UsersId.First()} хочет создать диалог с пользователем с Id: {UsersId.Last()}";
        }
    }
}