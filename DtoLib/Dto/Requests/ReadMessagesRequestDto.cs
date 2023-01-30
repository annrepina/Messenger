using ProtoBuf;

namespace CommonLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет запрос на прочтение сообщения от сервера клиенту
    /// </summary>
    [ProtoContract]
    public class ReadMessagesRequestDto
    {
        /// <summary>
        /// Список Id прочитанных сообщений
        /// </summary>
        [ProtoMember(1)]
        public List<int> MessagesId { get; init; }

        /// <summary>
        /// Id диалога, которому принадлежат сообщения
        /// </summary>
        [ProtoMember(2)]
        public int DialogId { get; init; }
    }
}