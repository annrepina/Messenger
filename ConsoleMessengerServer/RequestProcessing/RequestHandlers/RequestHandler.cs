using AutoMapper;
using Common.Dto.Responses;
using Common.NetworkServices;
using ConsoleMessengerServer.DataBaseServices;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Responses;

namespace ConsoleMessengerServer.RequestProcessing.RequestHandlers
{
    /// <summary>
    /// Базовый абстрактный класс - бработчик сетевого сообщения
    /// </summary>
    public abstract class RequestHandler
    {
        /// <summary>
        /// Маппер для мапинга DTO
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Отвечает за соединение по сети с клиентами
        /// </summary>
        protected readonly IConnectionController _conectionController;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="connectionController">Отвечает за соединение по сети с клиентами</param>
        public RequestHandler(IMapper mapper, IConnectionController connectionController)
        {
            _mapper = mapper;
            _conectionController = connectionController;
        }

        /// <summary>
        /// Обработать сетевое сообщение
        /// </summary>
        /// <param name="dbService">Сервис для работы с базой данных</param>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        /// <returns>Ответ на сетевое сообщение в виде массива байт</returns>
        public byte[] Process(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            try
            {
                return OnProcess(dbService, networkMessage, networkProvider);
            }

            catch (IOException)
            {
                throw;
            }

            catch (Exception)
            {
                OnError(networkMessage, networkProvider);
                throw;
            }
        }

        /// <summary>
        /// Обрабатывает ошибку возникшую при обработке сетевого сообщения
        /// Ошибка скорее всего может бть связана с базой данных
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        protected virtual void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            Response response = new Response(NetworkResponseStatus.FatalError);
            SendErrorResponse<Response, ResponseDto>(networkProvider, response, NetworkMessageCode.DeleteMessageResponseCode);
        }

        /// <summary>
        /// Обрабатывает сетевое сообщение
        /// </summary>
        /// <param name="dbService">Сервис для работы с базой данных</param>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        /// <returns>Ответ на сетевое сообщение в виде массива байт</returns>
        protected abstract byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider);

        /// <summary>
        /// Отправить ошибку в качестве ответа на сетевое сообщение
        /// </summary>
        /// <typeparam name="TResponse">Тип объекта представляющего ответ</typeparam>
        /// <typeparam name="TResponseDto">Тип объекта, представляющего DTO - ответ</typeparam>
        /// <param name="networkProvider">Сетевой провайдер</param>
        /// <param name="response">Ответ на сетевое сообщение</param>
        /// <param name="code">Код сетевого сообщения</param>
        protected void SendErrorResponse<TResponse, TResponseDto>(IServerNetworProvider networkProvider, TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            byte[] responseBytes = NetworkMessageConverter<TResponse, TResponseDto>.Convert(response, code);

            _conectionController.BroadcastError(responseBytes, networkProvider);
        }

        /// <summary>
        /// Напечать отчет о запросе и ответе
        /// </summary>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        /// <param name="requestCode">Код запроса</param>
        /// <param name="responseCode">Код ответа</param>
        /// <param name="request">Запрос</param>
        /// <param name="responseStatus">Статус ответа</param>
        protected void PrintReport(int networkProviderId, NetworkMessageCode requestCode, NetworkMessageCode responseCode, string request, NetworkResponseStatus responseStatus)
        {
            ReportPrinter.PrintRequestReport(networkProviderId, requestCode, request);
            ReportPrinter.PrintResponseReport(networkProviderId, responseCode, responseStatus);
        }
    }
}