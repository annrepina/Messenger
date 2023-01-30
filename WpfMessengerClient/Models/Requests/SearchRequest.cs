using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Класс - представляет запрос по поиску пользователя среди зарегистрированных в мессенджере
    /// </summary>
    public class SearchRequest : BaseNotifyPropertyChanged, IDataErrorInfo
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

        /// <summary>
        /// Длина мобильного телефона
        /// </summary>
        private const int PhoneNumberLength = 12;

        #endregion Константы

        #region Приватные поля

        /// <inheritdoc cref="Name"/>
        private string _name;

        /// <inheritdoc cref="PhoneNumber"/>
        private string _phoneNumber;

        /// <inheritdoc cref="Error"
        private string _error;

        #endregion Приватные поля

        #region Свойства

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
        /// Ошибка при валидации свойства
        /// </summary>
        public string Error
        {
            get => _error;

            set
            {
                _error = value;

                OnPropertyChanged(nameof(Error));
            }
        }

        /// <summary>
        /// Ошибка при валидации телефона
        /// </summary>
        public string PhoneNumberError { get; private set; }

        /// <summary>
        /// Ошибка при валидации имени
        /// </summary>
        public string NameError { get; private set; }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SearchRequest()
        {
            Name = "";
            PhoneNumber = "";
            PhoneNumberError = "";
            NameError = "";
        }

        #endregion Конструкторы

        /// <summary>
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <returns>Возвращает ошибку, полученную во время валидации</returns>
        public string this[string propName]
        {
            get
            {
                ValidateProperty(propName);

                return Error;
            }
        }

        /// <summary>
        /// Проверить на корректность свойство
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        private void ValidateProperty(string propName)
        {
            switch (propName)
            {
                case nameof(Name):
                    ValidateName();
                    break;

                case nameof(PhoneNumber):
                    ValidatePhoneNumber();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Проверить на корректность номер телефона
        /// </summary>
        private void ValidatePhoneNumber()
        {
            Regex regex = new Regex(@"^\+7\d{10}");

            if (!String.IsNullOrEmpty(PhoneNumber) && (!regex.IsMatch(PhoneNumber) || PhoneNumber.Length > PhoneNumberLength))
            {
                Error = "Телефон должен начинаться с +7\nи не превышать 12 символов";
                PhoneNumberError = Error;
            }

            else
            {
                Error = "";
                PhoneNumberError = "";
            }
        }

        /// <summary>
        /// Проверить на корретность имя
        /// </summary>
        private void ValidateName()
        {
            Regex regex = new Regex(@"^\w+");

            if (!String.IsNullOrEmpty(Name) && Name.Length < MinNameLength)
            {
                Error = "Имя должно быть не меньше 2х символов";
                NameError = Error;
            }

            else if (!String.IsNullOrEmpty(Name) && !regex.IsMatch(Name))
            {
                Error = "Недопустимые символы в имени";
                NameError = Error;
            }

            else if (Name.Length > MaxNameLength)
            {
                Error = "Имя не должно превышать 50ти символов";
                NameError = Error;
            }

            else
            {
                Error = "";
                NameError = "";
            }

        }

        /// <summary>
        /// Запрос содержит ошибку?
        /// </summary>
        /// <returns>true - если содержит, false - если нет</returns>
        public bool HasNotErrors()
        {
            return PhoneNumberError == "" && NameError == "" && (PhoneNumber != "" || Name != "");
        }

        /// <summary>
        /// Получить текст ошибки запроса
        /// </summary>
        /// <returns>текст ошибки запроса</returns>
        public string GetError()
        {
            if (PhoneNumberError != "")
                return PhoneNumberError;

            else if (NameError != "")
                return NameError;

            return "Заполните одно из полей поиска";
        }
    }
}