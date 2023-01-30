using ProtoBuf;

namespace CommonLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object который представляет расширенный запрос на прочтение сообщения от клиента серверу
    /// </summary>
    [ProtoContract]
    public class MessagesReadRequestDto
    {
        /// <summary>
        /// Список Id прочитанных сообщений
        /// </summary>
        [ProtoMember(1)]
        public List<int> MessagesId { get; set; }

        /// <summary>
        /// Id пользователя прочитавшего сообщения
        /// </summary>
        [ProtoMember(2)]
        public int UserId { get; set; }

        /// <summary>
        /// Id диалога, которому принадлежат сообщения
        /// </summary>
        [ProtoMember(3)]
        public int DialogId { get; set; }

        /// <summary>
        /// Перегрузка ToString()
        /// </summary>
        /// <returns>Строковое представление класса</returns>
        public override string ToString()
        {
            return $"Количество прочитанных сообщений: {MessagesId.Count}.";
        }
    }
}