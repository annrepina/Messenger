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

namespace ConsoleMessengerServer.RequestHandlers
{
    public class DeleteMessageRequestHandler : RequestHandler
    {
        public DeleteMessageRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            DeleteMessageRequestDto deleteMessageRequestDto = SerializationHelper.Deserialize<DeleteMessageRequestDto>(networkMessage.Data);

            Message? message = dbService.FindMessage(deleteMessageRequestDto);

            Response response = CreateDeleteMessageResponse(dbService, message, deleteMessageRequestDto.UserId, networkProvider.Id);

            NetworkMessage responseMessage = CreateNetworkMessage<Response, ResponseDto>(response, out ResponseDto dto, NetworkMessageCode.DeleteMessageResponseCode);

            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);

            PrintReport(networkProvider.Id, networkMessage.Code, responseMessage.Code, deleteMessageRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        private Response CreateDeleteMessageResponse(DbService dbService, Message? message, int userId, int networkProviderId)
        {
            if (message != null)
            {
                int interlocutorId = dbService.GetInterlocutorId(message.DialogId, userId);
                dbService.DeleteMessage(message);

                DeleteMessageRequestForClient deleteMessageRequest = new DeleteMessageRequestForClient(message.Id, message.DialogId);

                NetworkMessage requestMessage = CreateNetworkMessage<DeleteMessageRequestForClient, DeleteMessageRequestForClientDto>(deleteMessageRequest, out DeleteMessageRequestForClientDto dto, NetworkMessageCode.DeleteMessageRequestCode);

                byte[] requestBytes = SerializationHelper.Serialize(requestMessage);

                _conectionController.BroadcastToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }
    }
}