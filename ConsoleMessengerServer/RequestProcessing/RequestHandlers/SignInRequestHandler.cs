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
    /// Обработчик запроса о входе в мессенджер
    /// </summary>
    public class SignInRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public SignInRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
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
            SignInResponse signInResponse = new SignInResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<SignInResponse, SignInResponseDto>(networkProvider, signInResponse, NetworkMessageCode.SignUpResponseCode);
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
            SignInRequestDto signInRequestDto = SerializationHelper.Deserialize<SignInRequestDto>(networkMessage.Data);

            User? user = dbService.FindUserByPhoneNumber(signInRequestDto.PhoneNumber);

            SignInResponse signInResponse = ProcessFoundUser(dbService, user, signInRequestDto, networkProvider.Id);

            byte[] responseBytes = NetworkMessageConverter<SignInResponse, SignInResponseDto>.Convert(signInResponse, NetworkMessageCode.SignInResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SignInResponseCode, signInRequestDto.ToString(), signInResponse.Status);

            return responseBytes;
        }

        /// <summary>
        /// Обработать найденного в базе данных пользователя
        /// </summary>
        /// <param name="dbService">Сервис для работы с базой данных</param>
        /// <param name="user">Найденный в базе данных пользователь</param>
        /// <param name="signInRequestDto">DTO - запрос на вход в мессенджер</param>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        /// <returns>Ответ на запрос о входе пользователя</returns>
        private SignInResponse ProcessFoundUser(DbService dbService, User? user, SignInRequestDto signInRequestDto, int networkProviderId)
        {
            if (user != null)
            {
                if (user.Password == signInRequestDto.Password)
                {
                    List<Dialog> dialogs = dbService.FindDialogsByUser(user);
                    _conectionController.AddNewSession(user.Id, networkProviderId);

                    return new SignInResponse(user, dialogs, NetworkResponseStatus.Successful);
                }

                return new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.Password);
            }

            return new SignInResponse(NetworkResponseStatus.Failed, SignInFailContext.PhoneNumber);
        }
    }
}