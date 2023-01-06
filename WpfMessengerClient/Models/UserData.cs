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
    public class UserData : BaseNotifyPropertyChanged, IDataErrorInfo
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

        #endregion Константы

        #region Приватные поля

        /// <summary>
        /// Идентификатор
        /// </summary>
        private int _id;

        /// <summary>
        /// Человек
        /// </summary>
        private Person _person;

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
        /// Свойство - человек
        /// </summary>
        public Person Person 
        { 
            get => _person;
            
            set
            {
                if(value != null)
                {
                    _person = value;

                    OnPropertyChanged(nameof(Person));
                }            
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
                // Сначала присваиваем ошибку, которая есть у человека
                Error = Person.Error;

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
        public UserData()
        {
            _id = 0;
            _person = new Person();
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

                default:
                    break;
            }

        }
        #endregion Валидация
    }
}