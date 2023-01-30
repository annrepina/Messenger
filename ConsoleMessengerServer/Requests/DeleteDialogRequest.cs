namespace ConsoleMessengerServer.Requests
{
    /// <summary>
    /// Запрос от сервера клиенту об удалении диалога
    /// </summary>
    public class DeleteDialogRequest
    {
        /// <summary>
        /// Id диалога
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="dialogId">Id диалога</param>
        public DeleteDialogRequest(int dialogId)
        {
            DialogId = dialogId;
        }
    }
}