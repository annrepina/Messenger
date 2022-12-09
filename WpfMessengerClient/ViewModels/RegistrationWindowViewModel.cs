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
//using DtoLib;

namespace WpfMessengerClient.ViewModels
{
    public class RegistrationWindowViewModel : INotifyPropertyChanged, IViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private UserAccount _currentUserAccount;

        private readonly IMapper _mapper;



        public UserAccount CurrentUserAccount
        {
            get => _currentUserAccount;

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));  
            }
        }

        public ConnectionService /*ConnectionService { get; set; }*/ _connectionService;

        public DelegateCommand OnRegisterInMessengerCommand { get; set; }

        public RegistrationWindowViewModel()
        {
            CurrentUserAccount = new UserAccount();
            OnRegisterInMessengerCommand = new DelegateCommand(OnRegisterInMessenger);
            _connectionService = new ConnectionService();

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        /// <summary>
        /// Метод, вызывающий событие PropertyChanged
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void OnRegisterInMessenger()
        {
            // если ошибок нет
            if (!String.IsNullOrEmpty(CurrentUserAccount.Error))
            {
                UserAccountDto userAcc = _mapper.Map<UserAccountDto>(CurrentUserAccount);

                NetworkMessage message = new NetworkMessage(userAcc, NetworkMessage.OperationCode.RegistrationCode);

                _connectionService.Connect(message);



                //MessengerMapper map = MessengerMapper.GetInstance();

                //var mapper = map.CreateIMapper();

                //UserAccountDto user = mapper.Map<UserAccountDto>(CurrentUserAccount);

                //var a = user;

            // подключаемся
            }
        }

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            
        }
    }
}
