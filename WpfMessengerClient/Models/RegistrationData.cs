using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Данные, необходимые при регистрации в мессенджере
    /// </summary>
    public class RegistrationData : BaseNotifyPropertyChanged, IDataErrorInfo
    {
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
        /// Повторяющийся пароль
        /// </summary>
        private string _repeatedPassword;

        /// <summary>
        /// Логин
        /// </summary>
        private string _login;

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
        /// Свойство - логин
        /// </summary>
        public string Login 
        { 
            get => _login; 

            set
            {
                _login = value;

                OnPropertyChanged(nameof(Login));
            }
        }

        #endregion Свойства 

        #region Конструкторы 

        public RegistrationData()
        {
            Login = "";
            Password = "";
            RepeatedPassword = "";
            PhoneNumber = "";
        }

        #endregion Конструкторы

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
        private void ValidateAllProperties(string propName)
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

        private void ValidatePassword()
        {
            throw new NotImplementedException();
        }

        private void ValidatePhoneNumber()
        {
            throw new NotImplementedException();
        }

        #endregion Валидация
    }
}