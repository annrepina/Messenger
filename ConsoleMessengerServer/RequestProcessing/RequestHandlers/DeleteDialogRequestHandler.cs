using AutoMapper;
using Common.Dto.Requests;
using Common.Dto.Responses;
using Common.NetworkServices;
using Common.Serialization;
using ConsoleMessengerServer.DataBaseServices;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestProcessing;
using ConsoleMessengerServer.Requests;
using ConsoleMessengerServer.Responses;

namespace ConsoleMessengerServer.RequestProcessing.RequestHandlers
{
    /// <summary>
    /// Обработчик запроса об удалении диалога
    /// </summary>
    public class DeleteDialogRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public DeleteDialogRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {
        }

        /// <summary>
        /// Обрабатывает сетевое сообщение
        /// </summary>
        /// <param name="dbService">Сервис для работы с базой данных</param>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        /// <returns>Ответ на сетевое сообщение в виде массива байт</returns>
        protected override byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            ExtendedDeleteDialogRequestDto deleteDialogRequestDto = SerializationHelper.Deserialize<ExtendedDeleteDialogRequestDto>(networkMessage.Data);

            Dialog? dialog = dbService.FindDialog(deleteDialogRequestDto);

            Response response = ProcessFoundDialog(dbService, dialog, networkProvider.Id, deleteDialogRequestDto.UserId);

            byte[] responseBytes = NetworkMessageConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.DeleteDialogResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.DeleteDialogResponseCode, deleteDialogRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        /// <summary>
        /// Обработать найденный в базе данных диалог
        /// </summary>
        /// <param name="dbService">Сервис для работы с базой данных</param>
        /// <param name="dialog">Диалог найденный в базе данных</param>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        /// <param name="userId">Id пользователя</param>
        /// <returns>Ответ на запрос об удалении диалога</returns>
        private Response ProcessFoundDialog(DbService dbService, Dialog? dialog, int networkProviderId, int userId)
        {
            if (dialog != null)
            {
                int interlocutorId = dbService.GetInterlocutorId(dialog.Id, userId);

                dbService.DeleteDialog(dialog);

                DeleteDialogRequest deleteDialogRequest = new DeleteDialogRequest(dialog.Id);

                byte[] requestBytes = NetworkMessageConverter<DeleteDialogRequest, DeleteDialogRequestDto>.Convert(deleteDialogRequest, NetworkMessageCode.DeleteDialogRequestCode);

                _conectionController.BroadcastToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }
    }
}