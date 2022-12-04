using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    public class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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
                if(!string.IsNullOrEmpty(value))
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
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
    }
}
