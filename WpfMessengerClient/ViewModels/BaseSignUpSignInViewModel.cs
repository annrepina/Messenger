using AutoMapper;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Mapping;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Базовый класс для вьюмоделей регистрации и входа
    /// </summary>
    public class BaseSignUpSignInViewModel : BaseNotifyPropertyChanged
    {
        /// <inheritdoc cref="IsControlsAvailable"/>
        protected bool _isControlsAvailable;

        /// <summary>
        /// Обработчик сетевых сообщений
        /// </summary>
        protected NetworkMessageHandler _networkMessageHandler;

        /// <summary>
        /// Менеджер окон приложения
        /// </summary>
        protected MessengerWindowsManager _messengerWindowsManager;

        /// <summary>
        /// Маппер для мапинга моделей на DTO и обратно
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Команда по нажатию кнопки "назад"
        /// </summary>
        public DelegateCommand BackCommand { get; init; }

        /// <summary>
        /// Доступны ли контролы на вьюхе
        /// </summary>
        public bool IsControlsAvailable
        {
            get => _isControlsAvailable;

            set
            {
                _isControlsAvailable = value;

                OnPropertyChanged(nameof(IsControlsAvailable));
            }
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="messengerWindowsManager">Менеджер окон в приложении</param>
        public BaseSignUpSignInViewModel(MessengerWindowsManager messengerWindowsManager, NetworkMessageHandler networkMessageHandler)
        {
            _networkMessageHandler = networkMessageHandler;

            BackCommand = new DelegateCommand(GoBack);
            _messengerWindowsManager = messengerWindowsManager;

            IsControlsAvailable = true;

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
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
            _messengerWindowsManager.SwitchToSignUpSignInWindow();
        }
    }
}
