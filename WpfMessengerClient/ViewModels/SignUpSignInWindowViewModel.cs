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
        /// <summary>
        /// Менеджер окон для мессенджера
        /// </summary>
        MessengerWindowsManager MessengerWindowsManager { get; init; }

        /// <summary>
        /// Команда по нажатию на кнопку регистрации
        /// </summary>
        public DelegateCommand OnSignUpCommand { get; set; }

        /// <summary>
        /// Команда по нажатию на кнопку входа
        /// </summary>
        public DelegateCommand OnSignInCommand { get; set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="messengerWindowsManager">Менеджер окон для приложения</param>
        public SignUpSignInWindowViewModel(MessengerWindowsManager messengerWindowsManager)
        {
            MessengerWindowsManager = messengerWindowsManager;

            OnSignUpCommand = new DelegateCommand(SwitchToSignUpWindow);
            OnSignInCommand = new DelegateCommand(SwitchToSignInWindow);
        }

        /// <summary>
        /// Переключиться на окно входа пользователя
        /// </summary>
        private void SwitchToSignInWindow()
        {
            MessengerWindowsManager.SwitchToSignInWindow();
        }

        /// <summary>
        /// Переключиться на окно с регистрацией нового пользователя
        /// </summary>
        private void SwitchToSignUpWindow()
        {
            MessengerWindowsManager.SwitchToSignUpWindow();
        }
    }
}
