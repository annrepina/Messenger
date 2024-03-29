﻿using System;
using System.Collections.Generic;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.NetworkMessageProcessing;
using WpfMessengerClient.NetworkServices.Interfaces;
using WpfMessengerClient.Services;
using WpfMessengerClient.ViewModels;
using WpfMessengerClient.Windows;

namespace WpfMessengerClient
{
    /// <summary>
    /// Класс, который управляет окнами в текущем приложении
    /// </summary>
    public class WindowsManager
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
        public WindowsManager(Application currentApplication)
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
            NetworkMessageHandler networkMessageHandler = new NetworkMessageHandler();
            IClientNetworkProvider clientNetworkProvider = new ClientNetworkProvider(networkMessageHandler);

            SignUpWindowViewModel signUpWindowViewModel = new SignUpWindowViewModel(this, networkMessageHandler, clientNetworkProvider);

            SwitchToWindow<SignUpWindowViewModel, SignUpWindow>(signUpWindowViewModel);

            if (_currentWindow is StartWindow)
            {
                SignUpWindow signUpWindow = new SignUpWindow(signUpWindowViewModel);
                ChangeWindow(signUpWindow);
            }
        }

        /// <summary>
        /// Переключиться на окно входа
        /// </summary>
        public void SwitchToSignInWindow()
        {
            NetworkMessageHandler networkMessageHandler = new NetworkMessageHandler();
            IClientNetworkProvider clientNetworkProvider = new ClientNetworkProvider(networkMessageHandler);

            SignInWindowViewModel signInWindowViewModel = new SignInWindowViewModel(this, networkMessageHandler, clientNetworkProvider);

            SwitchToWindow<SignInWindowViewModel, SignInWindow>(signInWindowViewModel);

            if (_currentWindow is StartWindow)
            {
                SignInWindow signInWindow = new SignInWindow(signInWindowViewModel);

                ChangeWindow(signInWindow);
            }
        }

        /// <summary>
        /// Переключиться на окно чата
        /// </summary>
        public void SwitchToChatWindow(NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider, User user)
        {
            ChatWindowViewModel chatWindowViewModel = new ChatWindowViewModel(this, networkMessageHandler, networkProvider, user);

            SwitchToChatWindow(chatWindowViewModel);
        }

        /// <summary>
        /// Перегрузка метода - переключиться на окно чата
        /// </summary>
        public void SwitchToChatWindow(NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider, User user, List<Dialog> dialogs)
        {
            ChatWindowViewModel chatWindowViewModel = new ChatWindowViewModel(this, networkMessageHandler, networkProvider, user, dialogs);

            SwitchToChatWindow(chatWindowViewModel);
        }

        /// <summary>
        /// Переключается на окно чатов
        /// </summary>
        /// <param name="chatWindowViewModel">ViewModel</param>
        private void SwitchToChatWindow(ChatWindowViewModel chatWindowViewModel)
        {
            SwitchToWindow<ChatWindowViewModel, ChatWindow>(chatWindowViewModel);

            if (_currentWindow is SignInWindow || _currentWindow is SignUpWindow)
            {
                ChatWindow chatWindow = new ChatWindow(chatWindowViewModel);
                ChangeWindow(chatWindow);
            }
        }

        /// <summary>
        /// Переключается на заданное окно
        /// </summary>
        /// <typeparam name="TViewModel">Тип объекта, представляющего ViewModel</typeparam>
        /// <typeparam name="TWindow">Тип окна</typeparam>
        /// <param name="viewModel">ViewModel</param>
        private void SwitchToWindow<TViewModel, TWindow>(TViewModel viewModel)
            where TWindow : Window, new()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is TWindow)
                {
                    window.DataContext = viewModel;
                    ChangeWindow(window);
                    return;
                }
            }
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