using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    public class UserAccount : INotifyPropertyChanged, IDataErrorInfo
    {
        private const int MaxLengthOfPassword = 10;
        private const int MinLengthOfPassword = 6;

        private int _id;
        private Person _person;
        private string _password;
        private bool _isOnline;

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

        public ObservableCollection<Dialog> Dialogs { get; set; }

        public UserAccount()
        {
            _id = 0;
            _person = new Person();
            _password = "";
            _isOnline = false;
            Dialogs = new ObservableCollection<Dialog>();
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case nameof(Password):
                        {
                            Regex regex = new Regex(@"^\w{6}");

                            if (!regex.IsMatch(Password))
                                error = "Пароль может состоять из заглавных и строчных букв, а также цифр";

                            else if (Password.Length > MaxLengthOfPassword)
                                error = "Пароль должен содержать не больше 10ти символов";

                            else if (Password.Length < MinLengthOfPassword)
                                error = "Пароль должен содержать не меньше 6ти символов";
                        }
                        break;

                    //case nameof(SecondPassword):
                    //{
                    //    Regex regex = new Regex(@"^\w{6}");

                    //    if (!regex.IsMatch(SecondPassword) || SecondPassword.Length > MaxLengthOfPassword || SecondPassword.Length < MinLengthOfPassword || String.Compare(FirstPassword, SecondPassword) != 0)
                    //        error = "Пароли не совпадают";
                    //}
                    //break;

                    default:
                        break;
                }
                return error;
            }//get
        }
    }
}
