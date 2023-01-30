using AutoMapper;
using Common.Dto.Requests;
using Common.Dto.Responses;
using Common.NetworkServices;
using Common.Serialization;
using ConsoleMessengerServer.DataBaseServices;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestProcessing;
using ConsoleMessengerServer.Responses;

namespace ConsoleMessengerServer.RequestProcessing.RequestHandlers
{
    /// <summary>
    /// Обработчик запроса по выходу пользователя из мессенджера
    /// </summary>
    public class SignOutRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public SignOutRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
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
            SignOutRequestDto signOutRequestDto = SerializationHelper.Deserialize<SignOutRequestDto>(networkMessage.Data);

            _conectionController.DisconnectUser(signOutRequestDto.UserId, networkProvider.Id);

            Response response = new Response(NetworkResponseStatus.Successful);

            byte[] responseBytes = NetworkMessageConverter<Response, ResponseDto>.Convert(response, NetworkMessageCode.SignOutResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SignOutResponseCode, signOutRequestDto.ToString(), response.Status);

            return responseBytes;
        }
    }
}