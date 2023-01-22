using AutoMapper;
using Azure;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Entities.Mapping;
using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Requests;
using ConsoleMessengerServer.Responses;
using DtoLib.Dto;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
using DtoLib.NetworkServices;
using DtoLib.Serialization;
using System.Data.Entity.Core;
using System.Net.Sockets;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Класс, который отвечает за логи
    /// </summary>
    public class NetworkMessageHandler : IServerNetworkMessageHandler
    {
        /// <summary>
        /// Словарь, который содержит пары: ключ - id агрегатора соединений пользователя 
        /// и значение - самого агрегатора
        /// </summary>
        private Dictionary<int, UserProxy> _userProxyList;

        /// <inheritdoc cref="ConnectionController"/>
        private IConnectionController _conectionController;

        /// <summary>
        /// Маппер для мапинга ентити на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис для работы с базами данных
        /// </summary>
        private readonly DbService _dbService;

        public IConnectionController ConnectionController { set => _conectionController = value; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkMessageHandler()
        {
            _userProxyList = new Dictionary<int, UserProxy>();

            DataBaseMapper mapper = DataBaseMapper.GetInstance();
            _mapper = mapper.CreateIMapper();

            _dbService = new DbService();
        }

        #region INetworkHandler Implementation

        public byte[] ProcessData(byte[] data, int senderId)
        {
            NetworkMessage networkMessage = SerializationHelper.Deserialize<NetworkMessage>(data);

            byte[] response = ProcessNetworkMessage(networkMessage, senderId);

            return response;
        }

        /// <summary>
        /// Обработка сетевого сообщения
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProviderId">Идентификатор отправителя</param>
        private byte[] ProcessNetworkMessage(NetworkMessage networkMessage, int networkProviderId)
        {
            switch (networkMessage.Code)
            {
                case NetworkMessageCode.SignUpRequestCode:
                    return ProcessRequest<SignUpRequestDto, User, SignUpResponse, SignUpResponseDto>(networkMessage, networkProviderId, _dbService.AddNewUser, CreateSignUpResponse, NetworkMessageCode.SignUpResponseCode);

                case NetworkMessageCode.SignInRequestCode:
                    return ProcessSignInRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.SearchUserRequestCode:
                    return ProcessSearchUserRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.CreateDialogRequestCode:
                    return ProcessCreateDialogRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.SendMessageRequestCode:
                    return ProcessSendMessageRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.DeleteMessageRequestCode:
                    return ProcessRequest<DeleteMessageRequestDto, Message, DeleteMessageResponse, DeleteMessageResponseDto>(networkMessage, networkProviderId, _dbService.FindMessage, CreateDeleteMessageResponse, NetworkMessageCode.DeleteMessageResponseCode);

                case NetworkMessageCode.DeleteDialogRequestCode:
                    return ProcessDeleteDialogRequest(networkMessage, networkProviderId);

                default:
                    return new byte[] {};
            }
        }

        #endregion INetworkHandler Implementation

        /// <summary>
        /// Обработать запрос на вход в мессенджер
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="networkProviderId"></param>
        private byte[] ProcessSignInRequest(NetworkMessage networkMessage, /*ServerNetworkProvider serverNetworkProvider*/int networkProviderId)
        {
            SignInRequestDto signInRequestDto = SerializationHelper.Deserialize<SignInRequestDto>(networkMessage.Data);

            User? user = _dbService.FindUserByPhoneNumber(signInRequestDto.PhoneNumber);

            SignInResponse response = CreateSignInResponse(user, signInRequestDto, networkProviderId);

            byte[] byteResponse = CreateNetworkMessageBytes<SignInResponse, SignInResponseDto>(response, NetworkMessageCode.SignInResponseCode);
            
            ReportPrinter.PrintRequestReport(networkMessage.Code, signInRequestDto.ToString());
            ReportPrinter.PrintResponseReport(NetworkMessageCode.SignInResponseCode, response.Status);

            return byteResponse;
        }

        /// <summary>
        /// Посик пользователя в мессенджере
        /// </summary>
        /// <param name="networkMessage">Сообщение</param>
        /// <returns></returns>
        private byte[] ProcessSearchUserRequest(NetworkMessage networkMessage, /*ServerNetworkProvider serverNetworkProvider*/int networkProviderId)
        {
            UserSearchRequestDto searchRequestDto = SerializationHelper.Deserialize<UserSearchRequestDto>(networkMessage.Data);

            List<User>? usersList = _dbService.FindListOfUsers(searchRequestDto);

            UserSearchResponse response = CreateUserSearchResponse(usersList);

            byte[] byteResponse = CreateNetworkMessageBytes<UserSearchResponse, UserSearchResponseDto>(response, NetworkMessageCode.SearchUserResponseCode);

            ReportPrinter.PrintRequestReport(networkMessage.Code, searchRequestDto.ToString());
            ReportPrinter.PrintResponseReport(NetworkMessageCode.SearchUserResponseCode, response.Status);

            return byteResponse;
        }

        /// <summary>
        /// Обработать запрос на создание нового диалога
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер на стороне сервера</param>
        private byte[] ProcessCreateDialogRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            CreateDialogRequestDto createDialogRequestDto = SerializationHelper.Deserialize<CreateDialogRequestDto>(networkMessage.Data);

            Dialog dialog = _dbService.CreateDialog(createDialogRequestDto);

            CreateDialogResponse response = _mapper.Map<CreateDialogResponse>(dialog);

            byte[] byteResponse = CreateNetworkMessageBytes<CreateDialogResponse, CreateDialogResponseDto>(response, NetworkMessageCode.CreateDialogResponseCode);

            BroadcastCreateDialogRequests(dialog, networkProviderId);

            ReportPrinter.PrintRequestReport(networkMessage.Code, createDialogRequestDto.ToString());
            ReportPrinter.PrintResponseReport(NetworkMessageCode.CreateDialogResponseCode, NetworkResponseStatus.Successful);

            return byteResponse;
        }

        private byte[] ProcessRequest<TReqDto, TReqResult, TResponse, TResponseDto>(NetworkMessage networkMessage, int networkProviderId, Func<TReqDto, TReqResult?> processInDb, Func<TReqResult?, int, TResponse> createResponse, NetworkMessageCode code)
            where TReqResult : class
            where TResponseDto : class
            where TResponse : IResponse
        {
            TReqDto reqDto = SerializationHelper.Deserialize<TReqDto>(networkMessage.Data);

            TReqResult? res = processInDb(reqDto);

            TResponse response = createResponse(res, networkProviderId);

            byte[] byteResponse = CreateNetworkMessageBytes<TResponse, TResponseDto>(response, code);

            ReportPrinter.PrintRequestReport(networkMessage.Code, reqDto.ToString());
            ReportPrinter.PrintResponseReport(code, response.Status);

            return byteResponse;
        }

        /// <summary>
        /// Обработать запрос на отправку сообщения
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер на стороне сервера</param>
        private byte[] ProcessSendMessageRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            SendMessageRequestDto sendMessageRequestDto = SerializationHelper.Deserialize<SendMessageRequestDto>(networkMessage.Data);

            Message message = _dbService.AddMessage(sendMessageRequestDto);
            SendMessageResponse responseForSender = new SendMessageResponse(message.Id);

            BroadcastSendMessageRequest(message, networkProviderId);

            byte[] byteResponse = CreateNetworkMessageBytes<SendMessageResponse, SendMessageResponseDto>(responseForSender, NetworkMessageCode.MessageDeliveredCode);

            ReportPrinter.PrintRequestReport(networkMessage.Code, sendMessageRequestDto.ToString());
            ReportPrinter.PrintResponseReport(NetworkMessageCode.MessageDeliveredCode, NetworkResponseStatus.Successful);

            return byteResponse;
        }

        /// <summary>
        /// Обрабатывает запрос на удаление диалога
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="serverNetworkProvider"></param>
        private byte[] ProcessDeleteDialogRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            DeleteDialogRequestDto deleteDialogRequestDto = SerializationHelper.Deserialize<DeleteDialogRequestDto>(networkMessage.Data);

            Dialog? dialog = _dbService.FindDialog(deleteDialogRequestDto);

            DeleteDialogResponse deleteDialogResponse = CreateDeleteDialogResponse(dialog, networkProviderId, deleteDialogRequestDto.UserId);

            byte[] responseBytes = CreateNetworkMessageBytes<DeleteDialogResponse, DeleteDialogResponseDto>(deleteDialogResponse, NetworkMessageCode.DeleteDialogResponseCode);

            ReportPrinter.PrintRequestReport(networkMessage.Code, deleteDialogRequestDto.ToString());
            ReportPrinter.PrintResponseReport(NetworkMessageCode.DeleteDialogResponseCode, deleteDialogResponse.Status);

            return responseBytes;
        }

        /// <summary>
        /// Обобщенный метод создания ответного сетевого сообщения
        /// </summary>
        /// <typeparam name="Tsource">Тип данных источника для мапинга на dto</typeparam>
        /// <typeparam name="Tdto">Тип конкретного dto</typeparam>
        /// <param name="tsource">Объект - сточник для мапинга на dto</param>
        /// <param name="dto">Объект, представляющий dto</param>
        /// <param name="code">Код операции</param>
        /// <returns></returns>
        public NetworkMessage CreateNetworkMessage<Tsource, Tdto>(Tsource tsource, out Tdto dto, NetworkMessageCode code)
            where Tdto : class
        {
            try
            {
                dto = _mapper.Map<Tdto>(tsource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            byte[] data = SerializationHelper.Serialize(dto);

            var message = new NetworkMessage(data, code);

            return message;
        }

        /// <summary>
        /// Отправить ответ на запрос сетевому провайдеру
        /// </summary>
        /// <typeparam name="TResponse">Тип класса ответа</typeparam>
        /// <typeparam name="TResponseDto">Тип dto класса ответа</typeparam>
        /// <param name="response">Ответ</param>
        /// <param name="code">Код операции</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер</param>
        /// <returns></returns>
        private byte[] CreateNetworkMessageBytes<TResponse, TResponseDto>(TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            NetworkMessage responseMessage = CreateNetworkMessage(response, out TResponseDto responseDto, code);

            return SerializationHelper.Serialize(responseMessage);
        }

        #region Методы, создающие ответы на запросы

        /// <summary>
        /// Создать ответы на запрос о регистрации
        /// Ответ для консоли типа String и ответ для клиента типа SignUpResponse
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер</param>
        /// <returns></returns>
        private SignUpResponse CreateSignUpResponse(User? user, int senderId)
        {
            if (user != null)
            {
                _conectionController.AddNewSession(user.Id, senderId);

                return new SignUpResponse(user.Id, NetworkResponseStatus.Successful);
            }

            return new SignUpResponse(NetworkResponseStatus.Failed);
        }

        private SignInResponse CreateSignInResponse(User? user, SignInRequestDto signInRequestDto, int networkProviderId)
        {
            if (user != null)
            {
                if (user.Password == signInRequestDto.Password)
                {
                    List<Dialog> dialogs = _dbService.FindDialogsByUser(user);
                    _conectionController.AddNewSession(user.Id, networkProviderId);

                    return new SignInResponse(user, dialogs, NetworkResponseStatus.Successful);
                }

                return new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.Password);
            }

            return new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.PhoneNumber);
        }

        private UserSearchResponse CreateUserSearchResponse(List<User>? usersList)
        {
            if (usersList != null)
                return new UserSearchResponse(usersList, NetworkResponseStatus.Successful);

            return new UserSearchResponse(usersList, NetworkResponseStatus.Failed);
        }

        private DeleteMessageResponse CreateDeleteMessageResponse(Message? message, int networkProviderId)
        {
            if (message != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(message.DialogId, message.UserSenderId);
                _dbService.DeleteMessage(message);

                DeleteMessageRequestForClient deleteMessageRequestForClient = new DeleteMessageRequestForClient(message.Id, message.DialogId);

                byte[] requestBytes = CreateNetworkMessageBytes<DeleteMessageRequestForClient, DeleteMessageRequestForClientDto>(deleteMessageRequestForClient, NetworkMessageCode.DeleteMessageRequestCode);

                _conectionController.BroadcastNetworkMessageToSenderAsync(requestBytes, message.UserSenderId, networkProviderId);
                _conectionController.BroadcastNetworkMessageToInterlocutorAsync(requestBytes, interlocutorId);

                return new DeleteMessageResponse(NetworkResponseStatus.Successful);
            }

            return new DeleteMessageResponse(NetworkResponseStatus.Failed);
        }

        private DeleteDialogResponse CreateDeleteDialogResponse(Dialog? dialog, int networkProviderId, int userId)
        {
            if (dialog != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(dialog.Id, userId);

                _dbService.DeleteDialog(dialog);

                DeleteDialogRequestForClient deleteDialogRequestForClient = new DeleteDialogRequestForClient(dialog.Id);

                byte[] requestBytes = CreateNetworkMessageBytes<DeleteDialogRequestForClient, DeleteDialogRequestForClientDto>(deleteDialogRequestForClient, NetworkMessageCode.DeleteDialogRequestCode);

                _conectionController.BroadcastNetworkMessageToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastNetworkMessageToInterlocutorAsync(requestBytes, interlocutorId);

                return new DeleteDialogResponse(NetworkResponseStatus.Successful);
            }

            return new DeleteDialogResponse(NetworkResponseStatus.Failed);
        }

        /// <summary>
        /// Создать ответ на запрос о создании диалога
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="networkProviderId"></param>
        /// <returns></returns>
        private void BroadcastCreateDialogRequests(Dialog dialog, int networkProviderId)
        {
            int senderId = dialog.Messages.First().UserSenderId;
            int recipientId = dialog.Users.First(user => user.Id != senderId).Id;

            byte[] createDialogRequest = CreateNetworkMessageBytes<Dialog, DialogDto>(dialog, NetworkMessageCode.CreateDialogRequestCode);

            _conectionController.BroadcastNetworkMessageToSenderAsync(createDialogRequest, senderId, networkProviderId);
            _conectionController.BroadcastNetworkMessageToInterlocutorAsync(createDialogRequest, recipientId);
        }

        private void BroadcastSendMessageRequest(Message message, int networkProviderId)
        {
            int recipietUserId = _dbService.GetRecipientUserId(message);

            SendMessageRequest sendMessageRequest = new SendMessageRequest(message, message.DialogId);

            byte[] sendmessageRequest = CreateNetworkMessageBytes<SendMessageRequest, SendMessageRequestDto>(sendMessageRequest, NetworkMessageCode.SendMessageRequestCode);

            _conectionController.BroadcastNetworkMessageToSenderAsync(sendmessageRequest, message.UserSenderId, networkProviderId);
            _conectionController.BroadcastNetworkMessageToInterlocutorAsync(sendmessageRequest, recipietUserId);
        }

        #endregion Методы, создающие ответы на запросы
    }
}