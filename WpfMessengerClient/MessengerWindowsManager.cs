using System.Collections.Generic;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.NetworkServices;
using WpfMessengerClient.Services;
using WpfMessengerClient.ViewModels;
using WpfMessengerClient.Windows;

namespace WpfMessengerClient
{
    /// <summary>
    /// Класс, который управляет окнами в текущем приложении
    /// </summary>
    public class MessengerWindowsManager
    {
        /// <summary>
        /// Текущее окно приложения
        /// </summary>
        private static Window _currentWindow;

        /// <summary>
        /// Текущее приложение окнами которого управляет менеджер
        /// </summary>
        private Application _currentApplication;

        /// <summary>
        /// Конструктор с параметром - приложением
        /// </summary>
        /// <param name="currentApplication">Текущее приложение</param>
        public MessengerWindowsManager(Application currentApplication)
        {
            _currentApplication = currentApplication;
            _currentApplication.MainWindow = _currentWindow;
        }

        /// <summary>
        /// Открывает начальное окно приложения
        /// </summary>
        public void OpenStartWindow()
        {
            StartWindowViewModel viewModel = new StartWindowViewModel(this);
            _currentWindow = new StartWindow(viewModel);
            _currentWindow.Show();
        }

        /// <summary>
        /// Вернуться в начальное окно
        /// </summary>
        public void ReturnToStartWindow()
        {
            _currentWindow.Hide();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is StartWindow)
                {
                    _currentWindow = window;
                    break;
                }
            }

            _currentWindow.Show();
        }

        /// <summary>
        /// Переключиться на окно регистрации
        /// </summary>
        public void SwitchToSignUpWindow()
        {
            SignUpWindowViewModel signUpWindowViewModel = new SignUpWindowViewModel(this, new NetworkMessageHandler(), new ClientNetworkProvider());
            SignUpWindow signUpWindow = new SignUpWindow(signUpWindowViewModel);

            ChangeWindow(signUpWindow);
        }

        /// <summary>
        /// Переключиться на окно входа
        /// </summary>
        public void SwitchToSignInWindow()
        {
            SignInWindowViewModel signInWindowViewModel = new SignInWindowViewModel(this, new NetworkMessageHandler(), new ClientNetworkProvider());
            SignInWindow signInWindow = new SignInWindow(signInWindowViewModel);

            ChangeWindow(signInWindow);
        }

        /// <summary>
        /// Переключиться на окно чата
        /// </summary>
        public void SwitchToChatWindow(NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider, User user)
        {
            ChatWindowViewModel chatWindowViewModel = new ChatWindowViewModel(this, networkMessageHandler, networkProvider, user);
            ChatWindow chatWindow = new ChatWindow(chatWindowViewModel);

            ChangeWindow(chatWindow);
        }

        /// <summary>
        /// Перегрузка метода - переключиться на окно чата
        /// </summary>
        public void SwitchToChatWindow(NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider, User user, List<Dialog> dialogs)
        {
            ChatWindowViewModel chatWindowViewModel = new ChatWindowViewModel(this, networkMessageHandler, networkProvider, user, dialogs);
            ChatWindow chatWindow = new ChatWindow(chatWindowViewModel);

            ChangeWindow(chatWindow);
        }

        /// <summary>
        /// Закрыть текущее окно
        /// </summary>
        public void CloseCurrentWindow()
        {
            _currentApplication.Shutdown();

            _currentWindow.Close();
        }

        /// <summary>
        /// Изменить текущее окно
        /// </summary>
        /// <param name="window">Окно</param>
        private void ChangeWindow(Window window)
        {
            _currentWindow.Hide();
            _currentWindow = window;
            _currentWindow.Show();
        }
    }
}