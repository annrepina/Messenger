using Common.NetworkServices;
using ProtoBuf;

namespace Common.Dto.Responses
{
    /// <summary>
    /// Data transfer onject представляющий ответ на запрос о регистрации
    /// </summary>
    [ProtoContract]
    public class SignInResponseDto
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        [ProtoMember(1)]
        public UserDto? User { get; init; }

        /// <summary>
        /// Список диалогов пользователя
        /// </summary>
        [ProtoMember(2)]
        public List<DialogDto>? Dialogs { get; init; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(3)]
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Контекст ошибки, если статус ответа неудачный
        /// </summary>
        [ProtoMember(4)]
        public SignInFailContext Context { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SignInResponseDto()
        {
            Dialogs = new List<DialogDto>();
        }
    }
}