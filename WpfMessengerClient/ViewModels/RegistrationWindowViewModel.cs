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

namespace WpfMessengerClient.ViewModels
{
    public class RegistrationWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private const int MaxLengthOfPassword = 10;

        private UserAccount _currentUserAccount;
        private string _firstPassword;
        private string _secondPassword;

        public event PropertyChangedEventHandler? PropertyChanged;

        public UserAccount CurrentUserAccount
        {
            get => _currentUserAccount;

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));  
            }
        }

        public string FirstPassword
        {
            get => _firstPassword;

            set
            {
                _firstPassword = value;
                OnPropertyChanged(nameof(FirstPassword));
            }
        }

        public string SecondPassword
        {
            get => _secondPassword;

            set
            {
                _secondPassword = value;
                OnPropertyChanged(nameof(SecondPassword));
            }
        }

        public DelegateCommand OnRegisterInMessengerCommand { get; set; }

        public DelegateCommand OnPhoneNumberInputTextBoxBackspaceDownCommand { get; set; }



        public RegistrationWindowViewModel()
        {
            CurrentUserAccount = new UserAccount();
            OnRegisterInMessengerCommand = new DelegateCommand(OnRegisterInMessenger);
            OnPhoneNumberInputTextBoxBackspaceDownCommand = new DelegateCommand(OnPhoneNumberInputTextBoxBackspaceDown);
            FirstPassword = "";
            SecondPassword = "";
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

        }

        private void OnPhoneNumberInputTextBoxBackspaceDown()
        {
            if (CurrentUserAccount.Person.PhoneNumber.Length < 2)
            {
                return;
            }
            else
            {
                CurrentUserAccount.Person.PhoneNumber = CurrentUserAccount.Person.PhoneNumber.Remove(CurrentUserAccount.Person.PhoneNumber.Length - 1);
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case nameof(FirstPassword):
                        {
                            Regex regex = new Regex(@"^\w{10}");

                            if (!regex.IsMatch(FirstPassword) || FirstPassword.Length > MaxLengthOfPassword)
                                error = "Недопустимые символы или пароль длиннее 10 символов";
                        }
                        break;

                    case nameof(SecondPassword):
                        {
                            Regex regex = new Regex(@"^\w{10}");

                            if (!regex.IsMatch(SecondPassword) || SecondPassword.Length > MaxLengthOfPassword || String.Compare(FirstPassword, SecondPassword) != 0)
                                error = "Пароли не совпадают";
                    }
                    break;

                    default:
                        break;
                }
                return error;
            }
        }
    }
}
