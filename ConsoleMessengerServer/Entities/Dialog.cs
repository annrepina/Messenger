namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс, который представляет сущность диалога
    /// </summary>
    public class Dialog
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Список данных пользователей, участвующих в диалоге
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// Список сообщений
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Dialog()
        {
            Users = new List<User>();
            Messages = new List<Message>();
        }
    }
}