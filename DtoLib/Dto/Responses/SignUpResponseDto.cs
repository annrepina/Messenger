using CommonLib.NetworkServices;
using ProtoBuf;

namespace CommonLib.Dto.Responses
{
    /// <summary>
    /// Data transfer onject представляющий ответ на запрос о регистрации пользователя в мессенджере
    /// </summary>
    [ProtoContract]
    public class SignUpResponseDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [ProtoMember(1)]
        public int UserId { get; init; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(2)]
        public NetworkResponseStatus Status { get; init; }
    }
}