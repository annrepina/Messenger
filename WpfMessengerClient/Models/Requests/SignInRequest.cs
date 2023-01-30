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
    /// Запрос на вход в мессенджер
    /// </summary>
    public class SignInRequest : BaseNotifyPropertyChanged, IDataErrorInfo
    {
        #region Константы

        /// <summary>
        /// Максимальная длина пароля
        /// </summary>
        protected const int MaxLengthOfPassword = 10;

        /// <summary>
        /// Минимальная длина пароля
        /// </summary>
        protected const int MinLengthOfPassword = 6;

        /// <summary>
        /// Длина мобильного телефона
        /// </summary>
        private const int PhoneNumberLength = 12;

        #endregion Константы

        #region Приватные поля

        /// <inheritdoc cref="PhoneNumber"/>
        private string _phoneNumber;

        /// <inheritdoc cref="Password"/>
        private string _password;

        /// <inheritdoc cref="Error"/>
        private string _error;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Ошибка при пооверке на корректность телефона
        /// </summary>
        public string PhoneNumberError { get; protected set; }

        /// <summary>
        /// Ошибка при проверке на корректность пароля
        /// </summary>
        public string PasswordError { get; protected set; }

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

        /// <summary>
        /// Ошибка при валидации свойства
        /// </summary>
        public virtual string Error
        {
            get => _error;

            set
            {
                _error = value;

                OnPropertyChanged(nameof(Error));
            }
        }

        #endregion Свойства 

        #region Конструкторы 

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SignInRequest()
        {
            Password = "";
            PhoneNumber = "";
            Error = "";
            PhoneNumberError = "";
            PasswordError = "";
        }

        #endregion Конструкторы

        #region Валидация 

        /// <summary>
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <returns></returns>
        public virtual string this[string propName]
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
        protected virtual void ValidateProperty(string propName)
        {
            switch (propName)
            {
                case nameof(PhoneNumber):
                    ValidatePhoneNumber();
                    break;

                case nameof(Password):
                    ValidatePassword();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Проверить на корректность пароль
        /// </summary>
        protected virtual void ValidatePassword()
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

            else
            {
                Error = "";
                PasswordError = "";
            }
        }

        /// <summary>
        /// Проверить на корректность номер телефона
        /// </summary>
        protected void ValidatePhoneNumber()
        {
            Regex regex = new Regex(@"^\+7\d{10}");

            //Error = "";

            if (!regex.IsMatch(PhoneNumber))
            {
                Error = "Телефон должен начинаться с +7 и далее состоять из 10 цифр";
                PhoneNumberError = Error;
            }

            else if (PhoneNumber.Length != PhoneNumberLength)
            {
                Error = "Телефон должнен состоять из 12 символов всего";
                PhoneNumberError = Error;
            }

            else
            {
                Error = "";
                PhoneNumberError = "";
            }
        }
        
        /// <summary>
        /// Запрос содержит ошибку?
        /// </summary>
        /// <returns>true - если содержит, false - если нет</returns>
        public virtual bool HasNotErrors()
        {
            return PhoneNumberError == "" && PasswordError == "";
        }

        /// <summary>
        /// Получить ошибку запроса
        /// </summary>
        /// <returns>Ошибка запроса</returns>
        public virtual string GetError()
        {
            if (PhoneNumberError != "")
                return PhoneNumberError;

            return PasswordError;
        }

        #endregion Валидация
    }
}