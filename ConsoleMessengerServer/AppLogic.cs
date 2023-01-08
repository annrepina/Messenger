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

namespace ConsoleMessengerServer
{
    public class AppLogic : INetworkController, IDisposable
    {
        public Server Server { get; set; }

        /// <summary>
        /// Словарь, который содержит пары ключ - id сетевого провайдера и самого провайдера
        /// </summary>
        private Dictionary<int, ServerNetworkProvider> _ServerNetworkProviders;

        private readonly IMapper _mapper;

        public AppLogic()
        {
            Server = new Server(this);
            _ServerNetworkProviders = new Dictionary<int, ServerNetworkProvider>();

            DataBaseMapper mapper = DataBaseMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

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

            _ServerNetworkProviders.Add(networkProvider.Id, networkProvider);
        }

        public ServerNetworkProviderEntity AddNewClientToDb()
        {
            // ентити
            ServerNetworkProviderEntity? dbClient;

            using (var dbContext = new MessengerDbContext())
            {
                dbClient = new ServerNetworkProviderEntity();

                dbContext.NetworkProviders.Add(dbClient);

                dbContext.SaveChanges();
            }

            return dbClient;
        }

        #region INetworkHandler Implementation

        public void RunNewBackClient(TcpClient tcpClient)
        {
            ServerNetworkProvider networkProvider = new ServerNetworkProvider(tcpClient, this);

            ServerNetworkProviderEntity? dbClient = AddNewClientToDb();

            if (dbClient != null)
            {
                AddNewClientToDictionary(dbClient.Id, networkProvider);

                Task.Run(() => networkProvider.StartProcessingNetworkMessagesAsync());
            }
        }

        /// <summary>
        /// Отключить клиентов от сервера
        /// </summary>
        public void DisconnectClients()
        {
            DeleteNetworkProvidersFromDb();

            foreach (var client in _ServerNetworkProviders)
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
            if (_ServerNetworkProviders != null && _ServerNetworkProviders.Count > 0)
            {
                _ServerNetworkProviders.Remove(clientId);
            }

            using(var dbContext = new MessengerDbContext())
            {
                var provider = dbContext.NetworkProviders.First(prov => prov.Id == clientId);

                dbContext.NetworkProviders.Remove(provider);
            }
        }

        public void ProcessNetworkMessage(NetworkMessage message, int networkProviderId)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    {
                        RegisterNewAccount(message, networkProviderId);
                    }
                    break;
            }
        }

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            throw new NotImplementedException();
        }

        #endregion INetworkHandler Implementation

        public async Task RegisterNewAccount(NetworkMessage message, int networkProviderId)
        {
            RegistrationDto registrationDto = Deserializer.Deserialize<RegistrationDto>(message.Data);

            using (var dbContext = new MessengerDbContext())
            {
                // ищем есть ли аккаунт с таким номером уже в бд
                var res = await dbContext.Users.FirstOrDefaultAsync(user => user.PhoneNumber == registrationDto.PhoneNumber);

                NetworkMessage responseMessage = null;

                // если вернули null значит аккаунта под таким номером еще нет
                if (res == null)
                {
                    // Создаем акккунт и добавляем его в бд
                    User userData = _mapper.Map<User>(registrationDto);

                    ServerNetworkProviderEntity networkProvider = dbContext.NetworkProviders.FirstOrDefault(n => n.Id == networkProviderId);

                    if (networkProvider != null)
                    {
                        userData.NetworkProviders.Add(networkProvider);

                        dbContext.Users.Add(userData);

                        networkProvider.User = userData;

                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            throw;
                        }

                        NetworkProviderDto networkProviderDto = _mapper.Map<NetworkProviderDto>(networkProvider);

                        byte[] data = Serializer<NetworkProviderDto>.Serialize(networkProviderDto);

                        responseMessage = new NetworkMessage(data, NetworkMessage.OperationCode.SuccessfulRegistrationCode);

                        Console.WriteLine($"Код операции: {NetworkMessage.OperationCode.RegistrationCode}. Id: {userData.Id}. Телефон: {registrationDto.PhoneNumber}. Пароль: {registrationDto.Password}");
                    }

                    else
                        responseMessage = new NetworkMessage(null, NetworkMessage.OperationCode.RegistrationFailedCode);

                }//if

                else
                {
                    responseMessage = new NetworkMessage(null, NetworkMessage.OperationCode.RegistrationFailedCode);
                }

                await _ServerNetworkProviders[networkProviderId].Sender.SendNetworkMessageAsync(responseMessage);

            }//using
        }//method

        public void Dispose()
        {
            DisconnectClients();

            Server.Stop();
        }
    }
}
