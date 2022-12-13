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
        //private UserAccount _receivingUserAccount;
        private Dialog _dialog;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id { get; set; }

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

        //public UserAccount ReceivingUserAccount
        //{
        //    get => _receivingUserAccount;

        //    set
        //    {
        //        _receivingUserAccount = value;

        //        OnPropertyChanged(nameof(ReceivingUserAccount));
        //    }
        //}

        public Dialog Dialog 
        {
            get => _dialog;

            set
            {
                _dialog = value;

                OnPropertyChanged(nameof(Dialog));
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
            Id = 0;
            _text = "";
            _sendingUserAccount = null;
            //_receivingUserAccount = null;
            _isRead = false;
            _dateTime = null;
            _dialog = null;
        }

        public Message(string text, UserAccount sendingUserAccount, UserAccount receivingUserAccount, Dialog dialog)
        {
            Id = 0;
            _text = text;
            _sendingUserAccount = sendingUserAccount;
            //_receivingUserAccount = receivingUserAccount;
            _isRead = false;
            _dateTime = null;
            _dialog = dialog;
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
