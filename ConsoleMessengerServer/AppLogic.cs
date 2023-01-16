using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleMessengerServer.Net;
using DtoLib.Serialization;
using DtoLib.Dto;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using AutoMapper;
using ConsoleMessengerServer.Entities.Mapping;
using DtoLib.NetworkInterfaces;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ConsoleMessengerServer.Net.Interfaces;
using DtoLib.NetworkServices;
using ConsoleMessengerServer.Responses;
using System.Formats.Asn1;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
using Microsoft.Data.SqlClient.DataClassification;
using ConsoleMessengerServer.Requests;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Класс, который отвечает за логи
    /// </summary>
    public class AppLogic : INetworkController, IDisposable
    {
        /// <summary>
        /// Словарь, который содержит пары: ключ - id агрегатора соединений пользователя 
        /// и значение - самого агрегатора
        /// </summary>
        private Dictionary<int, UserProxy> _userProxyList;

        /// <summary>
        /// Маппер для мапинга ентити на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис для работы с базами данных
        /// </summary>
        private readonly DbService _dbService;

        /// <summary>
        /// Сервер, который прослушивает входящие подключения
        /// </summary>
        public Server Server { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public AppLogic()
        {
            Server = new Server(this);
            _userProxyList = new Dictionary<int, UserProxy>();

            DataBaseMapper mapper = DataBaseMapper.GetInstance();
            _mapper = mapper.CreateIMapper();

            _dbService = new DbService();
        }

        /// <summary>
        /// Асинхронный метод - начать прослушивание входящих подключений
        /// </summary>
        /// <returns></returns>
        public async Task StartListeningConnectionsAsync()
        {
            try
            {
                await Server.ListenIncomingConnectionsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region INetworkHandler Implementation

        /// <summary>
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient)
        {
            ServerNetworkProvider networkProvider = new ServerNetworkProvider(tcpClient, this);

            Console.WriteLine($"Клиент {networkProvider.Id} подключился ");

            Task.Run(() => networkProvider.ProcessNetworkMessagesAsync());
        }

        /// <summary>
        /// Отключить клиентов от сервера
        /// </summary>
        public void DisconnectClients()
        {
            //DeleteNetworkProvidersFromDb();

            foreach (var client in _userProxyList.Values)
            {
                client.CloseAll();
            }
        }

        /// <summary>
        /// Отключить конкретного клиента
        /// </summary>
        /// <param name="clientId">Идентификатор клиента</param>
        public void DisconnectClient(int clientId)
        {
            if (_userProxyList != null && _userProxyList.Count > 0)
            {
                _userProxyList.Remove(clientId);
            }
        }

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        public async Task ProcessNetworkMessage(NetworkMessage networkMessage, ServerNetworkProvider serverNetworkProvider)
        {
            switch (networkMessage.Code)
            {
                case NetworkMessageCode.SignUpRequestCode:
                    ProcessSignUpRequest(networkMessage, serverNetworkProvider);
                    break;

                case NetworkMessageCode.SignInRequestCode:
                    ProcessSignInRequestAsync(networkMessage, serverNetworkProvider);
                    break;

                case NetworkMessageCode.SearchUserRequestCode:
                    ProcessSearchUserMessage(networkMessage, serverNetworkProvider);
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



        public void ProcessNetworkMessage(NetworkMessage message)
        {
            //switch (networkMessage.Code)
            //{

            //}
        }


        #endregion INetworkHandler Implementation

        /// <summary>
        /// Зарегистрировать нового пользователя в мессенджере
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        /// <returns></returns>
        public async Task ProcessSignUpRequest(NetworkMessage networkMessage, ServerNetworkProvider serverNetworkProvider)
        {
            SignUpRequestDto registrationDto = SerializationHelper.Deserialize<SignUpRequestDto>(networkMessage.Data);
            User? user = _dbService.AddNewUser(registrationDto);

            string resultOfOperation;
            SignUpResponse signUpResponse;

            if (user != null)
            {
                AddUserProxy(user.Id, serverNetworkProvider);

                signUpResponse = new SignUpResponse(user.Id, serverNetworkProvider.Id, NetworkResponseStatus.Successful);
                resultOfOperation = $"Код операции: {NetworkMessageCode.SignUpRequestCode}. Статус операции: {NetworkResponseStatus.Successful}. Пользователь: {user.ToString()}.";
            }
            else
            {
                signUpResponse = new SignUpResponse(NetworkResponseStatus.Failed);
                resultOfOperation = $"Код операции: {NetworkMessageCode.SignUpRequestCode}. Статус операции: {NetworkResponseStatus.Failed}. Данные о регистрации: {registrationDto.ToString()}."; ;
            }

            NetworkMessage responseMessage = CreateResponse(signUpResponse, out SignUpResponseDto dto, NetworkMessageCode.SignUpResponseCode);
            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseBytes);

            Console.WriteLine(resultOfOperation);
        }//method

        /// <summary>
        /// Обработать запрос на вход в мессенджер
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="serverNetworkProvider"></param>
        private async Task ProcessSignInRequestAsync(NetworkMessage networkMessage, ServerNetworkProvider serverNetworkProvider)
        {
            SignInRequestDto signInRequestDto = SerializationHelper.Deserialize<SignInRequestDto>(networkMessage.Data);

            //bool result = _dbService.FindUserByPhoneNumber(signInRequestDto.PhoneNumber);
            User? user = _dbService.FindUserByPhoneNumber(signInRequestDto.PhoneNumber);

            SignInResponse signInResponse;
            string resultOfOperation;

            // номер телефона есть
            if(user != null)
            {
                if(user.Password == signInRequestDto.Password)
                {
                    List<Dialog> dialogs = _dbService.FindDialogsByUser(user);

                    signInResponse = new SignInResponse(user, dialogs, NetworkResponseStatus.Successful);
                    resultOfOperation = $"Код операции: {NetworkMessageCode.SignInRequestCode}. Статус операции: {NetworkResponseStatus.Successful}. Данные входа: Телефон - {signInRequestDto.PhoneNumber}.";

                    AddUserProxy(user.Id, serverNetworkProvider);
                }
                else
                {
                    signInResponse = new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.Password);
                    resultOfOperation = $"Код операции: {NetworkMessageCode.SignInRequestCode}. Статус операции: {NetworkResponseStatus.Successful}. " +
                                        $"\nКонтекст ошибки: {SignInFailContext.Password}. Данные входа: Телефон - {signInRequestDto.PhoneNumber}.";
                }
            }
            else
            {
                signInResponse = new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.PhoneNumber);
                resultOfOperation = $"Код операции: {NetworkMessageCode.SignInRequestCode}. Статус операции: {NetworkResponseStatus.Failed}." +
                                    $"\nКонтекст ошибки: {SignInFailContext.PhoneNumber}. Данные входа: Телефон - {signInRequestDto.PhoneNumber}.";
            }

            NetworkMessage responseMessage = CreateResponse(signInResponse, out SignInResponseDto dto, NetworkMessageCode.SignInResponseCode);
            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseBytes);

            Console.WriteLine(resultOfOperation);
        }

        /// <summary>
        /// Создать агрегатора всех подключений определенного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        public void AddUserProxy(int userId, ServerNetworkProvider serverNetworkProvider)
        {
            if(_userProxyList.ContainsKey(userId))
            {
                _userProxyList[userId].AddConnection(serverNetworkProvider);
            }
            else
            {
                UserProxy userProxy = new UserProxy(userId);
                userProxy.AddConnection(serverNetworkProvider);

                _userProxyList.Add(userId, userProxy);
            }
        }

        /// <summary>
        /// Посик пользователя в мессенджере
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task ProcessSearchUserMessage(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            UserSearchRequestDto searchRequestDto = SerializationHelper.Deserialize<UserSearchRequestDto>(message.Data);

            List<User> usersList = _dbService.FindListOfUsers(searchRequestDto);

            UserSearchResponse userSearchResult;
            string resultOfOperation = "";

            if (usersList?.Count > 0)
            {
                userSearchResult = new UserSearchResponse(usersList, NetworkResponseStatus.Successful);
                resultOfOperation = $"Код операции: {NetworkMessageCode.SearchUserRequestCode}. Статус ответа: {NetworkResponseStatus.Successful}. Список составлен на основе запросов - Имя: {searchRequestDto.Name}. Телефон: {searchRequestDto.PhoneNumber}";
            }
            else
            {
                userSearchResult = new UserSearchResponse(usersList, NetworkResponseStatus.Failed);
                resultOfOperation = $"Код операции: {NetworkMessageCode.SearchUserRequestCode}. Статус ответа: {NetworkResponseStatus.Failed}. Поиск на основе запросов - Имя: {searchRequestDto.Name}. Телефон: {searchRequestDto.PhoneNumber} не дал результатов";
            }

            NetworkMessage responseMessage = CreateResponse(userSearchResult, out UserSearchResponseDto dto, NetworkMessageCode.SearchUserResponseCode);
            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseBytes);

            Console.WriteLine(resultOfOperation);
        }

        /// <summary>
        /// Обработать запрос на создание нового диалога
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер на стороне сервера</param>
        private async Task ProcessCreateDialogRequest(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            CreateDialogRequestDto createDialogRequestDto = SerializationHelper.Deserialize<CreateDialogRequestDto>(message.Data);

            Dialog dialog = _dbService.CreateDialog(createDialogRequestDto);
            int senderId = dialog.Messages.First().UserSenderId;
            int recipientId = dialog.Users.First(user => user.Id != senderId).Id;

            CreateDialogResponse createDialogResponse = _mapper.Map<CreateDialogResponse>(dialog);

            NetworkMessage responseMessageForSenderServer = CreateResponse(createDialogResponse, out CreateDialogResponseDto dto, NetworkMessageCode.CreateDialogResponseCode);
            byte[] responseMessageForSenderServerBytes = SerializationHelper.Serialize(responseMessageForSenderServer);
            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseMessageForSenderServerBytes);

            NetworkMessage responseWithFullDialog = CreateResponse(dialog, out DialogDto dialogDto, NetworkMessageCode.CreateDialogRequestCode);
            byte[] responseWithFullDialogBytes = SerializationHelper.Serialize(responseWithFullDialog);
            await _userProxyList[senderId].BroadcastNetworkMessageAsync(responseWithFullDialogBytes, serverNetworkProvider);

            if (_userProxyList.ContainsKey(recipientId))
                await _userProxyList[recipientId].BroadcastNetworkMessageAsync(responseWithFullDialogBytes);

            Console.WriteLine($"Код операции: {NetworkMessageCode.CreateDialogResponseCode}. Диалог с Id {createDialogResponse.DialogId} создан."); 
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
            NetworkMessage newMessage = CreateResponse(sendMessageRequest, out SendMessageRequestDto dto, NetworkMessageCode.SendMessageRequestCode);
            byte[] newMessageBytes = SerializationHelper.Serialize(newMessage);
            await _userProxyList[message.UserSenderId].BroadcastNetworkMessageAsync(newMessageBytes, serverNetworkProvider);

            if(_userProxyList.ContainsKey(recipietUserId))
                await _userProxyList[recipietUserId].BroadcastNetworkMessageAsync(newMessageBytes);

            SendMessageResponse response = new SendMessageResponse(message.Id);
            NetworkMessage responseMessage = CreateResponse(response, out SendMessageResponseDto responseDto, NetworkMessageCode.MessageDeliveredCode);
            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseBytes);

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
                NetworkMessage deleteMessageRequest = CreateResponse(deleteMessageRequestForClient, out DeleteMessageRequestForClientDto dto, NetworkMessageCode.DeleteMessageRequestCode);

                byte[] requestDeleteMessageBytes = SerializationHelper.Serialize(deleteMessageRequest);
                await _userProxyList[deleteMessageRequestDto.UserId].BroadcastNetworkMessageAsync(requestDeleteMessageBytes, serverNetworkProvider);

                if (_userProxyList.ContainsKey(interlocutorId))
                    await _userProxyList[interlocutorId].BroadcastNetworkMessageAsync(requestDeleteMessageBytes);

                deleteMessageResponse = new DeleteMessageResponse(NetworkResponseStatus.Successful);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteMessageResponseCode}. Статус ответа: {NetworkResponseStatus.Successful}. Сообщение - Id: {message.Id} успешно удалено.\n"
                            + $"Текст сообщения: {message.Text}";
            }
            else
            {
                deleteMessageResponse = new DeleteMessageResponse(NetworkResponseStatus.Failed);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteMessageResponseCode}. Статус ответа: {NetworkResponseStatus.Failed}. Сообщение - Id: {message.Id} не существует в базе данных.";
            }

            responseDeleteMessage = CreateResponse(deleteMessageResponse, out DeleteMessageResponse responseDto, NetworkMessageCode.DeleteMessageResponseCode);
            byte[] responseDeleteMessageBytes = SerializationHelper.Serialize(responseDeleteMessage);    
            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseDeleteMessageBytes);

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

            if(dialog != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(deleteDialogRequestDto.DialogId, deleteDialogRequestDto.UserId);

                _dbService.DeleteDialog(dialog);

                DeleteDialogRequestForClient deleteDialogRequestForClient = new DeleteDialogRequestForClient(deleteDialogRequestDto.DialogId);
                NetworkMessage deleteDialogRequest = CreateResponse(deleteDialogRequestForClient, out DeleteDialogRequestForClientDto dto, NetworkMessageCode.DeleteDialogRequestCode);

                byte[] requestDeleteDialogBytes = SerializationHelper.Serialize(deleteDialogRequest);
                await _userProxyList[deleteDialogRequestDto.UserId].BroadcastNetworkMessageAsync(requestDeleteDialogBytes, serverNetworkProvider);

                if (_userProxyList.ContainsKey(interlocutorId))
                    await _userProxyList[interlocutorId].BroadcastNetworkMessageAsync(requestDeleteDialogBytes);

                deleteDialogResponse = new DeleteDialogResponse(NetworkResponseStatus.Successful);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteDialogResponseCode}. Статус ответа: {NetworkResponseStatus.Successful}. Диалог - Id: {dialog.Id} успешно удален.\n";
            }
            else
            {
                deleteDialogResponse = new DeleteDialogResponse(NetworkResponseStatus.Failed);
                resultOfOperation = $"Ответ - Код операции: {NetworkMessageCode.DeleteDialogResponseCode}. Статус ответа: {NetworkResponseStatus.Failed}. Диалог - Id: {dialog.Id} не существует в базе данных.\n";
            }

            responseNetworkMessage = CreateResponse(deleteDialogResponse, out DeleteDialogResponse responseDto, NetworkMessageCode.DeleteDialogResponseCode);
            byte[] responseDeleteDialogBytes = SerializationHelper.Serialize(responseNetworkMessage);
            await serverNetworkProvider.Transmitter.SendNetworkMessageAsync(responseDeleteDialogBytes);

            Console.WriteLine(resultOfOperation);
        }

        #region Методы создания ответов на запросы

        /// <summary>
        /// Обобщенный метод создания ответного сетевого сообщения
        /// </summary>
        /// <typeparam name="Tsource">Тип данных источника для мапинга на dto</typeparam>
        /// <typeparam name="Tdto">Тип конкретного dto</typeparam>
        /// <param name="tsource">Объект - сточник для мапинга на dto</param>
        /// <param name="dto">Объект, представляющий dto</param>
        /// <param name="code">Код операции</param>
        /// <returns></returns>
        public NetworkMessage CreateResponse<Tsource, Tdto>(Tsource tsource, out Tdto dto, NetworkMessageCode code)
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

        #endregion Методы создания ответов на запросы

        public void Dispose()
        {
            DisconnectClients();

            Server.Stop();
        }
    }
}