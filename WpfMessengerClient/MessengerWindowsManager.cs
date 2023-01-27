using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.NetworkServices;
using WpfMessengerClient.Services;
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
        public void OpenStartWindow()
        {
            StartWindowViewModel viewModel = new StartWindowViewModel(this);

            CurrentWindow = new StartWindow(viewModel);

            CurrentApplication.MainWindow = CurrentWindow;
            CurrentApplication.MainWindow.Show();
        }

        public void ReturnToStartWindow()
        {
            Window windowToShow = null;

            CurrentWindow.Hide();

            foreach(Window window in Application.Current.Windows)
            {
                if(window is StartWindow signUpSignInWindow)
                {
                    windowToShow = window;
                    break;
                }
            }

            CurrentWindow = windowToShow;
            CurrentWindow.Show();
        }

        /// <summary>
        /// Переключиться на окно регистрации
        /// </summary>
        public void SwitchToSignUpWindow()
        {
            //var builder = new NetworkMessageHandlerBuilder();
            //builder.Build();

            ConnectionController connectionController = new ConnectionController();
            NetworkMessageHandler networkMessageHandler = new NetworkMessageHandler();
            connectionController.NetworkMessageHandler = networkMessageHandler;
            networkMessageHandler.ConnectionController = connectionController;  

            SignUpWindowViewModel registrationWindowViewModel = new SignUpWindowViewModel(this, networkMessageHandler, connectionController);
            SignUpWindow registrationWindow = new SignUpWindow(registrationWindowViewModel);

            CurrentWindow.Hide();
            CurrentWindow = registrationWindow;
            CurrentWindow.Show();
        }

        /// <summary>
        /// Переключиться на окно входа
        /// </summary>
        public void SwitchToSignInWindow()
        {
            //var builder = new NetworkMessageHandlerBuilder();
            //builder.Build();
            //ConnectionController connectionController = new ConnectionController();
            NetworkMessageHandler networkMessageHandler = new NetworkMessageHandler();
            //connectionController.NetworkMessageHandler = networkMessageHandler;
            //networkMessageHandler.ConnectionController = connectionController;
            IClientNetworkProvider clientNetworkProvider = new ClientNetworkProvider();

            SignInWindowViewModel signInWindowViewModel = new SignInWindowViewModel(this, networkMessageHandler, connectionController);
            SignInWindow signInWindow = new SignInWindow(signInWindowViewModel);

            CurrentWindow.Hide();
            CurrentWindow = signInWindow;
            CurrentWindow.Show();
        }

        /// <summary>
        /// Переключиться на окно чата
        /// </summary>
        public void SwitchToChatWindow(NetworkMessageHandler networkMessageHandler, /*ConnectionController connectionController,*/IClientNetworkProvider networkProvider, User user)
        {
            ChatWindowViewModel chatWindowViewModel = new ChatWindowViewModel(this, networkMessageHandler, networkProvider, user);

            ChatWindow chatWindow = new ChatWindow(chatWindowViewModel);

            CurrentWindow.Hide();

            CurrentWindow = chatWindow;

            CurrentWindow.Show();
        }

        /// <summary>
        /// Переключиться на окно чата
        /// </summary>
        public void SwitchToChatWindow(NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider, User user, List<Dialog> dialogs)
        {
            ChatWindowViewModel chatWindowViewModel = new ChatWindowViewModel(this, networkMessageHandler, networkProvider, user, dialogs);

            ChatWindow chatWindow = new ChatWindow(chatWindowViewModel);

            CurrentWindow.Hide();

            CurrentWindow = chatWindow;

            CurrentWindow.Show();
        }

        public void CloseCurrentWindow()
        {
            CurrentWindow.Close();

            CurrentApplication.Shutdown();
        }
    }
}