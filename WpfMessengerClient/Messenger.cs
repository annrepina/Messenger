using AutoMapper;
using DtoLib;
using DtoLib.Dto;
using DtoLib.NetworkInterfaces;
using DtoLib.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Mapping;
using WpfMessengerClient.Services;

namespace WpfMessengerClient
{
    public class Messenger : INotifyPropertyChanged, INetworkMessageHandler
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public delegate void StateChanged();

        public event StateChanged OnRegistered;
        public event StateChanged OnSignIn;

        private UserAccount _currentUserAccount;

        private readonly IMapper _mapper;

        public FrontClient Client { get; set; }

        public UserAccount CurrentUserAccount
        {
            get => _currentUserAccount;

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        //public FrontClient FrontClient { get; private set; }

        public Messenger()
        {
            CurrentUserAccount = new UserAccount();

            Client = new FrontClient();
            Client.NetworkMessageHandler = this;

            CurrentUserAccount.AddClient(Client);

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

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    break;
                case NetworkMessage.OperationCode.SuccessfulRegistrationCode:
                    {
                        RegisterNewAccount(message);



                    }
                    break;

                case NetworkMessage.OperationCode.RegistrationFailedCode:
                    break;

                case NetworkMessage.OperationCode.AuthorizationCode:
                    break;
                case NetworkMessage.OperationCode.SendingMessageCode:
                    break;
                case NetworkMessage.OperationCode.ExitCode:
                    break;
                default:
                    break;
            }
        }

        public void RegisterNewAccount(NetworkMessage message)
        {
            var data = message.Data;

            Deserializer<SuccessfulRegistrationDto> deserializer = new Deserializer<SuccessfulRegistrationDto>();

            SuccessfulRegistrationDto acc = deserializer.Deserialize(data);

            UserAccount usacc = _mapper.Map<UserAccount>(acc);

            CurrentUserAccount = usacc;
        }

        public async Task SendRegistrationRequest(string phoneNumber, string password)
        {
            RegistrationAuthentificationDto userAccountRegistrationDto = new RegistrationAuthentificationDto() { PhoneNumber = phoneNumber, Password = password};

            byte[] data = new Serializator<RegistrationAuthentificationDto>().Serialize(userAccountRegistrationDto);

            NetworkMessage message = new NetworkMessage(data, NetworkMessage.OperationCode.RegistrationCode);

            await Client.ConnectAsync(message);
        }
    }
}
