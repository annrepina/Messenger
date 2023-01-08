using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Services;
using WpfMessengerClient.Models;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель окна чатов
    /// </summary>
    public class ChatWindowViewModel : BaseNotifyPropertyChanged
    {
        #region Приватные поля


        /// <summary>
        /// Посредник между сетевым провайдером и данными пользователя
        /// </summary>
        private NetworkProviderUserDataMediator _networkProviderUserDataMediator;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Менеджер окон приложения
        /// </summary>
        public MessengerWindowsManager MessengerWindowsManager { get; init; }

        /// <summary>
        /// Свойство - посредник между сетевым провайдером и данными пользователя
        /// </summary>
        public NetworkProviderUserDataMediator NetworkProviderUserDataMediator
        {
            get => _networkProviderUserDataMediator;

            set
            {
                _networkProviderUserDataMediator = value;

                OnPropertyChanged(nameof(NetworkProviderUserDataMediator));
            }
        }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="networkProviderUserDataMediator">Посредник между сетевым провайдером и данными пользователя</param>
        /// <param name="messengerWindowsManager">Менеджер окон приложения</param>
        public ChatWindowViewModel(NetworkProviderUserDataMediator networkProviderUserDataMediator, MessengerWindowsManager messengerWindowsManager)
        {
            NetworkProviderUserDataMediator = networkProviderUserDataMediator;
            MessengerWindowsManager = messengerWindowsManager;
        }

        #endregion Конструкторы
    }
}
