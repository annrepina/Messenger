using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfMessengerClient.Services;
using DtoLib;

namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Класс - модель данных пользователя
    /// </summary>
    public class User : BaseNotifyPropertyChanged, IDataErrorInfo
    {
        #region Константы

        /// <summary>
        /// Максимальная длина пароля
        /// </summary>
        private const int MaxLengthOfPassword = 10;

        /// <summary>
        /// Минимальная длина пароля
        /// </summary>
        private const int MinLengthOfPassword = 6;

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
        /// Идентификатор
        /// </summary>
        private int _id;

        /// <summary>
        /// Имя
        /// </summary>
        private string _name;

        /// <summary>
        /// Мобильный телефон
        /// </summary>
        private string _phoneNumber;

        /// <summary>
        /// Пароль
        /// </summary>
        private string _password;

        /// <summary>
        /// Пользователь онлайн?
        /// </summary>
        private bool _isOnline;

        /// <summary>
        /// Ошибка при валидации свойств
        /// </summary>
        private string _error;

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
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;

                    OnPropertyChanged(nameof(Name));
                }
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
                if (!string.IsNullOrEmpty(value))
                {
                    _password = value;

                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        /// <summary>
        /// Свойство - пользователь онлайн?
        /// </summary>
        public bool IsOnline
        {
            get => _isOnline;

            set
            {
                _isOnline = value;

                OnPropertyChanged(nameof(IsOnline));
            }
        }

        /// <summary>
        /// Свойство - обозреваемая коллекция - список диалогов
        /// </summary>
        public ObservableCollection<Dialog> Dialogs { get; set; }

        #endregion Свойства

        #region Реализация интерфейса IDataErrorInfo

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
        /// Получает сообщение об ошибке для свойства с заданным именем по индексатору
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        /// <returns></returns>
        public string this[string propName]
        {
            get
            {
                // Потом проверяем есть ли у текущего объекта ошибка
                ValidateAllProperties(propName);

                return Error;
            }
        }

        #endregion Реализация интерфейса IDataErrorInfo

        #region Конструкторы

        /// <summary>
        /// Конструкторы по умолчанию
        /// </summary>
        public User()
        {
            _id = 0;
            _name = "";
            _phoneNumber = "";
            _password = "";
            _isOnline = false;
            Dialogs = new ObservableCollection<Dialog>();
            _error = null;
        }

        #endregion Конструкторы

        #region Валидация

        /// <summary>
        /// Проверить пароль на корректность
        /// </summary>
        private void ValidatePassword()
        {
            Regex regex = new Regex(@"^\w{6}");

            Error = null;

            if (!regex.IsMatch(Password))
                Error = "Пароль может состоять из заглавных и строчных букв, а также цифр";

            else if (Password.Length > MaxLengthOfPassword)
                Error = "Пароль должен содержать не больше 10ти символов";

            else if (Password.Length < MinLengthOfPassword)
                Error = "Пароль должен содержать не меньше 6ти символов";
        }

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
        /// Проверить все свойства на корректность
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        private void ValidateAllProperties(string propName)
        {
            switch (propName)
            {
                case nameof(Password):
                    ValidatePassword();
                    break;

                case nameof(PhoneNumber):
                    ValidatePhoneNumber();
                    break;

                case nameof(Name):
                    ValidateName();
                    break;

                default:
                    break;
            }

        }
        #endregion Валидация
    }
}