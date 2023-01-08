using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Данные, необходимые при регистрации в мессенджере
    /// </summary>
    public class RegistrationData : LoginData
    {
        #region Константы

        /// <summary>
        /// Максимальная длина имени
        /// </summary>
        private const int MaxNameLength = 50;

        /// <summary>
        /// Минимальная длина имени
        /// </summary>
        private const int MinNameLength = 2;

        #endregion Константы

        #region Приватные поля

        /// <summary>
        /// Повторяющийся пароль
        /// </summary>
        private string _repeatedPassword;

        /// <summary>
        /// Имя
        /// </summary>
        private string _name;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Свойство - повторяйющийся пароль
        /// </summary>
        public string RepeatedPassword
        {
            get => _repeatedPassword;

            set
            {
                _repeatedPassword = value;

                ValidatePassword();

                OnPropertyChanged(nameof(RepeatedPassword));
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

        #endregion Свойства 

        #region Конструкторы 

        public RegistrationData() : base()
        {
            Name = "";
            RepeatedPassword = "";
        }

        #endregion Конструкторы

        #region Реализация интерфейса IDataErrorInfo

        /// <summary>
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <returns></returns>
        public override string this[string propName]
        {
            get
            {
                // Проверяем есть ли у текущего объекта ошибка
                ValidateAllProperties(propName);

                return Error;
            }
        }

        #endregion Реализация интерфейса IDataErrorInfo

        #region Валидация 

        /// <summary>
        /// Проверить все свойства класса на корректность
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        protected override void ValidateAllProperties(string propName)
        {
            base.ValidateAllProperties(propName);

            switch (propName)
            {
                case nameof(Name):
                    ValidateName();
                    break;

                case nameof(RepeatedPassword):
                    ValidatePassword();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Проверить на корректность повторяемый пароль
        /// </summary>
        protected override void ValidatePassword()
        {
            Regex regex = new Regex(@"^\w{6}");

            if (!regex.IsMatch(Password) || !regex.IsMatch(RepeatedPassword))
                Error = "Пароль может состоять из заглавных и строчных букв, а также цифр";

            else if (Password.Length > MaxLengthOfPassword || RepeatedPassword.Length > MaxLengthOfPassword)
                Error = "Пароль должен содержать не больше 10ти символов";

            else if (Password.Length < MinLengthOfPassword || RepeatedPassword.Length < MinLengthOfPassword)
                Error = "Пароль должен содержать не меньше 6ти символов";

            else if (Password != RepeatedPassword)
                Error = "Пароли должны совпадать";
        }

        /// <summary>
        /// Проверить на корретность имя
        /// </summary>
        private void ValidateName()
        {
            Regex regex = new Regex(@"^\w+");

            if (!regex.IsMatch(Name))
                Error = "Недопустимые символы";

            else if (Name.Length > MaxNameLength)
                Error = "Имя не должно превышать 50ти символов";

            else if (Name.Length < MinNameLength)
                Error = "Имя должно быть не меньше 2х символов";
        }

        #endregion Валидация
    }
}