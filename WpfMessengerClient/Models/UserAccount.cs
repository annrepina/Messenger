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
    public class UserAccount : INotifyPropertyChanged, IDataErrorInfo, INetworkMessageHandler
    {
        private const int MaxLengthOfPassword = 10;
        private const int MinLengthOfPassword = 6;

        private int _id;
        private Person _person;
        private string _password;
        private bool _isOnline;
        private string _error;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id 
        { 
            get => _id; 
            
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

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

        public bool IsOnline
        {
            get => _isOnline;

            set
            {
                _isOnline = value;

                OnPropertyChanged(nameof(IsOnline));
            }
        }

        public ObservableCollection<Dialog> Dialogs { get; init; }

        public List<FrontClient> Clients { get; init; }

        public FrontClient CurrentClient { get; set; }

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
                // Сначала присваиваем ошибку, которая есть у человека
                Error = Person.Error;

                // Потом проверяем есть ли у текущего объекта ошибка
                ValidateAllProperties(propName);

                return Error;
            }
        }

        public UserAccount()
        {
            _id = 0;
            _person = new Person();
            _password = "";
            _isOnline = false;
            Dialogs = new ObservableCollection<Dialog>();
            _error = null;
            Clients = new List<FrontClient>();
            CurrentClient = null;
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void AddClient(FrontClient client)
        {
            Clients.Add(client);
        }

        #region Валидация

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

        private void ValidateAllProperties(string propName)
        {
            switch (propName)
            {
                //case nameof(Name):
                //{
                //    //Regex regex = new Regex(@"^8\d{10}");
                //    Regex regex = new Regex(@"^\w+");
                //    //Regex regex = new Regex(@"^\d{10}");                

                //    if (!regex.IsMatch(Name))
                //        error = "Недопустимые символы";

                //    else if (Name.Length > MaxNameLength)
                //        error = "Имя не должно превышать 50ти символов";

                //    else if(Name.Length < MinNameLength)
                //        error = "Имя должно быть не меньше 2х символов";
                //}
                //break;

                case nameof(Password):
                    ValidatePassword();

                    break;

                default:
                    break;
            }

        }
        #endregion Валидация


        public void ProcessNetworkMessage(NetworkMessage message)
        {
            Password = "kuku epta";
        }


    }
}
