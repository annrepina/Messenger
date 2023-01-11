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
            switch (message.Code)
            {
                case NetworkMessageCode.RegistrationCode:
                    ProcessRegistrationNewUser(message, serverNetworkProvider);

                    break;

                case NetworkMessageCode.SearchUserCode:
                    ProcessSearchUser(message, serverNetworkProvider);

                    break;

            }
        }

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            switch (message.Code)
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
        public async Task ProcessRegistrationNewUser(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            RegistrationDto registrationDto = Deserializer.Deserialize<RegistrationDto>(message.Data);
            User? user = _dbService.TryAddNewUser(registrationDto);

            string resultOfOperation;
            NetworkMessage responseMessage;

            if (user != null)
            {
                CreateUserProxy(user.Id, serverNetworkProvider);

                SuccessfulRegistrationResponse registrationResponse = new SuccessfulRegistrationResponse(user.Id, serverNetworkProvider.Id);
                responseMessage = CreateResponse(registrationResponse, out SuccessfulRegistrationResponseDto dto, NetworkMessageCode.SuccessfulRegistrationCode);

                resultOfOperation = $"Код операции: {NetworkMessageCode.RegistrationCode}. {user.ToString()}";
            }
            else
            {
                responseMessage = new NetworkMessage(null, NetworkMessageCode.RegistrationFailedCode);

                resultOfOperation = $"Код операции: {NetworkMessageCode.RegistrationFailedCode}. {user.ToString()}";
            }

            await serverNetworkProvider.Sender.SendNetworkMessageAsync(responseMessage);

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
        private async Task ProcessSearchUser(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            UserSearchRequestDto searchRequestDto = Deserializer.Deserialize<UserSearchRequestDto>(message.Data);

            List<User> usersList = _dbService.TrySearchUsers(searchRequestDto);

            NetworkMessage responseMessage = null;
            string resultOfOperation = "";

            if (usersList?.Count > 0)
            {
                UserSearchResult userSearchResult = new UserSearchResult(usersList);

                responseMessage = CreateResponse(userSearchResult, out UserSearchResultDto dto, NetworkMessageCode.SuccessfulSearchCode);

                resultOfOperation = $"Код операции: {NetworkMessageCode.SuccessfulSearchCode}. Список составлен на основе запросов - Имя: {searchRequestDto.Name}. Телефон: {searchRequestDto.PhoneNumber}";
            }
            else
            {
                responseMessage = new NetworkMessage(null, NetworkMessageCode.SearchFailedCode);

                resultOfOperation = $"Код операции: {NetworkMessageCode.SearchFailedCode}. Поиск на основе запросов - Имя: {searchRequestDto.Name}. Телефон: {searchRequestDto.PhoneNumber} не дал результатов";
            }

            await serverNetworkProvider.Sender.SendNetworkMessageAsync(responseMessage);

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

            byte[] data = Serializer<Tdto>.Serialize(dto);

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
