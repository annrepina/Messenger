using AutoMapper;
using DtoLib.Dto;
using DtoLib.NetworkInterfaces;
using DtoLib.NetworkServices;
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

        public event Action SignOut;

        /// <summary>
        /// Событие получение успешного результата поиска пользователя
        /// </summary>
        public event Action<UserSearchResult> SearchResultReceived;

        #endregion События

        #region Приватные поля

        /// <summary>
        /// Пользователь
        /// </summary>
        private User _user;

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
        /// Пользователь
        /// </summary>
        public User User
        {
            get => _user;

            set
            {
                _user = value;

                OnPropertyChanged(nameof(User));
            }
        }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkProviderUserDataMediator()
        {
            User = new User();

            ClientNetworkProvider = new ClientNetworkProvider(this);

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        #endregion Конструкторы

        #region Реализация INetworkMessageHandler

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param _name="message">Сетевое сообщение</param>
        public void ProcessNetworkMessage(NetworkMessage message)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    break;

                case NetworkMessage.OperationCode.SuccessfulRegistrationCode:
                    ProcessSuccessfulRegistrationNetworkMessage(message);

                    break;

                case NetworkMessage.OperationCode.RegistrationFailedCode:
                    break;

                case NetworkMessage.OperationCode.AuthorizationCode:
                    break;

                case NetworkMessage.OperationCode.SuccessfulSearchCode:
                    ProcessSuccessfulSearchNetworkMessage(message);

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

        #region Методы обработки сетевых сообщений

        /// <summary>
        /// Обаработать сетевое сообщение о регистрации пользователя асинхронно
        /// </summary>
        /// <param _name="message">Сетевое сообщение</param>
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
        /// Обработать сетвое сообщение об успешном поиске пользователя
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        private void ProcessSuccessfulSearchNetworkMessage(NetworkMessage message)
        {
            var data = message.Data;

            UserSearchResultDto userSearchResultDto = Deserializer.Deserialize<UserSearchResultDto>(data);

            UserSearchResult userSearchResult = _mapper.Map<UserSearchResult>(userSearchResultDto);

            SearchResultReceived?.Invoke(userSearchResult);
        }

        #endregion Методы обработки сетевых сообщений

        /// <summary>
        /// Зарегистрировать нового пользователя асинхронно
        /// </summary>
        /// <param _name="networkProviderDto">DTO, сетевого провайдера с Id из базы данных</param>
        /// 
        public void RegisterNewUser(NetworkProviderDto networkProviderDto)
        {
            ClientNetworkProvider.Id = networkProviderDto.Id;

            SignUp?.Invoke();
        }



        #region Методы взаимодействия с сетью

        /// <summary>
        /// Отправить регистрационный запрос асинхронно
        /// </summary>
        /// <param _name="searchingData">Данные о регистрации</param>
        /// <returns></returns>
        public async Task SendRegistrationRequestAsync(RegistrationRequest registrationData)
        {
            User user = _mapper.Map<User>(registrationData);
            User = user;

            RegistrationDto registrationDto = _mapper.Map<RegistrationDto>(registrationData);

            byte[] data = Serializer<RegistrationDto>.Serialize(registrationDto);

            NetworkMessage message = new NetworkMessage(data, NetworkMessage.OperationCode.RegistrationCode);

            await ClientNetworkProvider.ConnectAsync(message);
        }

        /// <summary>
        /// Отправить запрос на поиск пользователя асинхронно
        /// </summary>
        /// <param _name="searchingData">Данные о запросе поиска пользователя</param>
        /// <returns></returns>
        public async Task SendSearchRequestAsync(UserSearchRequest searchingData)
        {
            UserSearchRequestDto searchRequestDto = _mapper.Map<UserSearchRequestDto>(searchingData);

            byte[] data = Serializer<UserSearchRequestDto>.Serialize(searchRequestDto);

            NetworkMessage message = new NetworkMessage(data, NetworkMessage.OperationCode.SearchUserCode);

            await ClientNetworkProvider.Sender.SendNetworkMessageAsync(message);
        }

        #endregion Методы взаимодействия с сетью
    }
}