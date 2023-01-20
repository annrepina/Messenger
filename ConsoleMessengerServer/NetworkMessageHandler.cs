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

        ///// <summary>
        ///// Сервер, который прослушивает входящие подключения
        ///// </summary>
        //public Server Server { get; set; }
        //IServerNetworkMessageHandler IConnectionController.ServerNetworkMessageHandler { set => throw new NotImplementedException(); }

        public IConnectionController ConnectionController 
        { 
            set
            {
                _conectionController = value;
            }
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkMessageHandler()
        {
            //Server = new Server(this);
            _userProxyList = new Dictionary<int, UserProxy>();

            DataBaseMapper mapper = DataBaseMapper.GetInstance();
            _mapper = mapper.CreateIMapper();

            _dbService = new DbService();
        }

        ///// <summary>
        ///// Асинхронный метод - начать прослушивание входящих подключений
        ///// </summary>
        ///// <returns></returns>
        //public async Task StartListeningConnectionsAsync()
        //{
        //    try
        //    {
        //        await Server.ListenIncomingConnectionsAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        #region INetworkHandler Implementation

        ///// <summary>
        ///// Инициализировать новое подключение
        ///// </summary>
        ///// <param name="tcpClient">TCP клиент</param>
        //public void InitializeNewConnection(TcpClient tcpClient)
        //{
        //    ServerNetworkProvider networkProvider = new ServerNetworkProvider(tcpClient, this);

        //    Console.WriteLine($"Клиент {networkProvider.Id} подключился ");

        //    Task.Run(() => networkProvider.ProcessNetworkMessagesAsync());
        //}

        //// todo Возможно убрать этот метод вообще
        ///// <summary>
        ///// Отключить клиентов от сервера
        ///// </summary>
        //public void DisconnectClients()
        //{
        //    foreach (var client in _userProxyList.Values)
        //    {
        //        client.CloseAll();
        //    }
        //}

        ////todo: Возможно убрать эту реализацию вообще
        ///// <summary>
        ///// Отключить конкретного клиента
        ///// </summary>
        ///// <param name="clientId">Идентификатор клиента</param>
        //public void DisconnectClient(int clientId)
        //{
        //    if (_userProxyList != null && _userProxyList.Count > 0)
        //    {
        //        _userProxyList.Remove(clientId);
        //    }
        //}

        public async Task ProcessDataAsync(byte[] data, int senderId)
        {
            NetworkMessage networkMessage = SerializationHelper.Deserialize<NetworkMessage>(data);

            ProcessNetworkMessage(networkMessage, senderId);
        }

        /// <summary>
        /// Обработка сетевого сообщения
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProviderId">Идентификатор отправителя</param>
        private void ProcessNetworkMessage(NetworkMessage networkMessage, int networkProviderId)
        {
            switch (networkMessage.Code)
            {
                case NetworkMessageCode.SignUpRequestCode:
                    ProcessSignUpRequest(networkMessage, networkProviderId);
                    break;

                case NetworkMessageCode.SignInRequestCode:
                    ProcessSignInRequestAsync(networkMessage, networkProviderId);
                    break;

                case NetworkMessageCode.SearchUserRequestCode:
                    ProcessSearchUserMessage(networkMessage, networkProviderId);
                    break;

                case NetworkMessageCode.CreateDialogRequestCode:
                    ProcessCreateDialogRequest(networkMessage, serverNetworkProvider);
                    break;

                case NetworkMessageCode.SendMessageRequestCode:
                    ProcessSendMessageRequest(networkMessage, serverNetworkProvider);
                    break;

                case NetworkMessageCode.DeleteMessageRequestCode:
                    ProcessDeleteMessageRequest(networkMessage, serverNetworkProvider);
                    break;

                case NetworkMessageCode.DeleteDialogRequestCode:
                    ProcessDeleteDialogRequest(networkMessage, serverNetworkProvider);
                    break;

                default:
                    break;
            }
        }

        ///// <summary>
        ///// Обработать сетевое сообщение
        ///// </summary>
        ///// <param name="networkMessage">Сетевое сообщение</param>
        ///// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        //public async Task ProcessNetworkMessage(NetworkMessage networkMessage, ServerNetworkProvider serverNetworkProvider)
        //{
        //    switch (networkMessage.Code)
        //    {
        //        case NetworkMessageCode.SignUpRequestCode:
        //            ProcessSignUpRequest(networkMessage, serverNetworkProvider);
        //            break;

        //        case NetworkMessageCode.SignInRequestCode:
        //            ProcessSignInRequestAsync(networkMessage, serverNetworkProvider);
        //            break;

        //        case NetworkMessageCode.SearchUserRequestCode:
        //            ProcessSearchUserMessage(networkMessage, serverNetworkProvider);
        //            break;

        //        case NetworkMessageCode.CreateDialogRequestCode:
        //            ProcessCreateDialogRequest(networkMessage, serverNetworkProvider);
        //            break;

        //        case NetworkMessageCode.SendMessageRequestCode:
        //            ProcessSendMessageRequest(networkMessage, serverNetworkProvider);
        //            break;

        //        case NetworkMessageCode.DeleteMessageRequestCode:
        //            ProcessDeleteMessageRequest(networkMessage, serverNetworkProvider);
        //            break;

        //        case NetworkMessageCode.DeleteDialogRequestCode:
        //            ProcessDeleteDialogRequest(networkMessage, serverNetworkProvider);
        //            break;

        //        default:
        //            break;
        //    }
        //}

        #endregion INetworkHandler Implementation

        /// <summary>
        /// Зарегистрировать нового пользователя в мессенджере
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        /// <returns></returns>
        public async Task ProcessSignUpRequest(NetworkMessage networkMessage, /*ServerNetworkProvider serverNetworkProvider*/int senderId)
        {
            SignUpRequestDto signUpRequestDto = SerializationHelper.Deserialize<SignUpRequestDto>(networkMessage.Data);

            User? user = _dbService.AddNewUser(signUpRequestDto);

            SignUpResponse response = CreateSignUpResponse(user, /*serverNetworkProvider*/senderId);

            byte[] byteResponse = CreateByteResponse<SignUpResponse, SignUpResponseDto>(response, NetworkMessageCode.SignUpResponseCode);

            if (response.Status == NetworkResponseStatus.Successful)
                _conectionController.SendResponseToVerifiedUser(byteResponse, user.Id, senderId);

            else
                _conectionController.SendResponseToNetworkProvider(byteResponse, senderId);

            //await SendResponseToNetworkProvider<SignUpResponse, SignUpResponseDto>(response, NetworkMessageCode.SignUpResponseCode, serverNetworkProvider);

            ReportPrinter.PrintRequestReport(networkMessage.Code, signUpRequestDto.ToString());
            ReportPrinter.PrintResponseReport(NetworkMessageCode.SignUpResponseCode, response.Status, signUpRequestDto.ToString());
        }

        /// <summary>
        /// Обработать запрос на вход в мессенджер
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="serverNetworkProvider"></param>
        private async Task ProcessSignInRequestAsync(NetworkMessage networkMessage, /*ServerNetworkProvider serverNetworkProvider*/int networkProviderId)
        {
            SignInRequestDto signInRequestDto = SerializationHelper.Deserialize<SignInRequestDto>(networkMessage.Data);

            User? user = _dbService.FindUserByPhoneNumber(signInRequestDto.PhoneNumber);

            SignInResponse response = CreateSignInResponse(user, signInRequestDto, networkProviderId);

            byte[] byteResponse = CreateByteResponse<SignInResponse, SignInResponseDto>(response, NetworkMessageCode.SignInResponseCode);

            if (response.Status == NetworkResponseStatus.Successful)
                _conectionController.SendResponseToVerifiedUser(byteResponse, user.Id, networkProviderId);

            else
                _conectionController.SendResponseToNetworkProvider(byteResponse, networkProviderId);
            
            ReportPrinter.PrintRequestReport(networkMessage.Code, signInRequestDto.ToString());
            //todo подумать вставлять ли контекст ошибки
            ReportPrinter.PrintResponseReport(NetworkMessageCode.SignInResponseCode, response.Status, signInRequestDto.ToString());
        }

        /// <summary>
        /// Посик пользователя в мессенджере
        /// </summary>
        /// <param name="networkMessage">Сообщение</param>
        /// <returns></returns>
        private async Task ProcessSearchUserMessage(NetworkMessage networkMessage, /*ServerNetworkProvider serverNetworkProvider*/int networkProviderId)
        {
            UserSearchRequestDto searchRequestDto = SerializationHelper.Deserialize<UserSearchRequestDto>(networkMessage.Data);

            List<User>? usersList = _dbService.FindListOfUsers(searchRequestDto);

            UserSearchResponse response = CreateUserSearchResponse(usersList);

            byte[] byteResponse = CreateByteResponse<UserSearchResponse, UserSearchResponseDto>(response, NetworkMessageCode.SearchUserResponseCode);



            await SendResponseToNetworkProvider<UserSearchResponse, UserSearchResponseDto>(response, NetworkMessageCode.SearchUserResponseCode, serverNetworkProvider);

            ReportPrinter.PrintRequestReport(networkMessage.Code, searchRequestDto.ToString());
            ReportPrinter.PrintResponseReport(NetworkMessageCode.SearchUserResponseCode, response.Status, searchRequestDto.ToString());
        }

        /// <summary>
        /// Обработать запрос на создание нового диалога
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер на стороне сервера</param>
        private async Task ProcessCreateDialogRequest(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            CreateDialogRequestDto createDialogRequestDto = SerializationHelper.Deserialize<CreateDialogRequestDto>(message.Data);

            Dialog? dialog = _dbService.CreateDialog(createDialogRequestDto);

            CreateDialogResponse createDialogResponse;

            if (dialog != null)
            {
                int senderId = dialog.Messages.First().UserSenderId;
                int recipientId = dialog.Users.First(user => user.Id != senderId).Id;

                createDialogResponse = _mapper.Map<CreateDialogResponse>(dialog);

                await BroadcastRequest<Dialog, DialogDto>(dialog, NetworkMessageCode.CreateDialogRequestCode, senderId, recipientId, serverNetworkProvider);

                await SendResponseToNetworkProvider<CreateDialogResponse, CreateDialogResponseDto>(createDialogResponse, NetworkMessageCode.CreateDialogResponseCode, serverNetworkProvider);

                Console.WriteLine($"Код операции: {NetworkMessageCode.CreateDialogResponseCode}. Диалог с Id {createDialogResponse.DialogId} создан.");
            }

            else
            {
                createDialogResponse = new CreateDialogResponse(NetworkResponseStatus.Failed);

                await SendResponseToNetworkProvider<CreateDialogResponse, CreateDialogResponseDto>(createDialogResponse, NetworkMessageCode.CreateDialogResponseCode, serverNetworkProvider);

                Console.WriteLine($"Код операции: {NetworkMessageCode.CreateDialogResponseCode}. Статус ответа: {NetworkResponseStatus.Failed}. Диалог не может быть создан.");

                // !!! поставить обработчик выше
                throw new EntityException("Error occurred due to adding dialog to database.");
            }
        }

        /// <summary>
        /// Обработать запрос на отправку сообщения
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер на стороне сервера</param>
        private async Task ProcessSendMessageRequest(NetworkMessage networkMessage, ServerNetworkProvider serverNetworkProvider)
        {
            SendMessageRequestDto sendMessageRequestDto = SerializationHelper.Deserialize<SendMessageRequestDto>(networkMessage.Data);
            SendMessageRequest messageRequest = _mapper.Map<SendMessageRequest>(sendMessageRequestDto);

            Message message = _dbService.AddMessage(messageRequest);
            int recipietUserId = _dbService.GetRecipientUserId(message);

            SendMessageRequest sendMessageRequest = new SendMessageRequest(message, message.DialogId);

            await BroadcastRequest<SendMessageRequest, SendMessageRequestDto>(sendMessageRequest, NetworkMessageCode.SendMessageRequestCode, message.UserSenderId, recipietUserId, serverNetworkProvider);

            SendMessageResponse responseForSender = new SendMessageResponse(message.Id);

            await SendResponseToNetworkProvider<SendMessageResponse, SendMessageResponseDto>(responseForSender, NetworkMessageCode.MessageDeliveredCode, serverNetworkProvider);

            Console.WriteLine($"Код операции {NetworkMessageCode.SendMessageRequestCode}. Сообщение от пользователя с Id: {message.UserSenderId} доставлено пользователю с Id: {recipietUserId}.\n"
                + $"Текст сообщения: {message.Text}");
        }

        /// <summary>
        /// Обработать запрос на удаление сообщения
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Сетевой провайдеор на стороне сервера</param>
        /// <returns></returns>
        private async Task ProcessDeleteMessageRequest(NetworkMessage networkMessage, ServerNetworkProvider serverNetworkProvider)
        {
            DeleteMessageRequestDto deleteMessageRequestDto = SerializationHelper.Deserialize<DeleteMessageRequestDto>(networkMessage.Data);

            Console.WriteLine($"Запрос - Код операции: {networkMessage.Code}. Пользователь - Id: {deleteMessageRequestDto.UserId} отправил запрос на удаление сообщения - Id: {deleteMessageRequestDto.MessageId},\nв диалоге - Id {deleteMessageRequestDto.DialogId}");

            Message? message = _dbService.FindMessage(deleteMessageRequestDto.MessageId);

            NetworkMessage responseDeleteMessage;
            DeleteMessageResponse deleteMessageResponse;
            string resultOfOperation = "";

            if (message != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(deleteMessageRequestDto.DialogId, deleteMessageRequestDto.UserId);
                _dbService.DeleteMessage(message);

                DeleteMessageRequestForClient deleteMessageRequestForClient = new DeleteMessageRequestForClient(message.Id, deleteMessageRequestDto.DialogId);

                await BroadcastRequest<DeleteMessageRequestForClient, DeleteMessageRequestForClientDto>(deleteMessageRequestForClient, NetworkMessageCode.DeleteMessageRequestCode, deleteMessageRequestDto.UserId, interlocutorId, serverNetworkProvider);

                deleteMessageResponse = new DeleteMessageResponse(NetworkResponseStatus.Successful);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteMessageResponseCode}. Статус ответа: {NetworkResponseStatus.Successful}. Сообщение - Id: {message.Id} успешно удалено.\n"
                            + $"Текст сообщения: {message.Text}";
            }
            else
            {
                deleteMessageResponse = new DeleteMessageResponse(NetworkResponseStatus.Failed);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteMessageResponseCode}. Статус ответа: {NetworkResponseStatus.Failed}. Сообщение - Id: {message.Id} не существует в базе данных.";
            }

            await SendResponseToNetworkProvider<DeleteMessageResponse, DeleteMessageResponseDto>(deleteMessageResponse, NetworkMessageCode.DeleteMessageResponseCode, serverNetworkProvider);

            Console.WriteLine(resultOfOperation);
        }

        /// <summary>
        /// Обрабатывает запрос на удаление диалога
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="serverNetworkProvider"></param>
        private async Task ProcessDeleteDialogRequest(NetworkMessage networkMessage, ServerNetworkProvider serverNetworkProvider)
        {
            DeleteDialogRequestDto deleteDialogRequestDto = SerializationHelper.Deserialize<DeleteDialogRequestDto>(networkMessage.Data);
            Console.WriteLine($"Запрос - Код операции: {networkMessage.Code}. Пользователь - Id: {deleteDialogRequestDto.UserId} отправил запрос на удаление диалога - Id: {deleteDialogRequestDto.DialogId}.");

            Dialog? dialog = _dbService.FindDialog(deleteDialogRequestDto.DialogId);

            NetworkMessage responseNetworkMessage;
            DeleteDialogResponse deleteDialogResponse;
            string resultOfOperation = "";

            if (dialog != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(deleteDialogRequestDto.DialogId, deleteDialogRequestDto.UserId);

                _dbService.DeleteDialog(dialog);

                DeleteDialogRequestForClient deleteDialogRequestForClient = new DeleteDialogRequestForClient(deleteDialogRequestDto.DialogId);

                await BroadcastRequest<DeleteDialogRequestForClient, DeleteDialogRequestForClientDto>(deleteDialogRequestForClient, NetworkMessageCode.DeleteDialogRequestCode, deleteDialogRequestDto.UserId, interlocutorId, serverNetworkProvider);

                deleteDialogResponse = new DeleteDialogResponse(NetworkResponseStatus.Successful);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteDialogResponseCode}. Статус ответа: {NetworkResponseStatus.Successful}. Диалог - Id: {dialog.Id} успешно удален.\n";
            }
            else
            {
                deleteDialogResponse = new DeleteDialogResponse(NetworkResponseStatus.Failed);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteDialogResponseCode}. Статус ответа: {NetworkResponseStatus.Failed}. Диалог - Id: {dialog.Id} не существует в базе данных.\n";
            }

            await SendResponseToNetworkProvider<DeleteDialogResponse, DeleteDialogResponseDto>(deleteDialogResponse, NetworkMessageCode.DeleteDialogResponseCode, serverNetworkProvider);

            Console.WriteLine(resultOfOperation);
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
        private async Task SendResponseToNetworkProvider<TResponse, TResponseDto>(TResponse response, NetworkMessageCode code, ServerNetworkProvider serverNetworkProvider)
            where TResponseDto : class
        {
            NetworkMessage responseMessage = CreateNetworkMessage(response, out TResponseDto responseDto, code);

            byte[] responseDeleteDialogBytes = SerializationHelper.Serialize(responseMessage);

            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseDeleteDialogBytes);
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
        private byte[] CreateByteResponse<TResponse, TResponseDto>(TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            NetworkMessage responseMessage = CreateNetworkMessage(response, out TResponseDto responseDto, code);

            return SerializationHelper.Serialize(responseMessage);
        }

        /// <summary>
        /// Отправить ответ на запрос сетевому провайдеру
        /// </summary>
        /// <typeparam name="TResponse">Тип класса ответа</typeparam>
        /// <typeparam name="TResponseDto">Тип dto класса ответа</typeparam>
        /// <param name="response">Ответ</param>
        /// <param name="code">Код операции</param>
        /// <param name="networkProviderId">Id отправителя</param>
        /// <returns></returns>
        private async Task SendResponseToNetworkProvider<TResponse, TResponseDto>(TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            NetworkMessage responseMessage = CreateNetworkMessage(response, out TResponseDto responseDto, code);

            byte[] responseDeleteDialogBytes = SerializationHelper.Serialize(responseMessage);

            _conectionController

            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseDeleteDialogBytes);
        }

        /// <summary>
        /// Транслировать сообщение-уведомление об изменениях отправителю запроса и его собеседнику
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="userSenderId">Id ползователя-отправителя запроса</param>
        /// <param name="interlocutorId">Id собеседника того пользователя, который отправил запрос</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер</param>
        /// <returns></returns>
        private async Task BroadcastRequest<TRequest, TRequestDto>(TRequest request, NetworkMessageCode code, int userSenderId, int interlocutorId, ServerNetworkProvider serverNetworkProvider)
            where TRequestDto : class
        {
            NetworkMessage networkMessage = CreateNetworkMessage(request, out TRequestDto dto, code);

            byte[] requestDeleteDialogBytes = SerializationHelper.Serialize(networkMessage);
            await _userProxyList[userSenderId].BroadcastNetworkMessageAsync(requestDeleteDialogBytes, serverNetworkProvider);

            if (_userProxyList.ContainsKey(interlocutorId))
                await _userProxyList[interlocutorId].BroadcastNetworkMessageAsync(requestDeleteDialogBytes);
        }

        #region Методы, создающие ответы на запросы

        /// <summary>
        /// Создать ответы на запрос о регистрации
        /// Ответ для консоли типа String и ответ для клиента типа SignUpResponse
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер</param>
        /// <returns></returns>
        private SignUpResponse CreateSignUpResponse(User? user, /*ServerNetworkProvider serverNetworkProvider*/int senderId)
        {
            if (user != null)
            {
                _conectionController.AddNewSession(user.Id, senderId);
                //AddUserProxy(user.Id, serverNetworkProvider);

                return new SignUpResponse(user.Id, /*serverNetworkProvider.Id,*/ NetworkResponseStatus.Successful);
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

        #endregion Методы, создающие ответы на запросы
    }
}