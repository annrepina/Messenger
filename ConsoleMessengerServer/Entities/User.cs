namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс - сущность данных пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Список диалогов
        /// </summary>
        public List<Dialog> Dialogs { get; set; }

        /// <summary>
        /// Список отправленных сообщений
        /// </summary>
        public List<Message> SentMessages { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public User()
        {
            Dialogs = new List<Dialog>();
            SentMessages = new List<Message>();
        }

        /// <summary>
        /// Перегрузка метода ToString()
        /// </summary>
        /// <returns>Строковое представление объекта класса</returns>
        public override string ToString()
        {
            return $"Id: {Id}. Имя: {Name}. Телефон: {PhoneNumber}. Пароль: {Password}";
        }
    }
}