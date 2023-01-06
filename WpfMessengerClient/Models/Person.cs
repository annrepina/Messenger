using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Класс, представляющий модель человека
    /// </summary>
    public class Person : BaseNotifyPropertyChanged, IDataErrorInfo
    {
        #region Константы

        /// <summary>
        /// Длина мобильного телефона
        /// </summary>
        private const int PhoneNumberLength = 12;

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
        /// Имя
        /// </summary>
        private string _name;

        /// <summary>
        /// Фамилия
        /// </summary>
        private string? _surname;

        /// <summary>
        /// Мобильный телефон
        /// </summary>
        private string _phoneNumber;

        /// <summary>
        /// Текст ошибки, которая может возникнуть во время валидации данных
        /// </summary>
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
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;

                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Свойство - фамилия
        /// </summary>
        public string? Surname
        {
            get => _surname;

            set
            {
                _surname = value;

                OnPropertyChanged(nameof(Surname));
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

        #endregion Свойства

        #region Конструкторы

        public Person()
        {
            _name = "";
            _surname = "";
            _phoneNumber = "";
            _error = "";
        }

        #endregion Конструкторы

        #region Реализация интерфейса IDataErrorInfo

        /// <summary>
        /// Свойство - ошибка, которая может возникнуть во время валидации
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
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <returns></returns>
        public string this[string propName]
        {
            get
            {
                ValidateAllProperties(propName);

                return Error;
            }
        }

        #endregion Реализация интерфейса IDataErrorInfo

        #region Валидация

        /// <summary>
        /// Проверить номер телефона на корректность
        /// </summary>
        private void ValidatePhoneNumber()
        {
            Regex regex = new Regex(@"^\+7\d{10}");

            Error = "";

            if (!regex.IsMatch(PhoneNumber))
                Error = "Телефон должен начинаться с +7 и далее состоять из 10 цифр";

            else if (PhoneNumber.Length != PhoneNumberLength)
                Error = "Номер телефон должнен состоять из 12 символов всего";
        }

        /// <summary>
        /// Проверить имя на корректность
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

        /// <summary>
        /// Проверить свойство класса на корректность
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        private void ValidateAllProperties(string propName)
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

        #endregion Валидация
    }
}