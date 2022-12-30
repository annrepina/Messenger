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
    public class AppLogic : INetworkHandler
    {
        public Server Server { get; set; }

        /// <summary>
        /// Словарь, который содержит пары ключ - id Клиента и сам клиент
        /// </summary>
        private Dictionary<int, ServerNetworkProvider> _clients;

        private readonly IMapper _mapper;

        //public delegate Task NetworkMessageSent(NetworkMessage message);

        //public event NetworkMessageSent OnNetworkMessageSent;

        public AppLogic()
        {
            Server = new Server(this);
            _clients = new Dictionary<int, ServerNetworkProvider>();

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

        public void AddNewClientToDictionary(int dbClientId, ServerNetworkProvider client)
        {
            Console.WriteLine($"{dbClientId} подключился ");

            client.Id = dbClientId;

            _clients.Add(client.Id, client);
        }

        public Client AddNewClientToDb()
        {
            // ентити
            Client? dbClient;

            using (var dbContext = new MessengerDbContext())
            {
                dbClient = new Client();

                dbContext.Clients.Add(dbClient);

                dbContext.SaveChanges();
            }

            return dbClient;
        }

        #region INetworkHandler Implementation

        public async Task RunNewBackClientAsync(TcpClient tcpClient)
        {
            ServerNetworkProvider client = new ServerNetworkProvider(tcpClient);

            client.NetworkHandler = this;

            Client? dbClient = AddNewClientToDb();

            if (dbClient != null)
            {
                AddNewClientToDictionary(dbClient.Id, client);

                await client.ProcessDataAsync();
            }
        }

        public void DisconnectClients()
        {
            foreach (var client in _clients)
            {
                client.Value.CloseConnection();
            }
        }

        public void RemoveClient(int clientId)
        {
            if (_clients != null && _clients.Count > 0)
            {
                _clients.Remove(clientId);
            }
        }

        public async void ProcessNetworkMessage(NetworkMessage message, int clientId)
        {
            switch (message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    {
 
                    }
                    break;
            }
        }

        public void ProcessNetworkMessage(NetworkMessage message)
        {
            throw new NotImplementedException();
        }

        #endregion INetworkHandler Implementation

        public async Task RegisterNewAccount(NetworkMessage message, int clientId)
        {
            RegistrationAuthentificationDto userAccountRegistrationDto = new Deserializer<RegistrationAuthentificationDto>().Deserialize(message.Data);

            using (var dbContext = new MessengerDbContext())
            {
                // ищем есть ли аккаунт с таким номером уже в бд
                var res = await dbContext.UserAccounts.FirstOrDefaultAsync(acc => acc.Person.PhoneNumber == userAccountRegistrationDto.PhoneNumber);

                NetworkMessage responseMessage = null;

                // если вернули null значит аккаунта под таким номером еще нет
                if (res == null)
                {
                    // Создаем акккунт и добавляем его в бд
                    UserAccount userAcc = _mapper.Map<UserAccount>(userAccountRegistrationDto);

                    // мапим клиента
                    Client client = _mapper.Map<Client>(_clients[clientId]);

                    userAcc.Clients.Add(client);

                    dbContext.UserAccounts.Add(userAcc);

                    dbContext.SaveChanges();

                    SuccessfulRegistrationDto successfulRegistrDto = _mapper.Map<SuccessfulRegistrationDto>(userAcc);

                    byte[] data = new Serializer<SuccessfulRegistrationDto>().Serialize(successfulRegistrDto);

                    responseMessage = new NetworkMessage(data, NetworkMessage.OperationCode.SuccessfulRegistrationCode);

                    Console.WriteLine($"Код операции: {NetworkMessage.OperationCode.RegistrationCode}. Id: {userAcc.Id}. Телефон: {userAccountRegistrationDto.PhoneNumber}. Пароль: {userAccountRegistrationDto.Password}");

                }//if

                else
                {
                    responseMessage = new NetworkMessage(null, NetworkMessage.OperationCode.RegistrationFailedCode);
                }

                await _clients[clientId].Sender.SendNetworkMessage(responseMessage);

            }//using
        }//method
    }
}
