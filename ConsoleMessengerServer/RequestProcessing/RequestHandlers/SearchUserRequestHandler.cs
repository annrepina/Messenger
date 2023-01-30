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
    /// Обработчик запроса о поиске пользователя среди зарегистрировавшихся в мессенджере
    /// </summary>
    public class SearchUserRequestHandler : RequestHandler
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public SearchUserRequestHandler(IMapper mapper, IConnectionController connectionController) : base(mapper, connectionController)
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
            UserSearchResponse errorResponse = new UserSearchResponse(NetworkResponseStatus.FatalError);
            SendErrorResponse<UserSearchResponse, UserSearchResponseDto>(networkProvider, errorResponse, NetworkMessageCode.SearchResponseCode);
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
            SearchRequestDto searchRequestDto = SerializationHelper.Deserialize<SearchRequestDto>(networkMessage.Data);

            List<User> usersList = dbService.FindListOfUsers(searchRequestDto);

            UserSearchResponse response = ProcessFoundUsersList(usersList);

            byte[] responseBytes = NetworkMessageConverter<UserSearchResponse, UserSearchResponseDto>.Convert(response, NetworkMessageCode.SearchResponseCode);

            PrintReport(networkProvider.Id, networkMessage.Code, NetworkMessageCode.SearchResponseCode, searchRequestDto.ToString(), response.Status);

            return responseBytes;
        }

        /// <summary>
        /// Обработать найденный в базе данных список пользователей
        /// </summary>
        /// <param name="usersList">Список пользователей</param>
        /// <returns>Ответ на запрос о поиске пользователя</returns>
        private UserSearchResponse ProcessFoundUsersList(List<User> usersList)
        {
            if (usersList.Count > 0)
                return new UserSearchResponse(usersList, NetworkResponseStatus.Successful);

            return new UserSearchResponse(NetworkResponseStatus.Failed);
        }
    }
}