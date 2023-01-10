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
using WpfMessengerClient.Models.Mapping;
using AutoMapper;
using DtoLib.Dto;
using WpfMessengerClient.Services;
using DtoLib;
using DtoLib.Serialization;
using WpfMessengerClient.Obsevers;
using System.Windows;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель для окна регистрации
    /// </summary>
    public class RegistrationWindowViewModel : BaseNotifyPropertyChanged
    {
        /// <summary>
        /// Посредник между сетевым провайдером и данными ипользователя
        /// </summary>
        private NetworkProviderUserDataMediator _networkProviderUserDataMediator;

        /// <summary>
        /// Доступна ли кнопка регистрации для нажатия
        /// </summary>
        private bool _isRegistrationButtonFree;

        /// <summary>
        /// Менеджер окон приложения
        /// </summary>
        public MessengerWindowsManager MessengerWindowsManager { get; init; }

        /// <summary>
        /// Команда по нажатию кнопки регистрации
        /// </summary>
        public DelegateCommand OnSignUpCommand { get; init; }

        /// <summary>
        /// Данные о регистрации нового пользователя
        /// </summary>
        public RegistrationRequest RegistrationRequestData { get; init; }

        /// <summary>
        /// Доступна ли кнопка регистрации для нажатия
        /// </summary>
        public bool IsRegistrationButtonFree
        {
            get => _isRegistrationButtonFree;

            set
            {
                _isRegistrationButtonFree = value;

                OnPropertyChanged(nameof(IsRegistrationButtonFree));
            }
        }

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="messengerWindowsManager">Менеджер окон в приложении</param>
        public RegistrationWindowViewModel(MessengerWindowsManager messengerWindowsManager)
        {
            _networkProviderUserDataMediator = new NetworkProviderUserDataMediator();
            _networkProviderUserDataMediator.SignUp += ChangeWindowToChatWindow;

            OnSignUpCommand = new DelegateCommand(async () => await RegisterNewUserAsync());
            MessengerWindowsManager = messengerWindowsManager;
            RegistrationRequestData = new RegistrationRequest(); 

            IsRegistrationButtonFree = true;
        }

        #endregion Конструкторы

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        /// <returns></returns>
        private async Task RegisterNewUserAsync()
        {
            //// если ошибок нет
            //if (String.IsNullOrEmpty(RegistrationRequest.Error))
            //{

            try
            {
                IsRegistrationButtonFree = false;

                TaskCompletionSource completionSource = new TaskCompletionSource();

                new SignUpObserver(_networkProviderUserDataMediator, completionSource);

                await _networkProviderUserDataMediator.SendRegistrationRequestAsync(RegistrationRequestData);

                await completionSource.Task;
            }

            finally
            {
                IsRegistrationButtonFree = true;
            }
            //}

            //else
            //{
            //    MessageBox.Show(RegistrationRequest.Error);
            //}

            ChangeWindowToChatWindow();
        }

        /// <summary>
        /// Изменить окно на окно чата
        /// </summary>
        public void ChangeWindowToChatWindow()
        {
            MessengerWindowsManager.SwitchToChatWindow(_networkProviderUserDataMediator);
        }
    }
}
