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
    public class DeleteDialogRequestHandler : RequestHandler
    {
        public DeleteDialogRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            DeleteDialogRequestDto deleteDialogRequestDto = SerializationHelper.Deserialize<DeleteDialogRequestDto>(networkMessage.Data);

            Dialog? dialog = dbService.FindDialog(deleteDialogRequestDto);

            Response response = CreateDeleteDialogResponse(dbService, dialog, networkProvider.Id, deleteDialogRequestDto.UserId);

            NetworkMessage responseMessage = CreateNetworkMessage<Response, ResponseDto>(response, out ResponseDto dto, NetworkMessageCode.DeleteDialogResponseCode);

            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);

            PrintReport(networkProvider.Id, networkMessage.Code, responseMessage.Code, deleteDialogRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        private Response CreateDeleteDialogResponse(DbService dbService, Dialog? dialog, int networkProviderId, int userId)
        {
            if (dialog != null)
            {
                int interlocutorId = dbService.GetInterlocutorId(dialog.Id, userId);

                dbService.DeleteDialog(dialog);

                DeleteDialogRequestForClient deleteDialogRequest = new DeleteDialogRequestForClient(dialog.Id);

                NetworkMessage requestMessage = CreateNetworkMessage<DeleteDialogRequestForClient, DeleteDialogRequestForClientDto>(deleteDialogRequest, out DeleteDialogRequestForClientDto dto, NetworkMessageCode.DeleteDialogRequestCode);

                byte[] requestBytes = SerializationHelper.Serialize(requestMessage);

                _conectionController.BroadcastToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }
    }
}