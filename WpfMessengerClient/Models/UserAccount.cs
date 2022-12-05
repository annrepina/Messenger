using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    public class UserAccount : INotifyPropertyChanged
    {
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
    }
}
