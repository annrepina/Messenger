using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Mapping;
using AutoMapper;
using DtoLib.Dto;
using WpfMessengerClient.Services;
using DtoLib;
using DtoLib.Serialization;
//using DtoLib;

namespace WpfMessengerClient.ViewModels
{
    public class RegistrationWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private const int PhoneNumberLength = 12;
        private const int MaxLengthOfPassword = 10;
        private const int MinLengthOfPassword = 6;

        private Messenger _messenger;
        private string _phoneNumber;
        private string _password;
        private string _error;

        public string PhoneNumber
        {
            get => _phoneNumber;

            set
            {
                _phoneNumber = value;

                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Password 
        { 
            get => _password;

            set
            {
                _password = value;

                OnPropertyChanged(nameof(Password));
            }
        }

        public Messenger Messenger
        {
            get => _messenger;

            set
            {
                _messenger = value;

                OnPropertyChanged(nameof(Messenger));
            }

        }

        public DelegateCommand OnRegisterInMessengerCommand { get; set; }

        public string Error
        {
            get
            {
                return _error;
            }

            set
            {
                _error = value;

                OnPropertyChanged(nameof(Error));
            }
        }

        public string this[string propName]
        {
            get
            {
                // Потом проверяем есть ли у текущего объекта ошибка
                ValidateAllProperties(propName);

                return Error;
            }
        }

        private void ValidateAllProperties(object propName)
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
            Regex regex = new Regex(@"^\w{6}");

            Error = null;

            if (!regex.IsMatch(Password))
                Error = "Пароль может состоять из заглавных и строчных букв, а также цифр";

            else if (Password.Length > MaxLengthOfPassword)
                Error = "Пароль должен содержать не больше 10ти символов";

            else if (Password.Length < MinLengthOfPassword)
                Error = "Пароль должен содержать не меньше 6ти символов";
        }

        private void ValidatePhoneNumber()
        {
            //Regex regex = new Regex(@"^8\d{10}");
            Regex regex = new Regex(@"^\+7\d{10}");
            //Regex regex = new Regex(@"^\d{10}");

            Error = null;

            if (!regex.IsMatch(PhoneNumber))
                Error = "Телефон должен начинаться с +7 и далее состоять из 10 цифр";

            else if (PhoneNumber.Length != PhoneNumberLength)
                Error = "Номер телефон должнен состоять из 12 символов всего";
        }

        public RegistrationWindowViewModel()
        {
            Messenger = new Messenger();
            OnRegisterInMessengerCommand = new DelegateCommand(async () => await OnRegisterInMessenger());
            _password = null;
            _phoneNumber = null;
            _error = null;
        }

        /// <summary>
        /// Метод, вызывающий событие PropertyChanged
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private async Task OnRegisterInMessenger()
        {
            // если ошибок нет
            if (String.IsNullOrEmpty(Error))
            {
                await Messenger.SendRegistrationRequest(PhoneNumber, Password);
            }
        }
    }
}
