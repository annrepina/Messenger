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
    public class NetworkMessageHandlerEvent<TResponce>
        where TResponce : class
    {
        public event Action<TResponce>? Occured;

        public void Invoke(TResponce responce)
        {
            Occured?.Invoke(responce);
        }
    }

    /// <summary>
    /// Класс, который является посредником между сетевым провайдероми и данными пользователя
    /// </summary>
    public class NetworkMessageHandler : BaseNotifyPropertyChanged, INetworkMessageHandler
    {
        #region События

        /// <summary>
        /// Событие регистрации пользователя
        /// </summary>
        //public event Action<SignUpResponse> SignUpResponseReceived;

        public readonly NetworkMessageHandlerEvent<SignUpResponse> SignUpResponseReceived = new ();

        /// <summary>
        /// Событие входа пользователя
        /// </summary>
        public event Action <SignInResponse> SignInResponseReceived;

        public event Action SignOut;

        /// <summary>
        /// Событие получение успешного результата поиска пользователя
        /// </summary>
        public event Action<UserSearchResponse> UserSearchResponseReceived;

        /// <summary>
        /// Событие - диалог создан, обработчик принимает в качестве аргумента ответ на создание нового диалога
        /// </summary>
        public event Action<CreateDialogResponse> CreateDialogResponseReceived;

        /// <summary>
        /// Событие - получили запрос на создание нового диалога
        /// </summary>
        public event Action<Dialog> CreateDialogRequestReceived;

        /// <summary>
        /// Событие - получили ответ, что сообщение доставлено
        /// </summary>
        public event Action<SendMessageResponse> SendMessageResponseReceived;

        /// <summary>
        /// Событие - в диалоге появилось новое сообщение
        /// Либо сообщение получено от собеседника, либо сообщение отправил текущий пользователь с другого устройства
        /// </summary>
        public event Action<SendMessageRequest> DialogReceivedNewMessage;

        /// <summary>
        /// Событие получения ответа на запрос обу удалении сообщения
        /// </summary>
        public event Action<DeleteMessageResponse> DeleteMessageResponseReceived;

        /// <summary>
        /// Событие получения запроса на удаление сообщения
        /// </summary>
        public event Action<DeleteMessageRequestForClient> DeleteMessageRequestForClientReceived;

        /// <summary>
        /// Событие получения ответа на запрос об удалении диалога
        /// </summary>
        public event Action<DeleteDialogResponse> DeleteDialogResponseReceived;

        /// <summary>
        /// Событие получения запроса на удаление диалога
        /// </summary>
        public event Action<DeleteDialogRequestForClient> DeleteDialogRequestForClientReceived;

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
                    //DeleteMessageResponseReceived?.Invoke();
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

        private void ProcessMessage<Tdto, Tdest>(NetworkMessage message, NetworkMessageHandlerEvent<Tdest> action)
            where Tdest : class
        {
            Tdest destination;

            Tdto dto = SerializationHelper.Deserialize<Tdto>(message.Data);
            destination = _mapper.Map<Tdest>(dto);

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

                if (!ClientNetworkProvider.IsConnected)
                    await ClientNetworkProvider.ConnectAsync(messageBytes);

                else
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