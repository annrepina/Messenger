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
    /// Обработчик запроса об удалении сообщения
    /// </summary>
    public class DeleteMessageRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public DeleteMessageRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
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
            ExtendedDeleteMessageRequestDto deleteMessageRequestDto = SerializationHelper.Deserialize<ExtendedDeleteMessageRequestDto>(networkMessage.Data);

            Message? message = dbService.FindMessage(deleteMessageRequestDto);

            Response response = ProcessFoundMessage(dbService, message, deleteMessageRequestDto.UserId, networkProvider.Id);

            byte[] responseBytes = NetworkMessageConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.DeleteMessageResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.DeleteMessageResponseCode, deleteMessageRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        /// <summary>
        /// Обработать найденное в базе данных сообщение
        /// </summary>
        /// <param name="dbService">Сервис для работы с базой данных</param>
        /// <param name="message">Сообщение найденное в базе данных</param>
        /// <param name="userId">Id пользователя</param>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        /// <returns>Ответ на запрос об удалении сообщения</returns>
        private Response ProcessFoundMessage(DbService dbService, Message? message, int userId, int networkProviderId)
        {
            if (message != null)
            {
                int interlocutorId = dbService.GetInterlocutorId(message.DialogId, userId);
                dbService.DeleteMessage(message);

                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest(message.Id, message.DialogId);

                byte[] requestBytes = NetworkMessageConverter<DeleteMessageRequest, DeleteMessageRequestDto>.Convert(deleteMessageRequest, NetworkMessageCode.DeleteMessageRequestCode);

                _conectionController.BroadcastToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }
    }
}