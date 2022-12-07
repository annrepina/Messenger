using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System;

namespace DtoLib.Dto
{
    public class UserAccountDto : IDataTransferObject
    {
        //private int _id;
        //private Person _person;
        //private string _password;
        //private bool _isOnline;
        //private string _error;

        public int Id { get; set; }
        //{
        //    get => _id;

        //    set
        //    {
        //        _id = value;
        //        OnPropertyChanged(nameof(Id));
        //    }
        //}

        public PersonDto Person { get; set; }
        //{
        //    get => _person;

        //    set
        //    {
        //        if (value != null)
        //        {
        //            _person = value;

        //            OnPropertyChanged(nameof(Person));
        //        }
        //    }
        //}

        public string Password { get; set; }
        //{
        //    get => _password;

        //    set
        //    {
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            _password = value;

        //            OnPropertyChanged(nameof(Password));
        //        }
        //    }
        //}

        public bool IsOnline { get; set; }
        //{
        //    get => _isOnline;

        //    set
        //    {
        //        _isOnline = value;

        //        OnPropertyChanged(nameof(IsOnline));
        //    }
        //}

        public ObservableCollection<DialogDto> Dialogs { get; set; }
    }
}