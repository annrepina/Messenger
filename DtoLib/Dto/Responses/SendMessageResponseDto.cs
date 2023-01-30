using CommonLib.NetworkServices;
using ProtoBuf;

namespace CommonLib.Dto.Responses
{
    /// <summary>
    /// Data transfer object для класса представляющего ответ на запрос об отправке сообщения
    /// </summary>
    [ProtoContract]
    public class SendMessageResponseDto
    {
        /// <summary>
        /// Идентификатор отправленного сообщения
        /// </summary>
        [ProtoMember(1)]
        public int MessageId { get; init; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(2)]
        public NetworkResponseStatus Status { get; init; }
    }
}