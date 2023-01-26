using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
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
    public class SignUpRequestHandler : RequestHandler
    {
        public SignUpRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {

        }

        protected override void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            SignUpResponse errorResponse = new SignUpResponse(NetworkResponseStatus.FatalError);
            SendError<SignUpResponse, SignUpResponseDto>(networkProvider, errorResponse, NetworkMessageCode.SignUpResponseCode);
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            SignUpRequestDto signUpRequestDto = SerializationHelper.Deserialize<SignUpRequestDto>(networkMessage.Data);

            User? user = dbService.AddNewUser(signUpRequestDto);

            SignUpResponse signUpResponse = CreateSignUpResponse(user, networkProvider.Id);

            NetworkMessage responseMessage = CreateNetworkMessage(signUpResponse, out SignUpResponseDto responseDto, NetworkMessageCode.SignUpResponseCode);

            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);

            PrintReport(networkProvider.Id, networkMessage.Code, responseMessage.Code, signUpRequestDto.ToString(), signUpResponse.Status);

            return responseBytes;
        }

        private SignUpResponse CreateSignUpResponse(User? user, int networkProviderId)
        {
            if (user != null)
            {
                _conectionController.AddNewSession(user.Id, networkProviderId);

                return new SignUpResponse(user.Id, NetworkResponseStatus.Successful);
            }

            return new SignUpResponse(NetworkResponseStatus.Failed);
        }
    }
}