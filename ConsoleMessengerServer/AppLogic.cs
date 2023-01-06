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

namespace ConsoleMessengerServer
{
    public class AppLogic : INetworkController
    {
        public Server Server { get; set; }

        /// <summary>
        /// Словарь, который содержит пары ключ - id сетевого провайдера и самого провайдера
        /// </summary>
        private Dictionary<int, ServerNetworkProvider> _ServerNetworkProviders;

        private readonly IMapper _mapper;

        //public delegate Task NetworkMessageSent(NetworkMessage message);

        //public event NetworkMessageSent OnNetworkMessageSent;

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
                await Task.Run(() => Server.ListenIncomingConnectionsAsync());
            }
            catch (Exception ex)
            {
                Server.DisconnectClients();
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

                dbContext.Clients.Add(dbClient);

                dbContext.SaveChanges();
            }

            return dbClient;
        }

        #region INetworkHandler Implementation

        public async Task RunNewBackClientAsync(TcpClient tcpClient)
        {
            ServerNetworkProvider networkProvider = new ServerNetworkProvider(tcpClient, this);

            ServerNetworkProviderEntity? dbClient = AddNewClientToDb();

            if (dbClient != null)
            {
                AddNewClientToDictionary(dbClient.Id, networkProvider);

                await networkProvider.StartProcessingNetworkMessagesAsync();
            }
        }

        public void DisconnectClients()
        {
            foreach (var client in _ServerNetworkProviders)
            {
                client.Value.CloseConnection();
            }
        }

        public void RemoveClient(int clientId)
        {
            if (_ServerNetworkProviders != null && _ServerNetworkProviders.Count > 0)
            {
                _ServerNetworkProviders.Remove(clientId);
            }
        }

        public async Task ProcessNetworkMessageAsync(NetworkMessage message, int networkProviderId)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    {
                        await RegisterNewAccount(message, networkProviderId);
                    }
                    break;
            }
        }

        public async Task ProcessNetworkMessageAsync(NetworkMessage message)
        {
            throw new NotImplementedException();
        }

        #endregion INetworkHandler Implementation

        public async Task RegisterNewAccount(NetworkMessage message, int networkProviderId)
        {
            RegistrationAuthentificationDto userAccountRegistrationDto = Deserializer.Deserialize<RegistrationAuthentificationDto>(message.Data);

            using (var dbContext = new MessengerDbContext())
            {
                // ищем есть ли аккаунт с таким номером уже в бд
                var res = await dbContext.UserAccounts.FirstOrDefaultAsync(acc => acc.Person.PhoneNumber == userAccountRegistrationDto.PhoneNumber);

                NetworkMessage responseMessage = null;

                // если вернули null значит аккаунта под таким номером еще нет
                if (res == null)
                {
                    // Создаем акккунт и добавляем его в бд
                    UserData userAcc = _mapper.Map<UserData>(userAccountRegistrationDto);

                    // мапим клиента
                    ServerNetworkProviderEntity networkProvider = _mapper.Map<ServerNetworkProviderEntity>(_ServerNetworkProviders[networkProviderId]);

                    userAcc.NetworkProviders.Add(networkProvider);

                    dbContext.UserAccounts.Add(userAcc);

                    dbContext.SaveChanges();

                    NetworkProviderDto networkProviderDto = _mapper.Map<NetworkProviderDto>(networkProvider);

                    byte[] data = Serializer<NetworkProviderDto>.Serialize(networkProviderDto);

                    responseMessage = new NetworkMessage(data, NetworkMessage.OperationCode.SuccessfulRegistrationCode);

                    Console.WriteLine($"Код операции: {NetworkMessage.OperationCode.RegistrationCode}. Id: {userAcc.Id}. Телефон: {userAccountRegistrationDto.PhoneNumber}. Пароль: {userAccountRegistrationDto.Password}");

                }//if

                else
                {
                    responseMessage = new NetworkMessage(null, NetworkMessage.OperationCode.RegistrationFailedCode);
                }

                await _ServerNetworkProviders[networkProviderId].Sender.SendNetworkMessageAsync(responseMessage);

            }//using
        }//method
    }
}
