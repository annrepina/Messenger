using Common.NetworkServices;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Представляет ответ на запрос об отправке сообщения для пользователя, который его отправил.
    /// Подтверждает, что сообщение отправлено.
    /// </summary>
    public class SendMessageResponse : Response
    {
        /// <summary>
        /// Id отправленного сообщения
        /// </summary>
        public int MessageId { get; init; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="messageId">Id отправленного сообщения</param>
        /// <param name="status">Статус ответа</param>
        public SendMessageResponse(int messageId, NetworkResponseStatus status) : base(status)
        {
            MessageId = messageId;
        }
    }
}