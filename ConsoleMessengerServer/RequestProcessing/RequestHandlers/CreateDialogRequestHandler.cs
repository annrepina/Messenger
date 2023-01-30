using AutoMapper;
using Common.Dto;
using Common.Dto.Requests;
using Common.Dto.Responses;
using Common.NetworkServices;
using Common.Serialization;
using ConsoleMessengerServer.DataBaseServices;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestProcessing;
using ConsoleMessengerServer.Responses;

namespace ConsoleMessengerServer.RequestProcessing.RequestHandlers
{
    /// <summary>
    /// Обработчик запроса по созданию диалога
    /// </summary>
    public class CreateDialogRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public CreateDialogRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
        {

        }

        /// <summary>
        /// Обрабатывает ошибку возникшую при обработке сетевого сообщения
        /// Ошибка скорее всего может бть связана с базой данных
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        protected override void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            CreateDialogResponse errorResponse = new CreateDialogResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<CreateDialogResponse, CreateDialogResponseDto>(networkProvider, errorResponse, NetworkMessageCode.CreateDialogResponseCode);
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
            CreateDialogRequestDto createDialogRequestDto = SerializationHelper.Deserialize<CreateDialogRequestDto>(networkMessage.Data);

            Dialog dialog = dbService.CreateDialog(createDialogRequestDto);

            CreateDialogResponse response = _mapper.Map<CreateDialogResponse>(dialog);

            byte[] responseBytes = NetworkMessageConverter<CreateDialogResponse, CreateDialogResponseDto>.Convert(response, NetworkMessageCode.CreateDialogResponseCode);

            BroadcastCreateDialogRequests(dialog, networkProvider.Id);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.CreateDialogResponseCode, createDialogRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        /// <summary>
        /// Транслировать запрос о создании диалога на другие сетевые провайдеры через которые подключен пользователь-инициатор диалога
        /// И на все сетевые провайдеры через которые подключен собеседник пользователя
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="networkProviderId"></param>
        private void BroadcastCreateDialogRequests(Dialog dialog, int networkProviderId)
        {
            int senderId = dialog.Messages.First().UserSenderId;
            int recipientId = dialog.Users.First(user => user.Id != senderId).Id;

            byte[] createDialogMessageBytes = NetworkMessageConverter<Dialog, DialogDto>.Convert(dialog, NetworkMessageCode.CreateDialogRequestCode);

            _conectionController.BroadcastToSenderAsync(createDialogMessageBytes, senderId, networkProviderId);
            _conectionController.BroadcastToInterlocutorAsync(createDialogMessageBytes, recipientId);
        }
    }
}