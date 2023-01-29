using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Responses;
using DtoLib.Dto;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
using DtoLib.NetworkServices;
using DtoLib.Serialization;

namespace ConsoleMessengerServer.RequestHandlers
{
    public class CreateDialogRequestHandler : RequestHandler
    {
        public CreateDialogRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {

        }

        protected override void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            CreateDialogResponse errorResponse = new CreateDialogResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<CreateDialogResponse, CreateDialogResponseDto>(networkProvider, errorResponse, NetworkMessageCode.CreateDialogResponseCode);
        }

        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            CreateDialogRequestDto createDialogRequestDto = SerializationHelper.Deserialize<CreateDialogRequestDto>(networkMessage.Data);

            Dialog dialog = dbService.CreateDialog(createDialogRequestDto);

            CreateDialogResponse response = _mapper.Map<CreateDialogResponse>(dialog);

            //NetworkMessage responseMessage = CreateNetworkMessage(response, out CreateDialogResponseDto responseDto, NetworkMessageCode.CreateDialogResponseCode);

            //byte[] responseBytes = SerializationHelper.Serialize(responseMessage);
            byte[] responseBytes = ByteArrayConverter<CreateDialogResponse, CreateDialogResponseDto>.Convert(response, NetworkMessageCode.CreateDialogResponseCode);

            BroadcastCreateDialogRequests(dialog, networkProvider.Id);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.CreateDialogResponseCode, createDialogRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        /// <summary>
        /// Создать ответ на запрос о создании диалога
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="networkProviderId"></param>
        /// <returns></returns>
        private void BroadcastCreateDialogRequests(Dialog dialog, int networkProviderId)
        {
            int senderId = dialog.Messages.First().UserSenderId;
            int recipientId = dialog.Users.First(user => user.Id != senderId).Id;

            //NetworkMessage createDialogMessage = CreateNetworkMessage(dialog, out DialogDto responseDto, NetworkMessageCode.CreateDialogRequestCode);

            //byte[] createDialogMessageBytes = SerializationHelper.Serialize(createDialogMessage);
            byte[] createDialogMessageBytes = ByteArrayConverter<Dialog, DialogDto>.Convert(dialog, NetworkMessageCode.CreateDialogRequestCode);

            _conectionController.BroadcastToSenderAsync(createDialogMessageBytes, senderId, networkProviderId);
            _conectionController.BroadcastToInterlocutorAsync(createDialogMessageBytes, recipientId);
        }
    }
}