using AutoMapper;
using CommonLib.Dto;
using CommonLib.Dto.Requests;
using CommonLib.Dto.Responses;
using CommonLib.NetworkServices;
using CommonLib.Serialization;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Mapping;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.NetworkServices.Interfaces;
using WpfMessengerClient.Services;

namespace WpfMessengerClient.NetworkMessageProcessing
{
    /// <summary>
    /// Обработчик сетевого сообщения
    /// </summary>
    public class NetworkMessageHandler : BaseNotifyPropertyChanged, INetworkMessageHandler
    {
        #region События

        /// <summary>
        /// Событие - ответ от сервера на запрос о регистрации пользователя в мессенджере получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SignUpResponse> SignUpResponseReceived = new();

        /// <summary>
        /// Событие - ответ от сервера на запрос о входе пользователя получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SignInResponse> SignInResponseReceived = new();

        /// <summary>
        /// Событие - ответ от сервера на запрос о выходе пользователя из мессенджера получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<Response> SignOutResponseReceived = new();

        /// <summary>
        /// Событие  - ответ от сервера на запрос о поиске пользователя получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SearchResponse> SearchResponseReceived = new();

        /// <summary>
        /// Событие - ответ от сервера на запрос о создании диалога получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<CreateDialogResponse> CreateDialogResponseReceived = new();

        /// <summary>
        /// Событие - запрос от сервера на создание диалога получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<Dialog> CreateDialogRequestReceived = new();

        /// <summary>
        /// Событие - ответ от сервера на запрос об отправке сообщения получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SendMessageResponse> SendMessageResponseReceived = new();

        /// <summary>
        /// Событие - запрос от сервера на получение диалогом нового сообщения получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<SendMessageRequest> DialogReceivedNewMessage = new();

        /// <summary>
        /// Событие - ответ от сервера на запрос об удалении сообщения получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<Response> DeleteMessageResponseReceived = new();

        /// <summary>
        /// Событие - запрос от сервера на удаления сообщения получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<DeleteMessageRequest> DeleteMessageRequestReceived = new();

        /// <summary>
        /// Событие - ответ от сервера на запрос об удалении диалога получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<Response> DeleteDialogResponseReceived = new();

        /// <summary>
        /// Событие - запрос об удалении диалога от сервера получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<DeleteDialogRequest> DeleteDialogRequestReceived = new();

        /// <summary>
        /// Событие - ответ от сервера на запрос о прочтении сообщения получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<Response> ReadMessageResponseReceived = new();

        /// <summary>
        /// Событие - запрос от сервера о прочтении сообщения текущим пользователем на другом клиенте получен
        /// </summary>
        public readonly NetworkMessageHandlerEvent<ReadMessagesRequest> ReadMessagesRequestReceived = new();

        #endregion События

        #region Приватные поля

        /// <summary>
        /// Маппер для мапинга DTO
        /// </summary>
        private readonly IMapper _mapper;

        #endregion Приватные поля

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkMessageHandler()
        {
            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        #endregion Конструкторы

        #region Реализация INetworkMessageHandler

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="netMessageBytes">Сетевое сообщение в виде массива байт</param>
        public void ProcessNetworkMessage(byte[] netMessageBytes)
        {
            NetworkMessage networkMessage = SerializationHelper.Deserialize<NetworkMessage>(netMessageBytes);

            ProcessNetworkMessage(networkMessage);
        }

        #endregion Реализация INetworkMessageHandler

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        private void ProcessNetworkMessage(NetworkMessage networkMessage)
        {
            switch (networkMessage.Code)
            {
                case NetworkMessageCode.SignUpResponseCode:
                    ProcessNetworkMessage<SignUpResponseDto, SignUpResponse>(networkMessage, SignUpResponseReceived);
                    break;

                case NetworkMessageCode.SignInResponseCode:
                    ProcessNetworkMessage<SignInResponseDto, SignInResponse>(networkMessage, SignInResponseReceived);
                    break;

                case NetworkMessageCode.SearchResponseCode:
                    ProcessNetworkMessage<UserSearchResponseDto, SearchResponse>(networkMessage, SearchResponseReceived);
                    break;

                case NetworkMessageCode.CreateDialogRequestCode:
                    ProcessNetworkMessage<DialogDto, Dialog>(networkMessage, CreateDialogRequestReceived);
                    break;

                case NetworkMessageCode.CreateDialogResponseCode:
                    ProcessNetworkMessage<CreateDialogResponseDto, CreateDialogResponse>(networkMessage, CreateDialogResponseReceived);
                    break;

                case NetworkMessageCode.SendMessageRequestCode:
                    ProcessNetworkMessage<SendMessageRequestDto, SendMessageRequest>(networkMessage, DialogReceivedNewMessage);
                    break;

                case NetworkMessageCode.SendMessageResponseCode:
                    ProcessNetworkMessage<SendMessageResponseDto, SendMessageResponse>(networkMessage, SendMessageResponseReceived);
                    break;

                case NetworkMessageCode.DeleteMessageResponseCode:
                    ProcessNetworkMessage<ResponseDto, Response>(networkMessage, DeleteMessageResponseReceived);
                    break;

                case NetworkMessageCode.DeleteMessageRequestCode:
                    ProcessNetworkMessage<DeleteMessageRequestDto, DeleteMessageRequest>(networkMessage, DeleteMessageRequestReceived);
                    break;

                case NetworkMessageCode.ReadMessagesRequestCode:
                    ProcessNetworkMessage<ReadMessagesRequestDto, ReadMessagesRequest>(networkMessage, ReadMessagesRequestReceived);
                    break;

                case NetworkMessageCode.ReadMessagesResponseCode:
                    ProcessNetworkMessage<ResponseDto, Response>(networkMessage, ReadMessageResponseReceived);
                    break;

                case NetworkMessageCode.DeleteDialogResponseCode:
                    ProcessNetworkMessage<ResponseDto, Response>(networkMessage, DeleteDialogResponseReceived);
                    break;

                case NetworkMessageCode.DeleteDialogRequestCode:
                    ProcessNetworkMessage<DeleteDialogRequestDto, DeleteDialogRequest>(networkMessage, DeleteDialogRequestReceived);
                    break;

                case NetworkMessageCode.SignOutResponseCode:
                    ProcessNetworkMessage<ResponseDto, Response>(networkMessage, SignOutResponseReceived);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <typeparam name="Tdto">Тип объекта представляющего DTO</typeparam>
        /// <typeparam name="Tdest">Тип объекта представляющего цель мапинга</typeparam>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="event">Событие получения ответа/запроса</param>
        private void ProcessNetworkMessage<Tdto, Tdest>(NetworkMessage networkMessage, NetworkMessageHandlerEvent<Tdest> @event)
            where Tdest : class
        {
            Tdto dto = SerializationHelper.Deserialize<Tdto>(networkMessage.Data);
            Tdest destination = _mapper.Map<Tdest>(dto);

            @event?.Invoke(destination);
        }
    }
}