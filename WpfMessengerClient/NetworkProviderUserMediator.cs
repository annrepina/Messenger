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
    /// <summary>
    /// Класс, который является посредником между сетевым провайдероми и управлением данными пользователя
    /// </summary>
    public class NetworkProviderUserMediator : BaseNotifyPropertyChanged, INetworkMessageHandler
    {

        #region События

        /// <summary>
        /// Событие регистрации пользователя
        /// </summary>
        public event Action SignUp;

        /// <summary>
        /// Событие входа пользователя
        /// </summary>
        public event Action SignIn;

        #endregion События

        #region Приватные поля

        /// <summary>
        /// Данные о пользователе
        /// </summary>
        private UserData _userData;

        /// <summary>
        /// Маппер для мапинга моделей на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

        #endregion Приватные поля

        #region  Свойства

        /// <summary>
        /// Сетевой провайдер на стороне клиента
        /// </summary>
        public ClientNetworkProvider ClientNetworkProvider { get; set; }

        /// <summary>
        /// Данные пользователя
        /// </summary>
        public UserData UserData
        {
            get => _userData;

            set
            {
                _userData = value;
                OnPropertyChanged(nameof(UserData));
            }
        }

        #endregion Свойства

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkProviderUserMediator()
        {
            UserData = new UserData();

            ClientNetworkProvider = new ClientNetworkProvider();
            ClientNetworkProvider.NetworkMessageHandler = this;

            UserData.AddClient(ClientNetworkProvider);

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        #region Реализация INetworkMessageHandler

        /// <summary>
        /// Обработать сетвое сообщение
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        public void ProcessNetworkMessage(NetworkMessage message)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    break;
                case NetworkMessage.OperationCode.SuccessfulRegistrationCode:
                    {
                        ProcessRegistrationNetworkMessage(message);
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

        #endregion Реализация INetworkMessageHandler

        #region Методы управления данными пользователя

        /// <summary>
        /// Обаработать сетевое сообщение о регистрации пользователя
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        public void ProcessRegistrationNetworkMessage(NetworkMessage message)
        {
            var data = message.Data;

            SuccessfulRegistrationDto successfulRegistrationDto = Deserializer.Deserialize<SuccessfulRegistrationDto>(data);

            RegisterNewUser(successfulRegistrationDto);
        }

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        /// <param name="successfulRegistrationDto">DTO, которое представляет данные о зарегистрированном пользователе</param>
        public void RegisterNewUser(SuccessfulRegistrationDto successfulRegistrationDto)
        {
            UserData = _mapper.Map<UserData>(successfulRegistrationDto);

            ClientNetworkProvider.Id = successfulRegistrationDto.ClientId;

            SignUp?.Invoke();
        }

        #endregion Методы управления данными пользователя

        #region Методы взаимодействия с сетью

        public void SendRegistrationRequest(string phoneNumber, string password)
        {
            RegistrationAuthentificationDto userAccountRegistrationDto = new RegistrationAuthentificationDto() { PhoneNumber = phoneNumber, Password = password};

            byte[] data = Serializer<RegistrationAuthentificationDto>.Serialize(userAccountRegistrationDto);

            NetworkMessage message = new NetworkMessage(data, NetworkMessage.OperationCode.RegistrationCode);

            Task.Run(() => ClientNetworkProvider.ConnectAsync(message));
        }

        #endregion Методы взаимодействия с сетью
    }
}