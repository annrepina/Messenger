using AutoMapper;
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

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Класс, который отвечает за логи
    /// </summary>
    public class NetworkMessageHandler : IServerNetworkMessageHandler
    {
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
                    return ProcessSignUpRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.SignInRequestCode:
                    return ProcessSignInRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.SearchUserRequestCode:
                    return ProcessSearchUserRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.CreateDialogRequestCode:
                    return ProcessCreateDialogRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.SendMessageRequestCode:
                    return ProcessSendMessageRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.DeleteMessageRequestCode:
                    return ProcessDeleteMessageRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.DeleteDialogRequestCode:
                    return ProcessDeleteDialogRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.SignOutRequestCode:
                    return ProcessSignOutRequest(networkMessage, networkProviderId);

                case NetworkMessageCode.MessagesAreReadRequestCode:
                    return ProcessMessagesAreReadRequest(networkMessage, networkProviderId);

                default:
                    return new byte[] { };
            }
        }

        #endregion INetworkHandler Implementation

        private byte[] ProcessSignUpRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                SignUpRequestDto signUpRequestDto = SerializationHelper.Deserialize<SignUpRequestDto>(networkMessage.Data);

                User? user = _dbService.AddNewUser(signUpRequestDto);

                SignUpResponse signUpResponse = CreateSignUpResponse(user, networkProviderId);

                byte[] responseBytes = CreateNetworkMessageBytes<SignUpResponse, SignUpResponseDto>(signUpResponse, NetworkMessageCode.SignUpResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, signUpRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.SignUpResponseCode, signUpResponse.Status);
                
                return responseBytes;
            }
            catch (Exception)
            {
                SignUpResponse errorResponse = new SignUpResponse(NetworkResponseStatus.FatalError);
                SendError<SignUpResponse, SignUpResponseDto>(networkProviderId, errorResponse, NetworkMessageCode.SignUpResponseCode);
                throw;
            }
        }

        /// <summary>
        /// Обработать запрос на вход в мессенджер
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="networkProviderId"></param>
        private byte[] ProcessSignInRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                SignInRequestDto signInRequestDto = SerializationHelper.Deserialize<SignInRequestDto>(networkMessage.Data);

                User? user = _dbService.FindUserByPhoneNumber(signInRequestDto.PhoneNumber);

                SignInResponse response = CreateSignInResponse(user, signInRequestDto, networkProviderId);

                byte[] byteResponse = CreateNetworkMessageBytes<SignInResponse, SignInResponseDto>(response, NetworkMessageCode.SignInResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, signInRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.SignInResponseCode, response.Status);

                return byteResponse;
            }
            catch (Exception)
            {
                SignInResponse signInResponse = new SignInResponse(NetworkResponseStatus.FatalError);
                SendError<SignInResponse, SignInResponseDto>(networkProviderId, signInResponse, NetworkMessageCode.SignUpResponseCode);
                throw;
            }
        }

        /// <summary>
        /// Посик пользователя в мессенджере
        /// </summary>
        /// <param name="networkMessage">Сообщение</param>
        /// <returns></returns>
        private byte[] ProcessSearchUserRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                UserSearchRequestDto searchRequestDto = SerializationHelper.Deserialize<UserSearchRequestDto>(networkMessage.Data);

                List<User> usersList = _dbService.FindListOfUsers(searchRequestDto);

                UserSearchResponse response = CreateUserSearchResponse(usersList);

                byte[] byteResponse = CreateNetworkMessageBytes<UserSearchResponse, UserSearchResponseDto>(response, NetworkMessageCode.SearchUserResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, searchRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.SearchUserResponseCode, response.Status);

                return byteResponse;
            }
            catch (Exception)
            {
                UserSearchResponse response = new UserSearchResponse(NetworkResponseStatus.FatalError);
                SendError<UserSearchResponse, UserSearchResponseDto>(networkProviderId, response, NetworkMessageCode.SearchUserResponseCode);
                throw;
            }
        }

        /// <summary>
        /// Обработать запрос на создание нового диалога
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProviderId">Сетевой провайдер на стороне сервера</param>
        private byte[] ProcessCreateDialogRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                CreateDialogRequestDto createDialogRequestDto = SerializationHelper.Deserialize<CreateDialogRequestDto>(networkMessage.Data);

                Dialog dialog = _dbService.CreateDialog(createDialogRequestDto);

                CreateDialogResponse response = _mapper.Map<CreateDialogResponse>(dialog);

                byte[] byteResponse = CreateNetworkMessageBytes<CreateDialogResponse, CreateDialogResponseDto>(response, NetworkMessageCode.CreateDialogResponseCode);

                BroadcastCreateDialogRequests(dialog, networkProviderId);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, createDialogRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.CreateDialogResponseCode, NetworkResponseStatus.Successful);

                return byteResponse;
            }
            catch (Exception)
            {
                CreateDialogResponse createDialogResponse = new CreateDialogResponse(NetworkResponseStatus.FatalError);
                SendError<CreateDialogResponse, CreateDialogResponseDto>(networkProviderId, createDialogResponse, NetworkMessageCode.CreateDialogResponseCode);
                throw;
            }
        }

        /// <summary>
        /// Обработать запрос на отправку сообщения
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProviderId">Сетевой провайдер на стороне сервера</param>
        private byte[] ProcessSendMessageRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                SendMessageRequestDto sendMessageRequestDto = SerializationHelper.Deserialize<SendMessageRequestDto>(networkMessage.Data);

                Message message = _dbService.AddMessage(sendMessageRequestDto);
                SendMessageResponse responseForSender = new SendMessageResponse(message.Id, NetworkResponseStatus.Successful);

                BroadcastSendMessageRequest(message, networkProviderId);

                byte[] byteResponse = CreateNetworkMessageBytes<SendMessageResponse, SendMessageResponseDto>(responseForSender, NetworkMessageCode.MessageDeliveredCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, sendMessageRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.MessageDeliveredCode, NetworkResponseStatus.Successful);

                return byteResponse;
            }
            catch (Exception)
            {
                SendMessageResponse sendMessageResponse = new SendMessageResponse(NetworkResponseStatus.FatalError);
                SendError<SendMessageResponse, SendMessageResponseDto>(networkProviderId, sendMessageResponse, NetworkMessageCode.MessageDeliveredCode);
                throw;
            }
        }

        private byte[] ProcessDeleteMessageRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                DeleteMessageRequestDto deleteMessageRequestDto = SerializationHelper.Deserialize<DeleteMessageRequestDto>(networkMessage.Data);

                Message? message = _dbService.FindMessage(deleteMessageRequestDto);

                Response deleteMessageResponse = CreateDeleteMessageResponse(message, deleteMessageRequestDto.UserId, networkProviderId);

                byte[] responseBytes = CreateNetworkMessageBytes<Response, ResponseDto>(deleteMessageResponse, NetworkMessageCode.DeleteMessageResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, deleteMessageRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.DeleteDialogResponseCode, deleteMessageResponse.Status);

                return responseBytes;
            }
            catch (Exception)
            {
                Response response = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, response, NetworkMessageCode.DeleteMessageResponseCode);
                throw;
            }
        }

        /// <summary>
        /// Обрабатывает запрос на удаление диалога
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="serverNetworkProvider"></param>
        private byte[] ProcessDeleteDialogRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                DeleteDialogRequestDto deleteDialogRequestDto = SerializationHelper.Deserialize<DeleteDialogRequestDto>(networkMessage.Data);

                Dialog? dialog = _dbService.FindDialog(deleteDialogRequestDto);

                Response deleteDialogResponse = CreateDeleteDialogResponse(dialog, networkProviderId, deleteDialogRequestDto.UserId);

                byte[] responseBytes = CreateNetworkMessageBytes<Response, ResponseDto>(deleteDialogResponse, NetworkMessageCode.DeleteDialogResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, deleteDialogRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.DeleteDialogResponseCode, deleteDialogResponse.Status);

                return responseBytes;
            }
            catch (Exception)
            {
                Response errorResponse = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, errorResponse, NetworkMessageCode.DeleteDialogResponseCode);
                throw;
            }
        }

        private byte[] ProcessSignOutRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                SignOutRequestDto signOutRequestDto = SerializationHelper.Deserialize<SignOutRequestDto>(networkMessage.Data);

                _conectionController.DisconnectUser(signOutRequestDto.UserId, networkProviderId);

                Response signOutResponse = new Response(NetworkResponseStatus.Successful);

                byte[] responseBytes = CreateNetworkMessageBytes<Response, ResponseDto>(signOutResponse, NetworkMessageCode.SignOutResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, signOutRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.SignOutResponseCode, signOutResponse.Status);

                return responseBytes;
            }
            catch (Exception)
            {
                Response errorResponse = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, errorResponse, NetworkMessageCode.SignOutResponseCode);
                throw;
            }
        }

        private byte[] ProcessMessagesAreReadRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                MessagesAreReadRequestDto messagesAreReadRequest = SerializationHelper.Deserialize<MessagesAreReadRequestDto>(networkMessage.Data);

                _dbService.ReadMessages(messagesAreReadRequest);

                Response response = CreateMessagesAreReadRersponse(messagesAreReadRequest, networkProviderId);

                byte[] byteResponse = CreateNetworkMessageBytes<Response, ResponseDto>(response, NetworkMessageCode.MessagesAreReadResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, messagesAreReadRequest.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.MessagesAreReadResponseCode, response.Status);

                return byteResponse;
            }
            catch (Exception)
            {
                Response errorResponse = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, errorResponse, NetworkMessageCode.MessagesAreReadResponseCode);
                throw;
            }
        }

        private void SendError<TResponse, TResponseDto>(int networkProviderId, TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            byte[] responseBytes = CreateNetworkMessageBytes<TResponse, TResponseDto>(response, code);
            _conectionController.BroadcastErrorToSenderAsync(responseBytes, networkProviderId);
        }

        /// <summary>
        /// Обобщенный метод создания ответного сетевого сообщения
        /// </summary>
        /// <typeparam name="Tsource">Тип данных источника для мапинга на dto</typeparam>
        /// <typeparam name="Tdto">Тип конкретного dto</typeparam>
        /// <param name="tsource">Объект - источник для мапинга на dto</param>
        /// <param name="dto">Объект, представляющий dto</param>
        /// <param name="code">Код операции</param>
        /// <returns></returns>
        public NetworkMessage CreateNetworkMessage<Tsource, Tdto>(Tsource tsource, out Tdto dto, NetworkMessageCode code)
            where Tdto : class
        {
            dto = _mapper.Map<Tdto>(tsource);

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
        /// <param name="networkProviderId">Сетевой провайдер</param>
        /// <returns></returns>
        private SignUpResponse CreateSignUpResponse(User? user, int networkProviderId)
        {
            if (user != null)
            {
                _conectionController.AddNewSession(user.Id, networkProviderId);

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

        private UserSearchResponse CreateUserSearchResponse(List<User> usersList)
        {
            if (usersList.Count > 0)
                return new UserSearchResponse(usersList, NetworkResponseStatus.Successful);

            return new UserSearchResponse(NetworkResponseStatus.Failed);
        }

        private Response CreateDeleteMessageResponse(Message? message, int userId, int networkProviderId)
        {
            if (message != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(message.DialogId, userId);
                _dbService.DeleteMessage(message);

                DeleteMessageRequestForClient deleteMessageRequestForClient = new DeleteMessageRequestForClient(message.Id, message.DialogId);

                byte[] requestBytes = CreateNetworkMessageBytes<DeleteMessageRequestForClient, DeleteMessageRequestForClientDto>(deleteMessageRequestForClient, NetworkMessageCode.DeleteMessageRequestCode);

                _conectionController.BroadcastNetworkMessageToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastNetworkMessageToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }

        private Response CreateDeleteDialogResponse(Dialog? dialog, int networkProviderId, int userId)
        {
            if (dialog != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(dialog.Id, userId);

                _dbService.DeleteDialog(dialog);

                DeleteDialogRequestForClient deleteDialogRequestForClient = new DeleteDialogRequestForClient(dialog.Id);

                byte[] requestBytes = CreateNetworkMessageBytes<DeleteDialogRequestForClient, DeleteDialogRequestForClientDto>(deleteDialogRequestForClient, NetworkMessageCode.DeleteDialogRequestCode);

                _conectionController.BroadcastNetworkMessageToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastNetworkMessageToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }

        private Response CreateMessagesAreReadRersponse(MessagesAreReadRequestDto readRequest, int networkProviderId)
        {
            Response response = new Response(NetworkResponseStatus.Successful);

            MessagesAreReadRequestForClient messagesAreReadRequest = new MessagesAreReadRequestForClient(readRequest.MessagesId, readRequest.DialogId);
            byte[] requestBytes = CreateNetworkMessageBytes<MessagesAreReadRequestForClient, MessagesAreReadRequestForClientDto>(messagesAreReadRequest, NetworkMessageCode.MessagesAreReadRequestCode);

            _conectionController.BroadcastNetworkMessageToSenderAsync(requestBytes, readRequest.UserId, networkProviderId);

            return response;
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