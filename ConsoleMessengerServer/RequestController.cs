using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities.Mapping;
using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestHandlers;
using CommonLib.NetworkServices;
using CommonLib.Serialization;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Класс, который отвечает
    /// </summary>
    public class RequestController : IRequestController
    {
        /// <inheritdoc cref="ConnectionController"/>
        private IConnectionController _conectionController;

        /// <summary>
        /// Маппер для мапинга ентити на DTO и обратно
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис для работы с базами данных
        /// </summary>
        private readonly DbService _dbService;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public RequestController(IConnectionController connectionController)
        {
            DataBaseMapper mapper = DataBaseMapper.GetInstance();
            _mapper = mapper.CreateIMapper();

            _dbService = new DbService(_mapper);
            _conectionController = connectionController;
        }

        #region INetworkHandler Implementation

        public byte[] ProcessRequest(byte[] data, IServerNetworProvider networkProvider)
        {
            NetworkMessage requestMessage = SerializationHelper.Deserialize<NetworkMessage>(data);

            byte[] response = ProcessRequestMessage(requestMessage, networkProvider);

            return response;
        }

        /// <summary>
        /// Обработка сетевого сообщения
        /// </summary>
        /// <param name="networkMessage">Сетевое сообщение</param>
        /// <param name="networkProvider">Идентификатор отправителя</param>
        private byte[] ProcessRequestMessage(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            RequestHandler handler = RequestHandlerCreator.FactoryMethod(_mapper, _conectionController, networkMessage.Code);

            return handler.Process(_dbService, networkMessage, networkProvider);
        }

        #endregion INetworkHandler Implementation
    }
}