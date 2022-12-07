using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    public class Message : INotifyPropertyChanged
    {
        private string _text;
        //private int _sendingUserAccountId;
        //private int _receivingUserAccountId;
        private bool _isRead;
        private DateTime? _dateTime;
        private UserAccount _sendingUserAccount;
        private UserAccount _receivingUserAccount;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Text
        {
            get => _text;

            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        //public int SendingUserAccountId
        //{
        //    get => _sendingUserAccountId;

        //    set
        //    {
        //        _sendingUserAccountId = value;  

        //        OnPropertyChanged(nameof(SendingUserAccountId));
        //    }
        //}

        //public int ReceivingUserAccountId
        //{
        //    get => _receivingUserAccountId;

        //    set
        //    {
        //        _receivingUserAccountId = value;

        //        OnPropertyChanged(nameof(ReceivingUserAccountId));
        //    }
        //}

        public UserAccount SendingUserAccount
        {
            get => _sendingUserAccount;

            set
            {
                _sendingUserAccount = value;

                OnPropertyChanged(nameof(SendingUserAccount));
            }
        }

        public UserAccount ReceivingUserAccount
        {
            get => _receivingUserAccount;

            set
            {
                _receivingUserAccount = value;

                OnPropertyChanged(nameof(ReceivingUserAccount));
            }
        }

        public bool IsRead
        {
            get => _isRead;

            set
            {
                _isRead = value;
                OnPropertyChanged(nameof(IsRead));
            }
        }

        public DateTime? DateTime
        {
            get => _dateTime;

            set
            {
                _dateTime = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        public Message()
        {
            _text = "";
            _sendingUserAccount = null;
            _receivingUserAccount = null;
            _isRead = false;
            _dateTime = null;
        }

        public Message(string text, UserAccount sendingUserAccount, UserAccount receivingUserAccount)
        {
            _text = text;
            _sendingUserAccount = sendingUserAccount;
            _receivingUserAccount = receivingUserAccount;
            _isRead = false;
            _dateTime = null;

        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
