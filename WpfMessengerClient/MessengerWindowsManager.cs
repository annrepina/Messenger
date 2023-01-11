using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.ViewModels;
using WpfMessengerClient.Windows;

namespace WpfMessengerClient
{
    /// <summary>
    /// Класс, который управляет окнами wpf в приложении
    /// </summary>
    public class MessengerWindowsManager
    {
        /// <summary>
        /// Текущее окно
        /// </summary>
        public static Window CurrentWindow { get; set; }

        /// <summary>
        /// Текущее приложение окнами которого управляет менеджер
        /// </summary>
        public Application CurrentApplication { get; set; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="currentApplication"></param>
        public MessengerWindowsManager(Application currentApplication)
        {
            CurrentWindow = null;
            CurrentApplication = currentApplication;
        }

        /// <summary>
        /// Открывает окно регистрации/входа
        /// </summary>
        public void OpenSignUpSignInWindow()
        {
            SignUpSignInWindowViewModel viewModel = new SignUpSignInWindowViewModel(this);

            CurrentWindow = new SignUpSignInWindow(viewModel);

            CurrentApplication.MainWindow = CurrentWindow;
            CurrentApplication.MainWindow.Show();
        }

        /// <summary>
        /// Переключиться на окно регистрации
        /// </summary>
        public void SwitchToSignUpWindow()
        {
            RegistrationWindowViewModel registrationWindowViewModel = new RegistrationWindowViewModel(this);

            RegistrationWindow registrationWindow = new RegistrationWindow(registrationWindowViewModel);

            CurrentWindow.Hide();

            CurrentWindow = registrationWindow;

            CurrentWindow.Show();
        }

        /// <summary>
        /// Переключиться на окно входа
        /// </summary>
        public void SwitchToSignInWindow()
        {

        }

        /// <summary>
        /// Переключиться на окно чата
        /// </summary>
        public void SwitchToChatWindow(NetworkMessageHandler networkProviderUserDataMediator, User user)
        {
            ChatWindowViewModel chatWindowViewModel = new ChatWindowViewModel(networkProviderUserDataMediator, this, user);

            ChatWindow chatWindow = new ChatWindow(chatWindowViewModel);

            CurrentWindow.Hide();

            CurrentWindow = chatWindow;

            CurrentWindow.Show();
        }
    }
}
