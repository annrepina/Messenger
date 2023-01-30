using Common.NetworkServices;
using ProtoBuf;

namespace Common.Dto.Responses
{
    /// <summary>
    /// Data transfer object представляет собой ответ на запрос о поиске пользователя среди зарегистрированных в мессенджере
    /// </summary>
    [ProtoContract]
    public class UserSearchResponseDto
    {
        /// <summary>
        /// Пользователи, удовлетворяющие поиску
        /// </summary>
        [ProtoMember(1)]
        public List<UserDto> RelevantUsers { get; init; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(2)]
        public NetworkResponseStatus Status { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public UserSearchResponseDto()
        {
            RelevantUsers = new List<UserDto>();
        }
    }
}