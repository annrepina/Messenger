using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Requests;
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
    public class SendMessageRequestHandler : RequestHandler
    {
        public SendMessageRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            throw new NotImplementedException();
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            SendMessageRequestDto sendMessageRequestDto = SerializationHelper.Deserialize<SendMessageRequestDto>(networkMessage.Data);

            Message message = dbService.AddMessage(sendMessageRequestDto);
            SendMessageResponse response = new SendMessageResponse(message.Id, NetworkResponseStatus.Successful);

            BroadcastSendMessageRequest(dbService, message, networkProvider.Id);

            //NetworkMessage responseMessage = CreateNetworkMessage(response, out SendMessageResponseDto responseDto, NetworkMessageCode.SendMessageResponseCode);

            //byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            byte[] responseBytes = ByteArrayConverter<SendMessageResponse, SendMessageResponseDto>.Convert(response, NetworkMessageCode.SendMessageResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SendMessageResponseCode, sendMessageRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        private void BroadcastSendMessageRequest(DbService dbService, Message message, int networkProviderId)
        {
            int recipietUserId = dbService.GetRecipientUserId(message);

            SendMessageRequest sendMessageRequest = new SendMessageRequest(message, message.DialogId);

            //NetworkMessage requestMessage = CreateNetworkMessage(sendMessageRequest, out SendMessageRequestDto requestDto, NetworkMessageCode.SendMessageRequestCode);

            //byte[] requestBytes = SerializationHelper.Serialize(requestMessage);
            byte[] requestBytes = ByteArrayConverter<SendMessageRequest, SendMessageRequestDto>.Convert(sendMessageRequest, NetworkMessageCode.SendMessageRequestCode);

            _conectionController.BroadcastToSenderAsync(requestBytes, message.UserSenderId, networkProviderId);
            _conectionController.BroadcastToInterlocutorAsync(requestBytes, recipietUserId);
        }
    }
}