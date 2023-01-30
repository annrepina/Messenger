using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Responses;
using CommonLib.Dto.Requests;
using CommonLib.Dto.Responses;
using CommonLib.NetworkServices;
using CommonLib.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.RequestHandlers
{
    public class SignInRequestHandler : RequestHandler
    {
        public SignInRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            SignInResponse signInResponse = new SignInResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<SignInResponse, SignInResponseDto>(networkProvider, signInResponse, NetworkMessageCode.SignUpResponseCode);
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            SignInRequestDto signInRequestDto = SerializationHelper.Deserialize<SignInRequestDto>(networkMessage.Data);

            User? user = dbService.FindUserByPhoneNumber(signInRequestDto.PhoneNumber);

            SignInResponse signInResponse = CreateSignInResponse(dbService, user, signInRequestDto, networkProvider.Id);

            //NetworkMessage responseMessage = CreateNetworkMessage(signInResponse, out SignInResponseDto responseDto, NetworkMessageCode.SignInResponseCode);

            //byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            byte[] responseBytes = ByteArrayConverter<SignInResponse, SignInResponseDto>.Convert(signInResponse, NetworkMessageCode.SignInResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SignInResponseCode, signInRequestDto.ToString(), signInResponse.Status);

            return responseBytes;
        }

        private SignInResponse CreateSignInResponse(DbService dbService, User? user, SignInRequestDto signInRequestDto, int networkProviderId)
        {
            if (user != null)
            {
                if (user.Password == signInRequestDto.Password)
                {
                    List<Dialog> dialogs = dbService.FindDialogsByUser(user);
                    _conectionController.AddNewSession(user.Id, networkProviderId);

                    return new SignInResponse(user, dialogs, NetworkResponseStatus.Successful);
                }

                return new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.Password);
            }

            return new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.PhoneNumber);
        }
    }
}