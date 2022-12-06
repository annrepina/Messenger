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
    public class Person : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private const int MaxPhoneNumberLength = 12;

        private string _name;
        private string? _surname;
        private string _phoneNumber;

        public string Name 
        {
            get => _name; 

            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string? Surname 
        { 
            get => _surname; 
            
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }             
        }

        public string PhoneNumber 
        { 
            get => _phoneNumber; 
            
            set
            {
                _phoneNumber = value;

                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public Person()
        {
            _name = "";
            _surname = "";
            _phoneNumber = "";
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
                    case nameof(PhoneNumber):
                    {
                        //Regex regex = new Regex(@"^8\d{10}");
                        Regex regex = new Regex(@"^\+7\d{10}");
                        //Regex regex = new Regex(@"^\d{10}");                

                        if(!regex.IsMatch(PhoneNumber) || PhoneNumber.Length > MaxPhoneNumberLength)
                            error = "Недопустимые символы или слишком длинный номер телефона";
                    }
                    break;

                    default:
                        break;
                }
                return error;
            }
        }
    }
}
