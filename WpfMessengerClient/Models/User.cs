namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Класс - модель пользователя
    /// </summary>
    public class User : BaseNotifyPropertyChanged
    {
        #region Приватные поля

        /// <inheritdoc cref="Id"/>
        private int _id;

        /// <inheritdoc cref="Name"/>
        private string _name;

        /// <inheritdoc cref="PhoneNumber"/>
        private string _phoneNumber;

        /// <inheritdoc cref="Password"/>
        private string _password;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Свойство - идентификатор
        /// </summary>
        public int Id
        {
            get => _id;

            set
            {
                _id = value;

                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Свойство - имя
        /// </summary>
        public string Name
        {
            get => _name;

            set
            {
                _name = value;

                OnPropertyChanged(nameof(Name));
            }
        }


        /// <summary>
        /// Свойство - номер телефона
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;

            set
            {
                _phoneNumber = value;

                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        /// <summary>
        /// Свойство - пароль
        /// </summary>
        public string Password
        {
            get => _password;

            set
            {
                _password = value;

                OnPropertyChanged(nameof(Password));
            }
        }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструкторы по умолчанию
        /// </summary>
        public User()
        {
            Id = 0;
            Name = "";
            PhoneNumber = "";
            Password = "";
        }

        #endregion Конструкторы
    }
}