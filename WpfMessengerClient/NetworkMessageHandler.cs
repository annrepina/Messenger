using AutoMapper;
using DtoLib.Dto;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
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
        public readonly NetworkMessageHandlerEvent<SignUpResponse> SignUpResponseReceived;

        /// <summary>
        /// Событие входа пользователя
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SignInResponse> SignInResponseReceived;

        /// <summary>
        /// Событие выхода пользователя из мессенджера
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SignOutResponse> SignOutResponseReceived;

        /// <summary>
        /// Событие получение успешного результата поиска пользователя
        /// </summary>
        public readonly NetworkMessageHandlerEvent<UserSearchResponse> UserSearchResponseReceived;

        /// <summary>
        /// Событие - диалог создан, обработчик принимает в качестве аргумента ответ на создание нового диалога
        /// </summary>
        public readonly NetworkMessageHandlerEvent<CreateDialogResponse> CreateDialogResponseReceived;

        /// <summary>
        /// Событие - получили запрос на создание нового диалога
        /// </summary>
        public readonly NetworkMessageHandlerEvent<Dialog> CreateDialogRequestReceived;

        /// <summary>
        /// Событие - получили ответ, что сообщение доставлено
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SendMessageResponse> SendMessageResponseReceived;

        /// <summary>
        /// Событие - в диалоге появилось новое сообщение
        /// Либо сообщение получено от собеседника, либо сообщение отправил текущий пользователь с другого устройства
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SendMessageRequest> DialogReceivedNewMessage;

        /// <summary>
        /// Событие получения ответа на запрос обу удалении сообщения
        /// </summary>
        public readonly NetworkMessageHandlerEvent<DeleteMessageResponse> DeleteMessageResponseReceived;

        /// <summary>
        /// Событие получения запроса на удаление сообщения
        /// </summary>
        public readonly NetworkMessageHandlerEvent<DeleteMessageRequestForClient> DeleteMessageRequestForClientReceived;

        /// <summary>
        /// Событие получения ответа на запрос об удалении диалога
        /// </summary>
        public readonly NetworkMessageHandlerEvent<DeleteDialogResponse> DeleteDialogResponseReceived;

        /// <summary>
        /// Событие получения запроса на удаление диалога
        /// </summary>
        public readonly NetworkMessageHandlerEvent<DeleteDialogRequestForClient> DeleteDialogRequestForClientReceived;

        #endregion События

        #region Приватные поля

        /// <summary>
        /// Маппер для мапинга моделей на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

        #endregion Приватные поля

        #region  Свойства

        public IConnectionController ConnectionController { get; set; }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkMessageHandler()
        {
            SignUpResponseReceived = new NetworkMessageHandlerEvent<SignUpResponse> ();
            SignInResponseReceived = new NetworkMessageHandlerEvent<SignInResponse> ();
            SignOutResponseReceived = new NetworkMessageHandlerEvent<SignOutResponse> ();
            UserSearchResponseReceived = new NetworkMessageHandlerEvent<UserSearchResponse> ();
            CreateDialogResponseReceived = new NetworkMessageHandlerEvent<CreateDialogResponse> ();
            CreateDialogRequestReceived = new NetworkMessageHandlerEvent<Dialog>();
            SendMessageResponseReceived = new NetworkMessageHandlerEvent<SendMessageResponse> ();
            DialogReceivedNewMessage = new NetworkMessageHandlerEvent<SendMessageRequest> ();
            DeleteMessageResponseReceived = new NetworkMessageHandlerEvent<DeleteMessageResponse> ();
            DeleteMessageRequestForClientReceived = new NetworkMessageHandlerEvent<DeleteMessageRequestForClient> ();
            DeleteDialogResponseReceived = new NetworkMessageHandlerEvent<DeleteDialogResponse> ();
            DeleteDialogRequestForClientReceived = new NetworkMessageHandlerEvent<DeleteDialogRequestForClient>();

            ConnectionController = null;

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        #endregion Конструкторы

        #region Реализация INetworkMessageHandler

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param _name="_message">Сетевое сообщение</param>
        private void ProcessNetworkMessage(NetworkMessage message)
        {
            switch (message.Code)
            {
                case NetworkMessageCode.SignUpResponseCode:
                    ProcessMessage<SignUpResponseDto, SignUpResponse>(message, SignUpResponseReceived);
                    break;

                case NetworkMessageCode.SignInResponseCode:
                    ProcessMessage<SignInResponseDto, SignInResponse>(message, SignInResponseReceived);
                    break;

                case NetworkMessageCode.SearchUserResponseCode:
                    ProcessMessage<UserSearchResponseDto, UserSearchResponse>(message, UserSearchResponseReceived);
                    break;

                case NetworkMessageCode.CreateDialogRequestCode:
                    ProcessMessage<DialogDto, Dialog>(message, CreateDialogRequestReceived);
                    break;

                case NetworkMessageCode.CreateDialogResponseCode:
                    ProcessMessage<CreateDialogResponseDto, CreateDialogResponse>(message, CreateDialogResponseReceived);
                    break;

                case NetworkMessageCode.SendMessageRequestCode:
                    ProcessMessage<SendMessageRequestDto, SendMessageRequest>(message, DialogReceivedNewMessage);
                    break;

                case NetworkMessageCode.MessageDeliveredCode:
                    ProcessMessage<SendMessageResponseDto, SendMessageResponse>(message, SendMessageResponseReceived);
                    break;

                case NetworkMessageCode.DeleteMessageResponseCode:
                    ProcessMessage<DeleteMessageResponseDto, DeleteMessageResponse>(message, DeleteMessageResponseReceived);
                    break;

                case NetworkMessageCode.DeleteMessageRequestCode:
                    ProcessMessage<DeleteMessageRequestForClientDto, DeleteMessageRequestForClient>(message, DeleteMessageRequestForClientReceived);
                    break;

                case NetworkMessageCode.DeleteDialogResponseCode:
                    ProcessMessage<DeleteDialogResponseDto, DeleteDialogResponse>(message, DeleteDialogResponseReceived);
                    break;

                case NetworkMessageCode.DeleteDialogRequestCode:
                    ProcessMessage<DeleteDialogRequestForClientDto, DeleteDialogRequestForClient>(message, DeleteDialogRequestForClientReceived);
                    break;

                case NetworkMessageCode.SignOutResponseCode:
                    ProcessMessage<SignOutResponseDto, SignOutResponse>(message, SignOutResponseReceived);
                    break;

                default:
                    break;
            }
        }

        #endregion Реализация INetworkMessageHandler

        private void ProcessMessage<Tdto, Tdest>(NetworkMessage message, NetworkMessageHandlerEvent<Tdest> action)
            where Tdest : class
        {
            Tdto dto = SerializationHelper.Deserialize<Tdto>(message.Data);
            Tdest destination = _mapper.Map<Tdest>(dto);

            action?.Invoke(destination);
        }

        #region Методы взаимодействия с сетью

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

                ConnectionController?.SendRequest(messageBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void ProcessData(byte[] data)
        {
            NetworkMessage networkMessage = SerializationHelper.Deserialize<NetworkMessage>(data);

            ProcessNetworkMessage(networkMessage);
        }

        #endregion Методы взаимодействия с сетью
    }
}