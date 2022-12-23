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
//using DtoLib;

namespace WpfMessengerClient.ViewModels
{
    public class RegistrationWindowViewModel : INotifyPropertyChanged/*, INetworkMessageHandler*/
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //private UserAccount _currentUserAccount;

        private readonly IMapper _mapper;
        private Messenger _messenger;

        //public UserAccount CurrentUserAccount
        //{
        //    get => _currentUserAccount;

        //    set
        //    {
        //        _currentUserAccount = value;
        //        OnPropertyChanged(nameof(CurrentUserAccount));  
        //    }
        //}

        public Messenger Messenger 
        { 
            get => _messenger; 
            
            set
            {
                _messenger = value;

                OnPropertyChanged(nameof(Messenger));
            }
        
        }

        //public ConnectionService /*ConnectionService { get; set; }*/ _connectionService;

        public FrontClient Client { get; set; }

        public DelegateCommand OnRegisterInMessengerCommand { get; set; }

        public RegistrationWindowViewModel()
        {
            //CurrentUserAccount = new UserAccount();
            Messenger = new Messenger();
            OnRegisterInMessengerCommand = new DelegateCommand(OnRegisterInMessenger);
            //_connectionService = new ConnectionService(this);
            // задаем клиента

            Client = new FrontClient(Messenger);
            //Client.UserAccount = Messenger.CurrentUserAccount;

            Messenger.CurrentUserAccount.AddClient(Client);

            //CurrentUserAccount.AddClient(Client);
            //CurrentUserAccount.CurrentClient = Client;

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

        private async void OnRegisterInMessenger()
        {
            // если ошибок нет
            if (String.IsNullOrEmpty(Messenger.CurrentUserAccount.Error))
            {
                UserAccountDto userAcc = _mapper.Map<UserAccountDto>(Messenger.CurrentUserAccount);

                byte[] data = new Serializator<UserAccountDto>().Serialize(userAcc);

                NetworkMessage message = new NetworkMessage(data, NetworkMessage.OperationCode.RegistrationCode);

                await Client.ConnectAsync(message);
            }
        }

        //public void ProcessNetworkMessage(NetworkMessage message)
        //{
            
        //}
    }
}
