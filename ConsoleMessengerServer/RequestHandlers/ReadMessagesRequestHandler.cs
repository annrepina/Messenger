using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Requests;
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
    public class ReadMessagesRequestHandler : RequestHandler
    {
        public ReadMessagesRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            MessagesReadRequestDto messagesAreReadRequest = SerializationHelper.Deserialize<MessagesReadRequestDto>(networkMessage.Data);

            dbService.ReadMessages(messagesAreReadRequest);

            Response response = CreateReadMessagesResponse(messagesAreReadRequest, networkProvider.Id);

            //NetworkMessage responseMessage = CreateNetworkMessage(response, out ResponseDto dto, NetworkMessageCode.ReadMessagesResponseCode);

            //byte[] byteResponse = SerializationHelper.Serialize(responseMessage);
            byte[] byteResponse = ByteArrayConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.ReadMessagesResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.ReadMessagesResponseCode, messagesAreReadRequest.ToString(), response.Status);

            return byteResponse;
        }

        private Response CreateReadMessagesResponse(MessagesReadRequestDto readRequest, int networkProviderId)
        {
            Response response = new Response(NetworkResponseStatus.Successful);

            MessagesAreReadRequestForClient readMessagesRequest = new MessagesAreReadRequestForClient(readRequest.MessagesId, readRequest.DialogId);

            //NetworkMessage requestMessage = CreateNetworkMessage(readMessagesRequest, out ReadMessagesRequestDto dto, NetworkMessageCode.ReadMessagesRequestCode);

            //byte[] requestBytes = SerializationHelper.Serialize(requestMessage);
            byte[] requestBytes = ByteArrayConverter<MessagesAreReadRequestForClient, ReadMessagesRequestDto>.Convert(readMessagesRequest, NetworkMessageCode.ReadMessagesRequestCode);

            _conectionController.BroadcastToSenderAsync(requestBytes, readRequest.UserId, networkProviderId);

            return response;
        }
    }
}