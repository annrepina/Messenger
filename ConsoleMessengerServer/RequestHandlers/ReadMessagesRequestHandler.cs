using AutoMapper;
using ConsoleMessengerServer.DataBase;
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
    public class ReadMessagesRequestHandler : RequestHandler
    {
        public ReadMessagesRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            MessagesAreReadRequestDto messagesAreReadRequest = SerializationHelper.Deserialize<MessagesAreReadRequestDto>(networkMessage.Data);

            dbService.ReadMessages(messagesAreReadRequest);

            Response response = CreateReadMessagesResponse(messagesAreReadRequest, networkProvider.Id);

            //NetworkMessage responseMessage = CreateNetworkMessage(response, out ResponseDto dto, NetworkMessageCode.MessagesAreReadResponseCode);

            //byte[] byteResponse = SerializationHelper.Serialize(responseMessage);
            byte[] byteResponse = ByteArrayConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.MessagesAreReadResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.MessagesAreReadResponseCode, messagesAreReadRequest.ToString(), response.Status);

            return byteResponse;
        }

        private Response CreateReadMessagesResponse(MessagesAreReadRequestDto readRequest, int networkProviderId)
        {
            Response response = new Response(NetworkResponseStatus.Successful);

            MessagesAreReadRequestForClient readMessagesRequest = new MessagesAreReadRequestForClient(readRequest.MessagesId, readRequest.DialogId);

            //NetworkMessage requestMessage = CreateNetworkMessage(readMessagesRequest, out MessagesAreReadRequestForClientDto dto, NetworkMessageCode.MessagesAreReadRequestCode);

            //byte[] requestBytes = SerializationHelper.Serialize(requestMessage);
            byte[] requestBytes = ByteArrayConverter<MessagesAreReadRequestForClient, MessagesAreReadRequestForClientDto>.Convert(readMessagesRequest, NetworkMessageCode.MessagesAreReadRequestCode);

            _conectionController.BroadcastToSenderAsync(requestBytes, readRequest.UserId, networkProviderId);

            return response;
        }
    }
}