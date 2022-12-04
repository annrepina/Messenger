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
        private int _sendingUserAccountId;
        private int _receivingUserAccountId;
        private bool _isRead;
        private DateTime? _dateTime;

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

        public int SendingUserAccountId
        {
            get => _sendingUserAccountId;

            set
            {
                _sendingUserAccountId = value;  

                OnPropertyChanged(nameof(SendingUserAccountId));
            }
        }

        public int ReceivingUserAccountId
        {
            get => _receivingUserAccountId;

            set
            {
                _receivingUserAccountId = value;

                OnPropertyChanged(nameof(ReceivingUserAccountId));
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
            _sendingUserAccountId = 0;
            _receivingUserAccountId = 0;
            _isRead = false;
            _dateTime = null;
    }
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
