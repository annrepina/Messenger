using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Requests;
using ConsoleMessengerServer.Responses;
using CommonLib.Dto.Requests;
using CommonLib.Dto.Responses;
using CommonLib.NetworkServices;
using CommonLib.Serialization;

namespace ConsoleMessengerServer.RequestHandlers
{
    public class DeleteMessageRequestHandler : RequestHandler
    {
        public DeleteMessageRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            ExtendedDeleteMessageRequestDto deleteMessageRequestDto = SerializationHelper.Deserialize<ExtendedDeleteMessageRequestDto>(networkMessage.Data);

            Message? message = dbService.FindMessage(deleteMessageRequestDto);

            Response response = CreateDeleteMessageResponse(dbService, message, deleteMessageRequestDto.UserId, networkProvider.Id);

            //NetworkMessage responseMessage = CreateNetworkMessage<Response, ResponseDto>(response, out ResponseDto dto, NetworkMessageCode.DeleteMessageResponseCode);

            //byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            byte[] responseBytes = ByteArrayConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.DeleteMessageResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.DeleteMessageResponseCode, deleteMessageRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        private Response CreateDeleteMessageResponse(DbService dbService, Message? message, int userId, int networkProviderId)
        {
            if (message != null)
            {
                int interlocutorId = dbService.GetInterlocutorId(message.DialogId, userId);
                dbService.DeleteMessage(message);

                DeleteMessageRequestForClient deleteMessageRequest = new DeleteMessageRequestForClient(message.Id, message.DialogId);

                //NetworkMessage requestMessage = CreateNetworkMessage<DeleteMessageRequestForClient, DeleteMessageRequestDto>(deleteMessageRequest, out DeleteMessageRequestDto dto, NetworkMessageCode.DeleteMessageRequestCode);

                //byte[] requestBytes = SerializationHelper.Serialize(requestMessage);
                byte[] requestBytes = ByteArrayConverter<DeleteMessageRequestForClient, DeleteMessageRequestDto>.Convert(deleteMessageRequest, NetworkMessageCode.DeleteMessageRequestCode);

                _conectionController.BroadcastToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }
    }
}