using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfMessengerClient.Models;
using AutoMapper;

namespace WpfMessengerClient.ViewModels
{
    public class RegistrationWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private UserAccount _currentUserAccount;
        //private string _firstPassword;
        //private string _secondPassword;
        //private bool _hasCurrentUserAccountName;
        //private bool _arePasswordsCorrect;
        //private bool _canUserRegisterAccount;

        private readonly IMapper mapper;



        public UserAccount CurrentUserAccount
        {
            get => _currentUserAccount;

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));  
            }
        }

        //public bool HasCurrentUserAccountName
        //{
        //    get => _hasCurrentUserAccountName;

        //    set
        //    {
        //        _hasCurrentUserAccountName = value;

        //        //OnPropertyChanged(nameof(HasCurrentUserAccountName));

        //        CanUserRegisterAccount = ArePasswordsCorrect && HasCurrentUserAccountName;
        //    }
        //}
      
        //public string FirstPassword
        //{
        //    get => _firstPassword;

        //    set
        //    {
        //        _firstPassword = value;
        //        OnPropertyChanged(nameof(FirstPassword));

        //        //ArePasswordsCorrect = String.Compare(FirstPassword, SecondPassword) == 0 && FirstPassword.Length >= MinLengthOfPassword && FirstPassword.Length <= MaxLengthOfPassword;
        //    }
        //}

        //public IsFirstPasswordCorrect

        //public string SecondPassword
        //{
        //    get => _secondPassword;

        //    set
        //    {
        //        _secondPassword = value;
        //        OnPropertyChanged(nameof(SecondPassword));
        //        ArePasswordsCorrect = String.Compare(FirstPassword, SecondPassword) == 0 && FirstPassword.Length >= MinLengthOfPassword && FirstPassword.Length <= MaxLengthOfPassword;
        //    }
        //}

        //public bool ArePasswordsCorrect
        //{
        //    get => _arePasswordsCorrect;

        //    set
        //    {
        //        _arePasswordsCorrect = value;

        //        //OnPropertyChanged(nameof(ArePasswordsCorrect));

        //        CanUserRegisterAccount = ArePasswordsCorrect && HasCurrentUserAccountName;
        //    }
        //}

        //public bool CanUserRegisterAccount
        //{
        //    get => _canUserRegisterAccount;

        //    set
        //    {
        //        _canUserRegisterAccount = value;

        //        OnPropertyChanged(nameof(CanUserRegisterAccount));
        //    }
        //}

        public DelegateCommand OnRegisterInMessengerCommand { get; set; }

        public RegistrationWindowViewModel()
        {
            CurrentUserAccount = new UserAccount();
            OnRegisterInMessengerCommand = new DelegateCommand(OnRegisterInMessenger);
        }

        /// <summary>
        /// Метод, вызывающий событие PropertyChanged
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void OnRegisterInMessenger()
        {
            // если ошибок нет
            if(!String.IsNullOrEmpty(CurrentUserAccount.Error))
            {
                // подключаемся
            }
        }
    }
}
