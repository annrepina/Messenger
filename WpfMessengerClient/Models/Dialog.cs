using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    public class Dialog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private const int NumberOfAccounts = 2;

        //private UserAccount _userAccount1;
        //private UserAccount _userAccount2;

        public int Id { get; set; }

        //public UserAccount UserAccount1
        //{
        //    get => _userAccount1;

        //    set
        //    {
        //        if(value != null)
        //        {
        //            _userAccount1 = value;
        //            OnPropertyChanged(nameof(UserAccount1));
        //        }

        //    }
        //}

        //public UserAccount UserAccount2
        //{
        //    get => _userAccount2;

        //    set
        //    {
        //        if (value != null)
        //        {
        //            _userAccount2 = value;
        //            OnPropertyChanged(nameof(UserAccount2));
        //        }
        //    }
        //}

        public UserData[] UserAccounts { get; init; }

        public ObservableCollection<Message> Messages { get; set; }

        public Dialog()
        {
            //_userAccount1 = null;
            //_userAccount2 = null;
            Id = 0;
            UserAccounts = new UserData[NumberOfAccounts];
            Messages = new ObservableCollection<Message>();
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
