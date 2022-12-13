﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    public class MessageModel : INotifyPropertyChanged
    {
        private string _text;
        //private int _sendingUserAccountId;
        //private int _receivingUserAccountId;
        private bool _isRead;
        private DateTime? _dateTime;
        private UserAccountModel _sendingUserAccount;
        private UserAccountModel _receivingUserAccount;

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

        public UserAccountModel SendingUserAccount
        {
            get => _sendingUserAccount;

            set
            {
                _sendingUserAccount = value;

                OnPropertyChanged(nameof(SendingUserAccount));
            }
        }

        public UserAccountModel ReceivingUserAccount
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

        public MessageModel()
        {
            Id = 0;
            _text = "";
            _sendingUserAccount = null;
            _receivingUserAccount = null;
            _isRead = false;
            _dateTime = null;
        }

        public MessageModel(string text, UserAccountModel sendingUserAccount, UserAccountModel receivingUserAccount)
        {
            Id = 0;
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