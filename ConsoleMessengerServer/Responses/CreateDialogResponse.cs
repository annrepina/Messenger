using Common.NetworkServices;

namespace ConsoleMessengerServer.Responses
{
    /// <summary>
    /// Класс, который представляет ответ на запрос о создании нового диалога
    /// </summary>
    public class CreateDialogResponse : Response
    {
        /// <summary>
        /// Идентификатор созданного диалога
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор первого сообщения
        /// </summary>
        public int MessageId { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public CreateDialogResponse() : base()
        {
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="status">Статус ответа</param>
        public CreateDialogResponse(NetworkResponseStatus status) : base(status)
        {
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="dialogId">Идентификатор созданного диалога</param>
        /// <param name="messageId">Идентификатор первого сообщения</param>
        public CreateDialogResponse(int dialogId, int messageId, NetworkResponseStatus status) : base(status)
        {
            DialogId = dialogId;
            MessageId = messageId;
        }
    }
}