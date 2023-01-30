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
    /// Данные о поисковом запросе
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

        /// <summary>
        /// Имя
        /// </summary>
        private string _name;

        /// <summary>
        /// Номер телефона
        /// </summary>
        private string _phoneNumber;
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

        public string PhoneNumberError { get; private set; }
        public string NameError { get; private set; }

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
                //HasPhoneNumberError = false;
            }
        }

        /// <summary>
        /// Проверить на корретность имя
        /// </summary>
        private void ValidateName()
        {
            Regex regex = new Regex(@"^\w+");

            //Error = "";

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

            //else if (HasPasswordError == false && HasRepeatedPasswordError == false && HasPhoneNumberError == false)
            //{
            //    Error = "";
            //    _hasNameError = false;
            //}

            else
            {
                Error = "";
                NameError = "";
            }

        }

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

        public bool HasNotErrors()
        {
            if (PhoneNumberError == "" && NameError == "" && (PhoneNumber != "" || Name != ""))
                return true;

            return false;
        }

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