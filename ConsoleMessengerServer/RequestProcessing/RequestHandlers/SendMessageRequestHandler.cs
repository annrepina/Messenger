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
    /// Обработчик запроса об отправке пользователем сообщения
    /// </summary>
    public class SendMessageRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public SendMessageRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
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
            SendMessageResponse errorResponse = new SendMessageResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<SendMessageResponse, SendMessageResponseDto>(networkProvider, errorResponse, NetworkMessageCode.SendMessageResponseCode);
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
            SendMessageRequestDto sendMessageRequestDto = SerializationHelper.Deserialize<SendMessageRequestDto>(networkMessage.Data);

            Message message = dbService.AddMessage(sendMessageRequestDto);
            SendMessageResponse response = new SendMessageResponse(message.Id, NetworkResponseStatus.Successful);

            BroadcastSendMessageRequest(dbService, message, networkProvider.Id);

            byte[] responseBytes = NetworkMessageConverter<SendMessageResponse, SendMessageResponseDto>.Convert(response, NetworkMessageCode.SendMessageResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SendMessageResponseCode, sendMessageRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        /// <summary>
        /// Транслировать запрос об отправке сообщения
        /// </summary>
        /// <param name="dbService">Сервис для работы с базой данных</param>
        /// <param name="message">Сообщение</param>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        private void BroadcastSendMessageRequest(DbService dbService, Message message, int networkProviderId)
        {
            int recipietUserId = dbService.GetRecipientUserId(message);

            SendMessageRequest sendMessageRequest = new SendMessageRequest(message, message.DialogId);

            byte[] requestBytes = NetworkMessageConverter<SendMessageRequest, SendMessageRequestDto>.Convert(sendMessageRequest, NetworkMessageCode.SendMessageRequestCode);

            _conectionController.BroadcastToSenderAsync(requestBytes, message.UserSenderId, networkProviderId);
            _conectionController.BroadcastToInterlocutorAsync(requestBytes, recipietUserId);
        }
    }
}