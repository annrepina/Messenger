using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// ViewModel для стартового окна
    /// </summary>
    public class StartWindowViewModel : BaseNotifyPropertyChanged
    {
        /// <inheritdoc cref="AreControlsAvailable"/>
        private bool _areControlsAvailable;

        /// <summary>
        /// Менеджер окон для мессенджера
        /// </summary>
        private readonly MessengerWindowsManager _messengerWindowsManager;

        /// <summary>
        /// Команда по нажатию на кнопку регистрации
        /// </summary>
        public DelegateCommand SignUpCommand { get; init; }

        /// <summary>
        /// Команда по нажатию на кнопку входа
        /// </summary>
        public DelegateCommand SignInCommand { get; init; }

        /// <summary>
        /// Доступны ли элементы управления на стратовом окне
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
        /// <param name="windowsManager">Менеджер окон для приложения</param>
        public StartWindowViewModel(MessengerWindowsManager windowsManager)
        {
            _messengerWindowsManager = windowsManager;

            SignUpCommand = new DelegateCommand(() => SwitchToWindow(_messengerWindowsManager.SwitchToSignUpWindow));
            SignInCommand = new DelegateCommand(() => SwitchToWindow(_messengerWindowsManager.SwitchToSignInWindow));

            AreControlsAvailable = true;
        }

        private void SwitchToWindow(Action switchAction)
        {
            AreControlsAvailable = false;
            switchAction.Invoke();
            AreControlsAvailable = true;
        }
    }
}