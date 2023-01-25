using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель для окна регистрации/входа
    /// </summary>
    public class SignUpSignInWindowViewModel : BaseNotifyPropertyChanged
    {
        /// <inheritdoc cref="AreControlsAvailable"/>
        protected bool _areControlsAvailable;

        /// <summary>
        /// Менеджер окон для мессенджера
        /// </summary>
        public MessengerWindowsManager MessengerWindowsManager { get; init; }

        /// <summary>
        /// Команда по нажатию на кнопку регистрации
        /// </summary>
        public DelegateCommand SignUpCommand { get; init; }

        /// <summary>
        /// Команда по нажатию на кнопку входа
        /// </summary>
        public DelegateCommand SignInCommand { get; init; }

        /// <summary>
        /// Доступны ли контролы на вьюхе
        /// </summary>
        public bool AreControlsAvailable
        {
            get => _areControlsAvailable;

            set
            {
                _areControlsAvailable = value;

                OnPropertyChanged(nameof(AreControlsAvailable));
            }
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="messengerWindowsManager">Менеджер окон для приложения</param>
        public SignUpSignInWindowViewModel(MessengerWindowsManager messengerWindowsManager)
        {
            MessengerWindowsManager = messengerWindowsManager;

            SignUpCommand = new DelegateCommand(SwitchToSignUpWindow);
            SignInCommand = new DelegateCommand(SwitchToSignInWindow);

            AreControlsAvailable = true;
        }

        /// <summary>
        /// Переключиться на окно входа пользователя
        /// </summary>
        private void SwitchToSignInWindow()
        {
            AreControlsAvailable = false;
            MessengerWindowsManager.SwitchToSignInWindow();
            AreControlsAvailable = true;
        }

        /// <summary>
        /// Переключиться на окно с регистрацией нового пользователя
        /// </summary>
        private void SwitchToSignUpWindow()
        {
            AreControlsAvailable = false;
            MessengerWindowsManager.SwitchToSignUpWindow();
            AreControlsAvailable = true;
        }
    }
}