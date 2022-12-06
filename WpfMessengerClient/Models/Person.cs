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

        private const int PhoneNumberLength = 12;
        private const int MaxNameLength = 50;
        private const int MinNameLength = 2;

        private string _name;
        private string? _surname;
        private string _phoneNumber;
        private string _error;

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
            _error = "";
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string Error
        {
            get
            {
                string error = String.Empty;
                //Regex regex = new Regex(@"^8\d{10}");
                Regex regex = new Regex(@"^\+7\d{10}");
                //Regex regex = new Regex(@"^\d{10}");

                if (!regex.IsMatch(PhoneNumber))
                    error = "Телефон должен начинасть с +7 и далее 10 цифр";

                else if (PhoneNumber.Length != PhoneNumberLength)
                    error = "Номер телефон должнен состоять из 12 символов";

                return error;
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
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

                    case nameof(PhoneNumber):
                    {
                        //Regex regex = new Regex(@"^8\d{10}");
                        Regex regex = new Regex(@"^\+7\d{10}");
                            //Regex regex = new Regex(@"^\d{10}");

                        if (!regex.IsMatch(PhoneNumber))
                            error = "Телефон должен начинасть с +7 и далее 10 цифр";

                        else if (PhoneNumber.Length != PhoneNumberLength)
                            error = "Номер телефон должнен состоять из 12 символов";
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
