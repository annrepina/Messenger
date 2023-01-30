using ProtoBuf;

namespace CommonLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет собой запрос от сервера клиенту на удаление диалога
    /// </summary>
    [ProtoContract]
    public class DeleteDialogRequestDto
    {
        /// <summary>
        /// Id диалога
        /// </summary>
        [ProtoMember(1)]
        public int DialogId { get; init; }
    }
}