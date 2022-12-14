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
    /// Информация необходимая для входа в мессенджер
    /// </summary>
    public class LoginRequest : BaseNotifyPropertyChanged, IDataErrorInfo
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

        /// <summary>
        /// Номер телефона
        /// </summary>
        private string _phoneNumber;

        /// <summary>
        /// Пароль
        /// </summary>
        private string _password;

        /// <summary>
        /// Ошибка, которая может возникнуть при валидации данных класса
        /// </summary>
        private string _error;

        #endregion Приватные поля

        #region Свойства

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
        /// Конструктор по умолчанию
        /// </summary>
        public LoginRequest()
        {
            Password = "";
            PhoneNumber = "";
            Error = "";
        }

        #endregion Конструкторы

        #region Реализация интерфейса IDataErrorInfo

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

        /// <summary>
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param _name="propName">Имя свойства</param>
        /// <returns></returns>
        public virtual string this[string propName]
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
        protected virtual void ValidateAllProperties(string propName)
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

            Error = "";

            if (!regex.IsMatch(Password))
                Error = "Пароль может состоять из заглавных и строчных букв, а также цифр";

            else if (Password.Length > MaxLengthOfPassword)
                Error = "Пароль должен содержать не больше 10ти символов";

            else if (Password.Length < MinLengthOfPassword)
                Error = "Пароль должен содержать не меньше 6ти символов";
        }

        /// <summary>
        /// Проверить на корректность номер телефона
        /// </summary>
        protected void ValidatePhoneNumber()
        {
            Regex regex = new Regex(@"^\+7\d{10}");

            Error = "";

            if (!regex.IsMatch(PhoneNumber))
                Error = "Телефон должен начинаться с +7 и далее состоять из 10 цифр";

            else if (PhoneNumber.Length != PhoneNumberLength)
                Error = "Номер телефон должнен состоять из 12 символов всего";
        }

        #endregion Валидация
    }
}