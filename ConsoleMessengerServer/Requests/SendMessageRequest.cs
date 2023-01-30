using ConsoleMessengerServer.Entities;

namespace ConsoleMessengerServer.Requests
{
    /// <summary>
    /// Запрос на отправку сообщения
    /// </summary>
    public class SendMessageRequest
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public Message Message { get; set; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        public int DialogId { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SendMessageRequest()
        {
            Message = new Message();
            DialogId = 0;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="dialogId">Идентификатор диалога</param>
        public SendMessageRequest(Message message, int dialogId)
        {
            Message = message;
            DialogId = dialogId;
        }
    }
}