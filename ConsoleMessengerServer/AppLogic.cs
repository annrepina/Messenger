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

namespace ConsoleMessengerServer
{
    public class AppLogic : INetworkMessageHandler
    {
        public Server Server { get; set; }

        private readonly IMapper _mapper;

        //public delegate Task NetworkMessageSent(NetworkMessage message);

        //public event NetworkMessageSent OnNetworkMessageSent;

        public AppLogic()
        {
            DataBaseMapper mapper = DataBaseMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        public async void ProcessNetworkMessage(NetworkMessage message, int clientId)
        {
           
            switch(message.CurrentCode)
            {
                case NetworkMessage.OperationCode.RegistrationCode:
                    {
                        UserAccountDto userAccountDto = new Deserializer<UserAccountDto>().Deserialize(message.Data);



                        using (var dbContext = new MessengerDbContext())
                        {
                            UserAccount userAcc = _mapper.Map<UserAccount>(userAccountDto);

                            var res = dbContext.UserAccounts.FirstOrDefault(acc => acc.Person.PhoneNumber == userAcc.Person.PhoneNumber);

                            // если вернули null значит аккаунта под таким номером еще нет
                            if(res == null)
                            {
                                dbContext.UserAccounts.Add(userAcc);
                                dbContext.SaveChanges();

                                UserAccountDto userAccountDtoForServer = _mapper.Map<UserAccountDto>(userAcc);

                                userAccountDtoForServer.Person.PhoneNumber = "+79999999999";

                                byte[] data = new Serializator<UserAccountDto>().Serialize(userAccountDtoForServer);

                                NetworkMessage responseMessage = new NetworkMessage(data, NetworkMessage.OperationCode.SuccessfulRegistrationCode);

                                await Server.SendMessageToViewModel(responseMessage, clientId);

                            }
                        }


                        //OnNetworkMessageSent?.Invoke(message);
                        Console.WriteLine($"Код операции: {NetworkMessage.OperationCode.RegistrationCode}. Телефон: {userAccountDto.Person.PhoneNumber}. Пароль: {userAccountDto.Password}");
                    }
                    break;
            }
        }
    }
}
