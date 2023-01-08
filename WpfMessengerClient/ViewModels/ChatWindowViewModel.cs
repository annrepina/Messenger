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
    public class ChatWindowViewModel : BaseNotifyPropertyChanged
    {
        private NetworkProviderUserDataMediator _networkProviderUserDataMediator;

        /// <summary>
        /// Менеджер окон приложения
        /// </summary>
        public MessengerWindowsManager MessengerWindowsManager { get; init; }

        public NetworkProviderUserDataMediator NetworkProviderUserDataMediator
        {
            get => _networkProviderUserDataMediator;

            set
            {
                _networkProviderUserDataMediator = value;

                OnPropertyChanged(nameof(NetworkProviderUserDataMediator));
            }

        }


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
    }
}
