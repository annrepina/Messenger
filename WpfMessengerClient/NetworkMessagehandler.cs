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
using System.Windows;
using WpfMessengerClient.Models.Mapping;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.Services;

namespace WpfMessengerClient
{
    /// <summary>
    /// Класс, который является посредником между сетевым провайдероми и данными пользователя
    /// </summary>
    public class NetworkMessageHandler : BaseNotifyPropertyChanged, INetworkMessageHandler
    {
        #region События

        /// <summary>
        /// Событие регистрации пользователя
        /// </summary>
        public event Action<int> SignUp;

        /// <summary>
        /// Событие входа пользователя
        /// </summary>
        public event Action SignIn;

        public event Action SignOut;

        /// <summary>
        /// Событие получение успешного результата поиска пользователя
        /// </summary>
        public event Action<UserSearchResponse?> SearchResultReceived;

        /// <summary>
        /// Событие - диалог создан, обработчик принимает в качестве аргумента Id диалога
        /// </summary>
        public event Action<int> DialogCreated;

        #endregion События

        #region Приватные поля

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

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkMessageHandler()
        {
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
            switch (message.Code)
            {
                case NetworkMessageCode.RegistrationCode:
                    break;

                case NetworkMessageCode.SuccessfulRegistrationCode:
                    ProcessSuccessfulRegistrationNetworkMessage(message);

                    break;

                case NetworkMessageCode.RegistrationFailedCode:
                    break;

                case NetworkMessageCode.AuthorizationCode:
                    break;

                case NetworkMessageCode.SuccessfulSearchCode:
                    ProcessSuccessfulSearchNetworkMessage(message);

                    break;

                case NetworkMessageCode.SearchFailedCode:
                    ProcessFailedSearchNetworkMessage();

                    break;

                case NetworkMessageCode.SendingMessageCode:
                    break;
                case NetworkMessageCode.ExitCode:
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
            try
            {
                SuccessfulRegistrationResponseDto successfulRegistrationDto = Deserializer.Deserialize<SuccessfulRegistrationResponseDto>(message.Data);

                RegisterNewUser(successfulRegistrationDto);
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
            byte[]? data = message.Data;

            UserSearchResponseDto userSearchResultDto = Deserializer.Deserialize<UserSearchResponseDto>(data);

            UserSearchResponse userSearchResult = _mapper.Map<UserSearchResponse>(userSearchResultDto);

            SearchResultReceived?.Invoke(userSearchResult);
        }

        /// <summary>
        /// Обработать сетевое сообщение о неудачном поиске
        /// </summary>
        private void ProcessFailedSearchNetworkMessage()
        {
            UserSearchResponse userSearchResult = null;

            SearchResultReceived?.Invoke(userSearchResult);
        }

        #endregion Методы обработки сетевых сообщений

        /// <summary>
        /// Зарегистрировать нового пользователя асинхронно
        /// </summary>
        /// <param _name="networkProviderDto">DTO, сетевого провайдера с Id из базы данных</param>
        /// 
        public void RegisterNewUser(SuccessfulRegistrationResponseDto successfulRegistrationDto)
        {
            SuccessfulRegistrationResponse registrationResponse = _mapper.Map<SuccessfulRegistrationResponse>(successfulRegistrationDto);

            ClientNetworkProvider.Id = registrationResponse.NetworkProviderId;

            SignUp?.Invoke(registrationResponse.UserId);
        }

        #region Методы взаимодействия с сетью

        /// <summary>
        /// Отправить регистрационный запрос асинхронно
        /// </summary>
        /// <param _name="searchingData">Данные о регистрации</param>
        /// <returns></returns>
        public async Task SendRegistrationRequestAsync(RegistrationRequest registrationData)
        {
            RegistrationDto registrationDto = _mapper.Map<RegistrationDto>(registrationData);

            byte[] data = Serializer<RegistrationDto>.Serialize(registrationDto);

            NetworkMessage message = new NetworkMessage(data, NetworkMessageCode.RegistrationCode);

            await ClientNetworkProvider.ConnectAsync(message);
        }

        /// <summary>
        /// Обобщенный метод асинхронной отправки сетевого сообщеия
        /// </summary>
        /// <typeparam name="Treq">Тип объекта, представляющего запрос на сервер</typeparam>
        /// <typeparam name="Tdto">Тип dto объекта, представляющего запрос на сервер</typeparam>
        /// <param name="requestData">Данные запроса на сервер</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns></returns>
        public async Task SenRequestAsync<Treq, Tdto>(Treq requestData, NetworkMessageCode code)
                    where Tdto : class
        {
            try
            {
                Tdto dto = _mapper.Map<Tdto>(requestData);

                byte[] data = Serializer<Tdto>.Serialize(dto);

                NetworkMessage networkMessage = new NetworkMessage(data, code);

                await ClientNetworkProvider.Sender.SendNetworkMessageAsync(networkMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        #endregion Методы взаимодействия с сетью
    }
}