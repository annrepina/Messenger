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
    /// Данные, необходимые при регистрации в мессенджере
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

        /// <summary>
        /// Повторяющийся пароль
        /// </summary>
        private string _repeatedPassword;

        /// <summary>
        /// Имя
        /// </summary>
        private string _name;

        //private bool HasRepeatedPasswordError;
        //private bool _hasNameError;

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

        public string NameError { get; protected set; }
        public string RepeatedPasswordError { get; protected set; } 

        #endregion Свойства 

        #region Конструкторы 

        public SignUpRequest() : base()
        {
            Name = "";
            RepeatedPassword = "";
            //HasRepeatedPasswordError = true;
            //_hasNameError = true;
            NameError = "";
            RepeatedPasswordError = "";
        }

        #endregion Конструкторы

        #region Реализация интерфейса IDataErrorInfo

        /// <summary>
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param _name="propName">Имя свойства</param>
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
        /// <param _name="propName">Имя свойства</param>
        protected override void ValidateAllProperties(string propName)
        {
            base.ValidatePhoneNumber();

            switch (propName)
            {
                case nameof(Name):
                    ValidateName();
                    break;

                case nameof(Password):
                    //ValidatePassword(Password, RepeatedPassword, PasswordError);
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
        private void ValidatePassword(string validatedPassword, string secondPassword, string paswordError)
        {
            Regex regex = new Regex(@"^\w{6}");

            //Error = "";

            if (!regex.IsMatch(validatedPassword))
            {
                Error = "Пароль может состоять из заглавных и строчных букв, а также цифр";
                paswordError = Error;
            }

            else if (validatedPassword.Length > MaxLengthOfPassword)
            {
                Error = "Пароль должен содержать не больше 10ти символов";
                paswordError = Error;
            }

            else if (validatedPassword.Length < MinLengthOfPassword)
            {
                Error = "Пароль должен содержать не меньше 6ти символов";
                paswordError = Error;
            }

            else if (secondPassword != "" && validatedPassword != secondPassword)
            {
                Error = "Пароли должны совпадать";
                paswordError = Error;
            }

            else
            {
                Error = "";
                paswordError = "";
            }
        }

        protected override void ValidatePassword()
        {
            Regex regex = new Regex(@"^\w{6}");

            //Error = "";

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

        protected void ValidateRepeatedPassword()
        {
            Regex regex = new Regex(@"^\w{6}");

            //Error = "";

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

            //Error = "";

            if (String.IsNullOrEmpty(Name))
            {
                Error = "Имя должно быть не меньше 2х символов";
                //_hasNameError = true;
                NameError = Error;
            }

            else if (!regex.IsMatch(Name))
            {
                Error = "Недопустимые символы";
                //_hasNameError = true;
                NameError = Error;
            }

            else if (Name.Length > MaxNameLength)
            {
                Error = "Имя не должно превышать 50ти символов";
                //_hasNameError = true;
                NameError = Error;
            }

            else if (Name.Length < MinNameLength)
            {
                Error = "Имя должно быть не меньше 2х символов";
                //_hasNameError = true;
                NameError = Error;
            }

            //else if (HasPasswordError == false && HasRepeatedPasswordError == false && HasPhoneNumberError == false)
            //{
            //    Error = "";
            //    _hasNameError = false;
            //}

            else
            {
                //_hasNameError = false;
                Error = "";
                NameError = "";
            }

        }

        #endregion Валидация

        public override bool HasNotErrors()
        {
            if(base.HasNotErrors() && RepeatedPasswordError == "" && NameError == "")
                return true;

            return false;
        }

        public override string GetError()
        {
            if (base.GetError() != "")
                return base.GetError();

            else if (RepeatedPasswordError != "")
                return RepeatedPasswordError;

            return NameError;
        }
    }
}