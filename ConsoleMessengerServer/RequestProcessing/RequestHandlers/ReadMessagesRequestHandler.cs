using AutoMapper;
using Common.Dto.Requests;
using Common.Dto.Responses;
using Common.NetworkServices;
using Common.Serialization;
using ConsoleMessengerServer.DataBaseServices;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestProcessing;
using ConsoleMessengerServer.Requests;
using ConsoleMessengerServer.Responses;

namespace ConsoleMessengerServer.RequestProcessing.RequestHandlers
{
    /// <summary>
    /// Обработчик запроса о прочтении сообщения
    /// </summary>
    public class ReadMessagesRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public ReadMessagesRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
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
            ExtendedReadMessagesRequestDto messagesAreReadRequest = SerializationHelper.Deserialize<ExtendedReadMessagesRequestDto>(networkMessage.Data);

            dbService.ReadMessages(messagesAreReadRequest);

            Response response = new Response(NetworkResponseStatus.Successful);

            BroadcastReadMessagesRequest(messagesAreReadRequest, networkProvider.Id);

            byte[] byteResponse = NetworkMessageConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.ReadMessagesResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.ReadMessagesResponseCode, messagesAreReadRequest.ToString(), response.Status);

            return byteResponse;
        }

        /// <summary>
        /// Транслировать запрос о прочтении сообщений всем сетевым провайдерам пользователя, кроме того, с которого пришел запрос о прочтении
        /// </summary>
        /// <param name="readRequest">Запрос о прочтении сообщений</param>
        /// <param name="networkProviderId"></param>
        private void BroadcastReadMessagesRequest(ExtendedReadMessagesRequestDto readRequest, int networkProviderId)
        {
            ReadMessagesRequest readMessagesRequest = new ReadMessagesRequest(readRequest.MessagesId, readRequest.DialogId);

            byte[] requestBytes = NetworkMessageConverter<ReadMessagesRequest, ReadMessagesRequestDto>.Convert(readMessagesRequest, NetworkMessageCode.ReadMessagesRequestCode);

            _conectionController.BroadcastToSenderAsync(requestBytes, readRequest.UserId, networkProviderId);
        }
    }
}