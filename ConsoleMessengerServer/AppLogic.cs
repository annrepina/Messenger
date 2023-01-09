using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoLib;
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

namespace ConsoleMessengerServer
{
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

        public void AddNewClientToDictionary(int dbClientId, ServerNetworkProvider networkProvider)
        {
            Console.WriteLine($"{dbClientId} подключился ");

            networkProvider.Id = dbClientId;

            _userProxyList.Add(networkProvider.Id, networkProvider);
        }

        //public ServerNetworkProviderEntity AddNewClientToDb()
        //{
        //    // ентити
        //    ServerNetworkProviderEntity? dbClient;

        //    using (var dbContext = new MessengerDbContext())
        //    {
        //        dbClient = new ServerNetworkProviderEntity();

        //        dbContext.NetworkProviders.Add(dbClient);

        //        dbContext.SaveChanges();
        //    }

        //    return dbClient;
        //}

        #region INetworkHandler Implementation

        /// <summary>
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient)
        {
            ServerNetworkProvider networkProvider = new ServerNetworkProvider(tcpClient, this);

            Task.Run(() => networkProvider.ProcessNetworkMessagesAsync());
        }

        /// <summary>
        /// Отключить клиентов от сервера
        /// </summary>
        public void DisconnectClients()
        {
            DeleteNetworkProvidersFromDb();

            foreach (var client in _userProxyList)
            {
                client.Value.CloseConnection();
            }
        }

        /// <summary>
        /// Удалить сетевых провайдеров из базы данных
        /// </summary>
        public void DeleteNetworkProvidersFromDb()
        {
            using (var dbContext = new MessengerDbContext())
            {
                //SqlCommand sqlCommand = new SqlCommand("DELETE FROM @tableName");           
                //SqlParameter param = new SqlParameter("@tableName", nameof(dbContext.NetworkProviders));

                // с параметрами почему-то не работает, поэтому пока без них
                var a = dbContext.Database.ExecuteSqlRaw($"DELETE FROM {nameof(dbContext.NetworkProviders)}");

                dbContext.SaveChanges();
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

            using(var dbContext = new MessengerDbContext())
            {
                var provider = dbContext.NetworkProviders.First(prov => prov.Id == clientId);

                dbContext.NetworkProviders.Remove(provider);
            }
        }

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="message">Сетевое сообщение</param>
        /// <param name="serverNetworkProvider">Серверный сетевой провайдер</param>
        public void ProcessNetworkMessage(NetworkMessage message, ServerNetworkProvider serverNetworkProvider)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    {
                        RegisterNewUser(message, serverNetworkProvider);
                    }
                    break;
            }
        }

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            throw new NotImplementedException();
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

            int userId = TryAddNewUserToDb(registrationDto, serverNetworkProvider);

            NetworkMessage responseMessage = null;

            if(userId != 0)
            {
                CreateUserProxy(userId, serverNetworkProvider);

                responseMessage = CreateSuccessfulRegistrationResponse(serverNetworkProvider);


            }
            else
                responseMessage = new NetworkMessage(null, NetworkMessage.OperationCode.RegistrationFailedCode);

            await _userProxyList[networkProviderId].Sender.SendNetworkMessageAsync(responseMessage);

        }//method

        /// <summary>
        /// Попытаться добавить нового пользователя в базу данных, 
        /// вернуть id пользователя, при успешном добавлении, ноль - при неудаче
        /// </summary>
        /// <param name="registrationDto">DTO, который содержит данные о регистрации</param>
        /// <returns></returns>
        public int TryAddNewUserToDb(RegistrationDto registrationDto, ServerNetworkProvider serverNetworkProvider)
        {
            int userId = 0;

            using (var dbContext = new MessengerDbContext())
            {
                // ищем есть ли аккаунт с таким номером уже в бд
                var res = dbContext.Users.FirstOrDefault(user => user.PhoneNumber == registrationDto.PhoneNumber);

                // если вернули null значит аккаунта под таким номером еще нет
                if (res == null)
                {
                    User user = _mapper.Map<User>(registrationDto);
                    dbContext.Users.Add(user);

                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }

                    Console.WriteLine($"Код операции: {NetworkMessage.OperationCode.RegistrationCode}. {user.ToString()}");

                    userId = user.Id;

                }//if                    

            }//using

            return userId;
        }

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
        /// Создать ответ об успешной регистрации
        /// </summary>
        public NetworkMessage CreateSuccessfulRegistrationResponse(ServerNetworkProvider serverNetworkProvider)
        {
            NetworkProviderDto networkProviderDto = _mapper.Map<NetworkProviderDto>(serverNetworkProvider);

            byte[] data = Serializer<NetworkProviderDto>.Serialize(networkProviderDto);

            return new NetworkMessage(data, NetworkMessage.OperationCode.SuccessfulRegistrationCode);
        }



        public void Dispose()
        {
            DisconnectClients();

            Server.Stop();
        }
    }
}
