using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto
{
    public class MessageDto : IDataTransferObject
    {
        public string Text { get; set; }
        //{
        //    get => _text;

        //    set
        //    {
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            _text = value;
        //            OnPropertyChanged(nameof(Text));
        //        }
        //    }
        //}

        public UserAccountDto SendingUserAccount { get; set; }
        //{
        //    get => _sendingUserAccountId;

        //    set
        //    {
        //        _sendingUserAccountId = value;

        //        OnPropertyChanged(nameof(SendingUserAccountId));
        //    }
        //}

        public UserAccountDto ReceivingUserAccount { get; set; }
        //{
        //    get => _receivingUserAccountId;

        //    set
        //    {
        //        _receivingUserAccountId = value;

        //        OnPropertyChanged(nameof(ReceivingUserAccountId));
        //    }
        //}

        public bool IsRead { get; set; }
        //{
        //    get => _isRead;

        //    set
        //    {
        //        _isRead = value;
        //        OnPropertyChanged(nameof(IsRead));
        //    }
        //}

        public DateTime? DateTime { get; set; }
        //{
        //    get => _dateTime;

        //    set
        //    {
        //        _dateTime = value;
        //        OnPropertyChanged(nameof(DateTime));
        //    }
        //}
    }
}
