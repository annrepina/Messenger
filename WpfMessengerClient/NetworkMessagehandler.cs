using AutoMapper;
using DtoLib.Dto;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
using DtoLib.NetworkInterfaces;
using DtoLib.NetworkServices;
using DtoLib.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Mapping;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.Services;
using static System.Net.Mime.MediaTypeNames;

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
        public event Action<RegistrationResponse> GotSignUpResponse;

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
        /// Событие - диалог создан, обработчик принимает в качестве аргумента ответ на создание нового диалога
        /// </summary>
        public event Action<CreateDialogResponse> DialogCreated;

        /// <summary>
        /// Событие - получили запрос на создание нового диалога
        /// </summary>
        public event Action<Dialog> GotCreateDialogRequest;

        /// <summary>
        /// Событие - получили ответ, что сообщение доставлено
        /// </summary>
        public event Action<SendMessageResponse> MessageDelivered;

        /// <summary>
        /// Событие - в диалоге появилось новое сообщение
        /// Либо сообщение получено от собеседника, либо сообщение отправил текущий пользователь с другого устройства
        /// </summary>
        public event Action<MessageRequest> DialogReceivedNewMessage;

        /// <summary>
        /// Событие удаления сообщения
        /// </summary>
        public event Action MessageDeletedResponse;

        /// <summary>
        /// Собфтие получения запроса на удаление сообщения
        /// </summary>
        public event Action<DeleteMessageRequest> GotDeleteMessageRequest;

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
        /// <param _name="_message">Сетевое сообщение</param>
        public void ProcessNetworkMessage(NetworkMessage message)
        {
            switch (message.Code)
            {
                case NetworkMessageCode.RegistrationResponseCode:
                    //ProcessSuccessfulRegistrationNetworkMessage(message);
                    ProcessMessage<RegistrationResponseDto, RegistrationResponse>(message, GotSignUpResponse);
                    break;

                case NetworkMessageCode.AuthorizationResponseCode:
                    break;

                case NetworkMessageCode.SearchUserResponseCode:
                    ProcessMessage<UserSearchResponseDto, UserSearchResponse>(message, SearchResultReceived);
                    //ProcessSuccessfulSearchNetworkMessage(message);
                    break;

                //case NetworkMessageCode.SearchFailedCode:
                //    ProcessFailedSearchNetworkMessage();
                //    break;

                case NetworkMessageCode.CreateDialogRequestCode:
                    ProcessMessage<DialogDto, Dialog>(message, GotCreateDialogRequest);
                    //ProcessCreateDialogRequest(message);
                    break;

                case NetworkMessageCode.CreateDialogResponseCode:
                    ProcessMessage<CreateDialogResponseDto, CreateDialogResponse>(message, DialogCreated);
                    //ProcessSuccessfulCreatingDialogNetworkMessage(message);
                    break;

                case NetworkMessageCode.SendMessageRequestCode:
                    ProcessMessage<MessageRequestDto, MessageRequest>(message, DialogReceivedNewMessage);
                    break;

                case NetworkMessageCode.MessageDeliveredCode:
                    ProcessMessage<SendMessageResponseDto, SendMessageResponse>(message, MessageDelivered);
                    //ProcessMessageDeliveredResponse(message);
                    break;

                case NetworkMessageCode.DeleteMessageResponseCode:
                    MessageDeletedResponse?.Invoke();
                    break;

                case NetworkMessageCode.DeleteMessageRequestCode:
                    ProcessMessage<DeleteMessageRequestDto, DeleteMessageRequest>(message, GotDeleteMessageRequest);
                    break;

                case NetworkMessageCode.ExitCode:
                    break;
                default:
                    break;
            }
        }

        





        #endregion Реализация INetworkMessageHandler

        /// <summary>
        /// Обобщенный метод обработки сетевого сообщения
        /// </summary>
        /// <typeparam name="Tdto">Тип представляющий dto ожидаемого объекта в сетевом сообщении</typeparam>
        /// <typeparam name="Tdest">Тип представляющий ожидаемый объект  в сетевом сообщении</typeparam>
        /// <param name="message">Сетевое сообщение</param>
        /// <param name="action">Событие, которое необходимо вызывать</param>
        private void ProcessMessage<Tdto, Tdest>(NetworkMessage message, Action<Tdest> action)
            where Tdest : class
        {
            Tdest destination;

            Tdto dto = SerializationHelper.Deserialize<Tdto>(message.Data);
            destination = _mapper.Map<Tdest>(dto);

            action?.Invoke(destination);
        }

        #region Методы взаимодействия с сетью

        /// <summary>
        /// Отправить регистрационный запрос асинхронно
        /// </summary>
        /// <param _name="registrationRequest">Запрос на регистрацию</param>
        /// <returns></returns>
        public async Task SendRegistrationRequestAsync(RegistrationRequest registrationRequest)
        {
            RegistrationRequestDto registrationDto = _mapper.Map<RegistrationRequestDto>(registrationRequest);

            byte[] data = SerializationHelper.Serialize(registrationDto);

            NetworkMessage message = new NetworkMessage(data, NetworkMessageCode.RegistrationRequestCode);

            byte[] messageBytes = SerializationHelper.Serialize(message);

            if(!ClientNetworkProvider.IsConnected)
                await ClientNetworkProvider.ConnectAsync(messageBytes);

            else
                await ClientNetworkProvider.Transmitter.SendNetworkMessageAsync(messageBytes);
        }

        /// <summary>
        /// Обобщенный метод асинхронной отправки сетевого сообщеия
        /// </summary>
        /// <typeparam name="Treq">Тип объекта, представляющего запрос на сервер</typeparam>
        /// <typeparam name="Tdto">Тип dto объекта, представляющего запрос на сервер</typeparam>
        /// <param name="requestData">Данные запроса на сервер</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns></returns>
        public async Task SendRequestAsync<Treq, Tdto>(Treq requestData, NetworkMessageCode code)
                    where Tdto : class
        {
            try
            {
                Tdto dto = _mapper.Map<Tdto>(requestData);

                byte[] data = SerializationHelper.Serialize(dto);

                NetworkMessage networkMessage = new NetworkMessage(data, code);

                byte[] messageBytes = SerializationHelper.Serialize(networkMessage);

                await ClientNetworkProvider.Transmitter.SendNetworkMessageAsync(messageBytes);
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