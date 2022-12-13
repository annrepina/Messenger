using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    public class DialogModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private UserAccountModel _userAccount1;
        private UserAccountModel _userAccount2;

        public int Id { get; set; }

        public UserAccountModel UserAccount1
        {
            get => _userAccount1;

            set
            {
                if(value != null)
                {
                    _userAccount1 = value;
                    OnPropertyChanged(nameof(UserAccount1));
                }

            }
        }

        public UserAccountModel UserAccount2
        {
            get => _userAccount2;

            set
            {
                if (value != null)
                {
                    _userAccount2 = value;
                    OnPropertyChanged(nameof(UserAccount2));
                }
            }
        }

        public ObservableCollection<MessageModel> Messages { get; set; }

        public DialogModel()
        {
            _userAccount1 = null;
            _userAccount2 = null;
            Messages = new ObservableCollection<MessageModel>();
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
