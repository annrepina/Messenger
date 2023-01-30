namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс - сущность сообщения
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Id сообщения
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Пользователь - отправитель сообщения
        /// </summary>
        public User UserSender { get; set; }

        /// <summary>
        /// Id пользователя - отправителя сообщения
        /// </summary>
        public int UserSenderId { get; set; }

        /// <summary>
        /// Диалог в котором существует сообщение
        /// </summary>
        public Dialog Dialog { get; set; }

        /// <summary>
        /// Id диалога в котором существует сообщение
        /// </summary>
        public int DialogId { get; set; }

        /// <summary>
        /// Прочитано сообщение?
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Дата и время отправки сообщения
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}