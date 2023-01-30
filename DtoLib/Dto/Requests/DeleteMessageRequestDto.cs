using ProtoBuf;

namespace CommonLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет запрос на удаление сообщения от сервера клиенту
    /// </summary>
    [ProtoContract]
    public class DeleteMessageRequestDto
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [ProtoMember(1)]
        public int MessageId { get; init; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        [ProtoMember(2)]
        public int DialogId { get; init; }
    }
}