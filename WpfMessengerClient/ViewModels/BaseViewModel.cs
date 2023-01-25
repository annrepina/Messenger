using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Базовый класс для ViewModel
    /// </summary>
    public class BaseViewModel : BaseNotifyPropertyChanged
    {
        /// <inheritdoc cref="AreControlsAvailable"/>
        protected bool _areControlsAvailable;

        /// <summary>
        /// Посредник между сетевым провайдером и данными пользователя
        /// </summary>
        protected readonly NetworkMessageHandler _networkMessageHandler;

        protected readonly MessengerWindowsManager _messengerWindowsManager;

        /// <summary>
        /// Доступны ли контролы на вьюхе
        /// </summary>
        public bool AreControlsAvailable
        {
            get => _areControlsAvailable;

            set
            {
                _areControlsAvailable = value;

                OnPropertyChanged(nameof(AreControlsAvailable));
            }
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="messengerWindowsManager">Менеджер окон в приложении</param>
        public BaseViewModel(MessengerWindowsManager messengerWindowsManager, NetworkMessageHandler networkMessageHandler)
        {
            _networkMessageHandler = networkMessageHandler;

            _messengerWindowsManager = messengerWindowsManager;

            AreControlsAvailable = true;
        }
    }
}
