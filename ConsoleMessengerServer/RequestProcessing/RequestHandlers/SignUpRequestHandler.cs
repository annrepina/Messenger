using AutoMapper;
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
    /// Обработчик запроса о регистрации в мессенджере
    /// </summary>
    public class SignUpRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public SignUpRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
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
            SignUpResponse errorResponse = new SignUpResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<SignUpResponse, SignUpResponseDto>(networkProvider, errorResponse, NetworkMessageCode.SignUpResponseCode);
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
            SignUpRequestDto signUpRequestDto = SerializationHelper.Deserialize<SignUpRequestDto>(networkMessage.Data);

            User? user = dbService.AddNewUser(signUpRequestDto);

            SignUpResponse signUpResponse = ProcessAddingUserInDb(user, networkProvider.Id);

            byte[] responseBytes = NetworkMessageConverter<SignUpResponse, SignUpResponseDto>.Convert(signUpResponse, NetworkMessageCode.SignUpResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SignUpResponseCode, signUpRequestDto.ToString(), signUpResponse.Status);

            return responseBytes;
        }

        /// <summary>
        /// Обработать добавление пользователя в базу данных
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        /// <returns>Ответ на запрос о регистрации пользователя</returns>
        private SignUpResponse ProcessAddingUserInDb(User? user, int networkProviderId)
        {
            if (user != null)
            {
                _conectionController.AddNewSession(user.Id, networkProviderId);

                return new SignUpResponse(user.Id, NetworkResponseStatus.Successful);
            }

            return new SignUpResponse(NetworkResponseStatus.Failed);
        }
    }
}