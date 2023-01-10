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
        /// <param name="message">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        public async Task ProcessNetworkMessage(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    RegisterNewUser(message, serverNetworkProvider);

                    break;

                case NetworkMessage.OperationCode.SearchUserCode:
                    await SearchUser(message, serverNetworkProvider);

                    break;

            }
        }

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            switch (message.CurrentCode)
            {


            }
        }

        #endregion INetworkHandler Implementation

        /// <summary>
        /// Зарегистрировать нового пользователя в мессенджере
        /// </summary>
        /// <param name="message"></param>
        /// <param name="networkProviderId"></param>
        /// <returns></returns>
        public async Task RegisterNewUser(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            RegistrationDto registrationDto = Deserializer.Deserialize<RegistrationDto>(message.Data);
            User? user = _dbService.TryAddNewUser(registrationDto);

            NetworkMessage responseMessage = null;

            string resultOfOperation = "";

            if(user != null)
            {
                CreateUserProxy(user.Id, serverNetworkProvider);

                responseMessage = CreateResponse<ServerNetworkProvider, NetworkProviderDto>(serverNetworkProvider, out NetworkProviderDto dto, NetworkMessage.OperationCode.SuccessfulRegistrationCode);

                await _userProxyList[user.Id].BroadcastNetworkMessageAsync(responseMessage);

                resultOfOperation = $"Код операции: {NetworkMessage.OperationCode.RegistrationCode}. {user.ToString()}";
            }
            else
            {
                responseMessage = new NetworkMessage(null, NetworkMessage.OperationCode.RegistrationFailedCode);

                await serverNetworkProvider.Sender.SendNetworkMessageAsync(responseMessage);

                resultOfOperation = $"Код операции: {NetworkMessage.OperationCode.RegistrationFailedCode}. {user.ToString()}";
            }

            Console.WriteLine(resultOfOperation);
        }//method

        /// <summary>
        /// Создать агрегатора всех подключений определенного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        public void CreateUserProxy(int userId, ServerNetworkProvider serverNetworkProvider)
        {
            UserProxy userProxy = new UserProxy(userId);
            userProxy.AddConnection(serverNetworkProvider);

            _userProxyList.Add(userId, userProxy);
        }

        /// <summary>
        /// Посик пользователя в мессенджере
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task SearchUser(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            UserSearchRequestDto searchRequestDto = Deserializer.Deserialize<UserSearchRequestDto>(message.Data);

            List<User>? usersList = _dbService.TrySearchUsers(searchRequestDto);

            NetworkMessage responseMessage = null;
            string resultOfOperation = "";

            if (usersList != null)
            {
                UserSearchResult userSearchResult = new UserSearchResult(usersList);

                responseMessage = CreateResponse<UserSearchResult, UserSearchResultDto>(userSearchResult, out UserSearchResultDto dto, NetworkMessage.OperationCode.SuccessfulSearchCode);

                await serverNetworkProvider.Sender.SendNetworkMessageAsync(responseMessage);

                resultOfOperation = $"Код операции: {NetworkMessage.OperationCode.SuccessfulSearchCode}. Список составлен на основе запросов - Имя: {searchRequestDto.Name}. Телефон: {searchRequestDto.PhoneNumber}";
            }
            else
            {
                responseMessage = new NetworkMessage(null, NetworkMessage.OperationCode.SearchFailedCode);
                resultOfOperation = $"Код операции: {NetworkMessage.OperationCode.SearchFailedCode}. Поиск на основе запросов - Имя: {searchRequestDto.Name}. Телефон: {searchRequestDto.PhoneNumber} не дал результатов";
            }

            Console.WriteLine(resultOfOperation);
        }

        #region Методы создания ответов на запросы

        ///// <summary>
        ///// Создать ответное сетевое сообщение об успешной регистрации
        ///// </summary>
        //public NetworkMessage CreateSuccessfulRegistrationResponse(ServerNetworkProvider serverNetworkProvider)
        //{
        //    NetworkProviderDto networkProviderDto = _mapper.Map<NetworkProviderDto>(serverNetworkProvider);
        //    byte[] data = Serializer<NetworkProviderDto>.Serialize(networkProviderDto);
        //    return new NetworkMessage(data, NetworkMessage.OperationCode.SuccessfulRegistrationCode);
        //}

        /// <summary>
        /// Обобщенный метод создания ответного сетевого сообщения
        /// </summary>
        /// <typeparam name="Tsource">Тип данных источника для мапинга на dto</typeparam>
        /// <typeparam name="Tdto">Тип конкретного dto</typeparam>
        /// <param name="tsource">Объект - сточник для мапинга на dto</param>
        /// <param name="dto">Объект, представляющий dto</param>
        /// <param name="operationCode">Код операции</param>
        /// <returns></returns>
        public NetworkMessage CreateResponse<Tsource, Tdto>(Tsource tsource, out Tdto dto, NetworkMessage.OperationCode operationCode)
            where Tdto : class
        {
            dto = _mapper.Map<Tdto>(tsource);

            byte[] data = Serializer<Tdto>.Serialize(dto);

            var message = new NetworkMessage(data, operationCode);

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
