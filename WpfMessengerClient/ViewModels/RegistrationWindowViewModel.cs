using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfMessengerClient.Models;

namespace WpfMessengerClient.ViewModels
{
    public class RegistrationWindowViewModel : INotifyPropertyChanged
    {
        private UserAccount _currentUserAccount;

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

        public DelegateCommand OnRegisterInMessengerCommand { get; set; }

        public DelegateCommand OnPhoneNumberInputTextBoxBackspaceDownCommand { get; set; }

        public RegistrationWindowViewModel()
        {
            CurrentUserAccount = new UserAccount();
            OnRegisterInMessengerCommand = new DelegateCommand(OnRegisterInMessenger);
            OnPhoneNumberInputTextBoxBackspaceDownCommand = new DelegateCommand(OnPhoneNumberInputTextBoxBackspaceDown);
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
    }
}
