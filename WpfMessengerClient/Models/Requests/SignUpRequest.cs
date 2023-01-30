using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос на регистрацию в мессенджере
    /// </summary>
    public class SignUpRequest : SignInRequest
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

        /// <inheritdoc cref="RepeatedPassword"/>
        private string _repeatedPassword;

        /// <inheritdoc cref="Name"/>
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

        /// <summary>
        /// Ошибка в имени в запросе
        /// </summary>
        public string NameError { get; protected set; }

        /// <summary>
        /// Ошика в повторяющемся пароле в запросе
        /// </summary>
        public string RepeatedPasswordError { get; protected set; } 

        #endregion Свойства 

        #region Конструкторы 

        /// <summary>
        /// Запрос по умолчанию
        /// </summary>
        public SignUpRequest() : base()
        {
            Name = "";
            RepeatedPassword = "";
            NameError = "";
            RepeatedPasswordError = "";
        }

        #endregion Конструкторы

        #region Валидация 

        /// <summary>
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <returns></returns>
        public override string this[string propName]
        {
            get
            {
                ValidateProperty(propName);

                return Error;
            }
        }

        /// <summary>
        /// Проверить все свойства класса на корректность
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        protected override void ValidateProperty(string propName)
        {
            base.ValidatePhoneNumber();

            switch (propName)
            {
                case nameof(Name):
                    ValidateName();
                    break;

                case nameof(Password):
                    ValidatePassword();
                    break;

                case nameof(RepeatedPassword):
                    ValidateRepeatedPassword();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Проверить на корректность пароль
        /// </summary>
        protected override void ValidatePassword()
        {
            Regex regex = new Regex(@"^\w{6}");

            if (!regex.IsMatch(Password))
            {
                Error = "Пароль может состоять из заглавных и строчных букв, а также цифр";
                PasswordError = Error;
            }

            else if (Password.Length > MaxLengthOfPassword)
            {
                Error = "Пароль должен содержать не больше 10ти символов";
                PasswordError = Error;
            }

            else if (Password.Length < MinLengthOfPassword)
            {
                Error = "Пароль должен содержать не меньше 6ти символов";
                PasswordError = Error;
            }

            else if (RepeatedPassword != "" && Password != RepeatedPassword)
            {
                Error = "Пароли должны совпадать";
                PasswordError = Error;
            }

            else
            {
                Error = "";
                PasswordError = "";
            }
        }

        /// <summary>
        /// Проверить на корректность повторяющийся пароль
        /// </summary>
        protected void ValidateRepeatedPassword()
        {
            Regex regex = new Regex(@"^\w{6}");

            if (!regex.IsMatch(RepeatedPassword))
            {
                Error = "Пароль может состоять из заглавных и строчных букв, а также цифр";
                RepeatedPasswordError = Error;
            }

            else if (RepeatedPassword.Length > MaxLengthOfPassword)
            {
                Error = "Пароль должен содержать не больше 10ти символов";
                RepeatedPasswordError = Error;
            }

            else if (RepeatedPassword.Length < MinLengthOfPassword)
            {
                Error = "Пароль должен содержать не меньше 6ти символов";
                RepeatedPasswordError = Error;
            }

            else if (Password != "" && Password != RepeatedPassword)
            {
                Error = "Пароли должны совпадать";
                RepeatedPasswordError = Error;
            }

            else
            {
                Error = "";
                RepeatedPasswordError = "";
            }
        }

        /// <summary>
        /// Проверить на корретность имя
        /// </summary>
        private void ValidateName()
        {
            Regex regex = new Regex(@"^\w+");

            if (String.IsNullOrEmpty(Name))
            {
                Error = "Имя должно быть не меньше 2х символов";
                NameError = Error;
            }

            else if (!regex.IsMatch(Name))
            {
                Error = "Недопустимые символы";
                NameError = Error;
            }

            else if (Name.Length > MaxNameLength)
            {
                Error = "Имя не должно превышать 50ти символов";
                NameError = Error;
            }

            else if (Name.Length < MinNameLength)
            {
                Error = "Имя должно быть не меньше 2х символов";
                NameError = Error;
            }

            else
            {
                Error = "";
                NameError = "";
            }
        }

        /// <summary>
        /// Есть ошибка в запросе?
        /// </summary>
        /// <returns>true - если есть, false - если нет</returns>
        public override bool HasNotErrors()
        {
            return base.HasNotErrors() && RepeatedPasswordError == "" && NameError == "";
        }

        /// <summary>
        /// Получить ошибку запроса
        /// </summary>
        /// <returns>Ошибка запроса</returns>
        public override string GetError()
        {
            if (base.GetError() != "")
                return base.GetError();

            else if (RepeatedPasswordError != "")
                return RepeatedPasswordError;

            return NameError;
        }

        #endregion Валидация
    }
}