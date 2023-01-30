using Common.NetworkServices;
using ProtoBuf;

namespace Common.Dto.Responses
{
    /// <summary>
    /// Data transfer object класса, который представляет ответ на запрос о создании нового диалога
    /// </summary>
    [ProtoContract]
    public class CreateDialogResponseDto
    {
        /// <summary>
        /// Идентификатор созданного диалога
        /// </summary>
        [ProtoMember(1)]
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор первого сообщения
        /// </summary>
        [ProtoMember(2)]
        public int MessageId { get; init; }

        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(3)]
        public NetworkResponseStatus Status { get; init; }
    }
}