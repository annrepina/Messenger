namespace ConsoleMessengerServer.Requests
{
    /// <summary>
    /// Запрос на удаление сообщения для клиента
    /// </summary>
    public class DeleteMessageRequest
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        public int DialogId { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DeleteMessageRequest()
        {
            MessageId = 0;
            DialogId = 0;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="messageId">Id сообщения</param>
        /// <param name="dialogId">Id диалога</param>
        public DeleteMessageRequest(int messageId, int dialogId)
        {
            MessageId = messageId;
            DialogId = dialogId;
        }
    }
}