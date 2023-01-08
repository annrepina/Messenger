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

        private bool _isFree;

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
        public RegistrationData RegistrationData { get; init; }

        public bool IsFree
        {
            get => _isFree;

            set
            {
                _isFree = value;

                OnPropertyChanged(nameof(IsFree));
            }
        }

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="messengerWindowsManager">Менеджер окон в приложении</param>
        public RegistrationWindowViewModel(MessengerWindowsManager messengerWindowsManager)
        {
            _networkProviderUserDataMediator = new NetworkProviderUserDataMediator();
            _networkProviderUserDataMediator.SignUp += ChangeWindowToChatWindow;

            OnSignUpCommand = new DelegateCommand(async () => await RegisterNewUserAsync());
            MessengerWindowsManager = messengerWindowsManager;
            RegistrationData = new RegistrationData(); 

            IsFree = true;
        }

        #endregion Конструкторы

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        /// <returns></returns>
        private async Task RegisterNewUserAsync()
        {
            //// если ошибок нет
            //if (String.IsNullOrEmpty(RegistrationData.Error))
            //{
                IsFree = false;

                TaskCompletionSource completionSource = new TaskCompletionSource();

                new SignUpObserver(_networkProviderUserDataMediator, completionSource);

                await _networkProviderUserDataMediator.SendRegistrationRequestAsync(RegistrationData);

                await completionSource.Task;

                IsFree = true;
            //}

            //else
            //{
            //    MessageBox.Show(RegistrationData.Error);
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
