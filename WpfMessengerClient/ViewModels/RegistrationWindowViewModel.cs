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
using WpfMessengerClient.Models.Requests;

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
        private NetworkMessageHandler _networkProviderUserDataMediator;

        /// <summary>
        /// Доступна ли кнопка регистрации для нажатия
        /// </summary>
        private bool _isRegistrationButtonFree;

        /// <summary>
        /// Маппер для мапинга моделей на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

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
        /// Доступны ли контролы на вьюхе
        /// </summary>
        public bool IsControlsFree
        {
            get => _isRegistrationButtonFree;

            set
            {
                _isRegistrationButtonFree = value;

                OnPropertyChanged(nameof(IsControlsFree));
            }
        }

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="messengerWindowsManager">Менеджер окон в приложении</param>
        public RegistrationWindowViewModel(MessengerWindowsManager messengerWindowsManager)
        {
            _networkProviderUserDataMediator = new NetworkMessageHandler();
            //_networkProviderUserDataMediator.SignUp += ChangeWindowToChatWindow;

            OnSignUpCommand = new DelegateCommand(async () => await RegisterNewUserAsync());
            MessengerWindowsManager = messengerWindowsManager;
            RegistrationRequestData = new RegistrationRequest(); 

            IsControlsFree = true;

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
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

            IsControlsFree = false;

            TaskCompletionSource completionSource = new TaskCompletionSource();

            var observer = new SignUpObserver(_networkProviderUserDataMediator, completionSource);

            _networkProviderUserDataMediator.SendRegistrationRequestAsync(RegistrationRequestData);

            await completionSource.Task;

            User user = _mapper.Map<User>(RegistrationRequestData);
            user.Id = observer.UserId;

            IsControlsFree = true;

            ChangeWindowToChatWindow(user);

            //}

            //else
            //{
            //    MessageBox.Show(RegistrationRequest.Error);
            //}


        }

        /// <summary>
        /// Изменить окно на окно чата
        /// </summary>
        public void ChangeWindowToChatWindow(User user)
        {
            MessengerWindowsManager.SwitchToChatWindow(_networkProviderUserDataMediator, user);
        }
    }
}
