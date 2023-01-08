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
    /// Класс, который является посредником между сетевым провайдероми и данными пользователя
    /// </summary>
    public class NetworkProviderUserDataMediator : BaseNotifyPropertyChanged, INetworkMessageHandler
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
        private User _userData;

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
        public User UserData
        {
            get => _userData;

            set
            {
                _userData = value;

                OnPropertyChanged(nameof(UserData));
            }
        }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkProviderUserDataMediator()
        {
            UserData = new User();

            ClientNetworkProvider = new ClientNetworkProvider(this);

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        #endregion Конструкторы

        #region Реализация INetworkMessageHandler

        /// <summary>
        /// Обработать сетевое сообщение
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
                        ProcessSuccessfulRegistrationNetworkMessage(message);
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
        /// Обаработать сетевое сообщение о регистрации пользователя асинхронно
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        public void ProcessSuccessfulRegistrationNetworkMessage(NetworkMessage message)
        {
            var data = message.Data;

            try
            {
                NetworkProviderDto networkProviderDto = Deserializer.Deserialize<NetworkProviderDto>(data);

                RegisterNewUser(networkProviderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Зарегистрировать нового пользователя асинхронно
        /// </summary>
        /// <param name="networkProviderDto">DTO, сетевого провайдера с Id из базы данных</param>
        /// 
        public void RegisterNewUser(NetworkProviderDto networkProviderDto)
        {
            ClientNetworkProvider.Id = networkProviderDto.Id;

            SignUp?.Invoke();
        }

        #endregion Методы управления данными пользователя

        #region Методы взаимодействия с сетью

        public async Task SendRegistrationRequestAsync(RegistrationData registrationData)
        {
            User userData = _mapper.Map<User>(registrationData);
            UserData = userData;

            RegistrationDto registrationDto = _mapper.Map<RegistrationDto>(registrationData);

            byte[] data = Serializer<RegistrationDto>.Serialize(registrationDto);

            NetworkMessage message = new NetworkMessage(data, NetworkMessage.OperationCode.RegistrationCode);

            await ClientNetworkProvider.ConnectAsync(message);
        }

        #endregion Методы взаимодействия с сетью

        public string GetRecipientName(int UserDataId, int DialogId)
        {
            return "Anna";
        }
    }
}