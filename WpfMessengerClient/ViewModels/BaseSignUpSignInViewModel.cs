﻿using AutoMapper;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Mapping;
using WpfMessengerClient.NetworkServices;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Базовый класс для вьюмоделей регистрации и входа
    /// </summary>
    public class BaseSignUpSignInViewModel : BaseViewModel
    {
        /// <summary>
        /// Команда по нажатию кнопки "назад"
        /// </summary>
        public DelegateCommand BackCommand { get; init; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="windowsManager">Менеджер окон в приложении</param>
        public BaseSignUpSignInViewModel(MessengerWindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, /*ConnectionController connectionController*/IClientNetworkProvider networkProvider) 
            : base(windowsManager, networkMessageHandler, /*connectionController*/networkProvider)
        {
            BackCommand = new DelegateCommand(GoBack);
        }

        /// <summary>
        /// Вернуться назад в предыдущее окно
        /// </summary>
        private void GoBack()
        {
            SwitchToSignUpSignInWindow();
        }

        /// <summary>
        /// Переключиться на окно регистрации/входа
        /// </summary>
        private void SwitchToSignUpSignInWindow()
        {
            _messengerWindowsManager.ReturnToStartWindow();
        }
    }
}