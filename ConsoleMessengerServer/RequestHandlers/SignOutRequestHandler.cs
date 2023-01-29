using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Responses;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
using DtoLib.NetworkServices;
using DtoLib.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.RequestHandlers
{
    public class SignOutRequestHandler : RequestHandler
    {
        public SignOutRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            SignOutRequestDto signOutRequestDto = SerializationHelper.Deserialize<SignOutRequestDto>(networkMessage.Data);

            _conectionController.DisconnectUser(signOutRequestDto.UserId, networkProvider.Id);

            Response response = new Response(NetworkResponseStatus.Successful);

            //NetworkMessage responseMessage = CreateNetworkMessage(response, out ResponseDto dto, NetworkMessageCode.SignOutResponseCode);

            //byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            byte[] responseBytes = ByteArrayConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.SignOutResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SignOutResponseCode, signOutRequestDto.ToString(), response.Status);

            return responseBytes;
        }
    }
}
