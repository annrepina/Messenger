using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Entities;
using ConsoleMessengerServer.Entities.Mapping;
using ConsoleMessengerServer.Net;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.RequestHandlers;
using ConsoleMessengerServer.Requests;
using ConsoleMessengerServer.Responses;
using DtoLib.Dto;
using DtoLib.Dto.Requests;
using DtoLib.Dto.Responses;
using DtoLib.NetworkServices;
using DtoLib.Serialization;

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
            RequestHandler handler;

            switch (networkMessage.Code)
            {
                case NetworkMessageCode.SignUpRequestCode:
                    handler = new SignUpRequestHandler(_mapper, _conectionController);
                    break;

                case NetworkMessageCode.SignInRequestCode:
                    handler = new SignInRequestHandler(_mapper, _conectionController);
                    break;

                case NetworkMessageCode.SearchUserRequestCode:
                    handler = new SearchUserRequestHandler(_mapper, _conectionController);
                    break;

                case NetworkMessageCode.CreateDialogRequestCode:
                    handler = new CreateDialogRequestHandler(_mapper, _conectionController);
                    break;

                case NetworkMessageCode.SendMessageRequestCode:
                    handler = new SendMessageRequestHandler(_mapper, _conectionController);
                    break;

                case NetworkMessageCode.DeleteMessageRequestCode:
                    handler = new DeleteMessageRequestHandler(_mapper, _conectionController);
                    break;

                case NetworkMessageCode.DeleteDialogRequestCode:
                    handler = new DeleteDialogRequestHandler(_mapper, _conectionController);
                    break;

                case NetworkMessageCode.SignOutRequestCode:
                    handler = new SignOutRequestHandler(_mapper, _conectionController);
                    break;
                //return ProcessSignOutRequest(networkMessage, networkProvider);

                case NetworkMessageCode.MessagesAreReadRequestCode:
                    //return ProcessMessagesAreReadRequest(networkMessage, networkProvider);
                    handler = new ReadMessagesRequestHandler(_mapper, _conectionController);    
                    break;

                default:
                    return new byte[] { };
            }

            return handler.Process(_dbService, networkMessage, networkProvider);
        }

        #endregion INetworkHandler Implementation
    }
}