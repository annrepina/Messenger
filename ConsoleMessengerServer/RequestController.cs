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

                //case NetworkMessageCode.DeleteMessageRequestCode:
                //    return ProcessDeleteMessageRequest(networkMessage, networkProvider);

                //case NetworkMessageCode.DeleteDialogRequestCode:
                //    return ProcessDeleteDialogRequest(networkMessage, networkProvider);

                //case NetworkMessageCode.SignOutRequestCode:
                //    return ProcessSignOutRequest(networkMessage, networkProvider);

                //case NetworkMessageCode.MessagesAreReadRequestCode:
                //    return ProcessMessagesAreReadRequest(networkMessage, networkProvider);

                default:
                    return new byte[] { };
            }

            return handler.Process(_dbService, networkMessage, networkProvider);
        }

        #endregion INetworkHandler Implementation

        private byte[] ProcessDeleteMessageRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                DeleteMessageRequestDto deleteMessageRequestDto = SerializationHelper.Deserialize<DeleteMessageRequestDto>(networkMessage.Data);

                Message? message = _dbService.FindMessage(deleteMessageRequestDto);

                Response deleteMessageResponse = CreateDeleteMessageResponse(message, deleteMessageRequestDto.UserId, networkProviderId);

                byte[] responseBytes = CreateNetworkMessageBytes<Response, ResponseDto>(deleteMessageResponse, NetworkMessageCode.DeleteMessageResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, deleteMessageRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.DeleteDialogResponseCode, deleteMessageResponse.Status);

                return responseBytes;
            }
            catch (Exception)
            {
                Response response = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, response, NetworkMessageCode.DeleteMessageResponseCode);
                throw;
            }
        }

        /// <summary>
        /// Обрабатывает запрос на удаление диалога
        /// </summary>
        /// <param name="networkMessage"></param>
        /// <param name="serverNetworkProvider"></param>
        private byte[] ProcessDeleteDialogRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                DeleteDialogRequestDto deleteDialogRequestDto = SerializationHelper.Deserialize<DeleteDialogRequestDto>(networkMessage.Data);

                Dialog? dialog = _dbService.FindDialog(deleteDialogRequestDto);

                Response deleteDialogResponse = CreateDeleteDialogResponse(dialog, networkProviderId, deleteDialogRequestDto.UserId);

                byte[] responseBytes = CreateNetworkMessageBytes<Response, ResponseDto>(deleteDialogResponse, NetworkMessageCode.DeleteDialogResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, deleteDialogRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.DeleteDialogResponseCode, deleteDialogResponse.Status);

                return responseBytes;
            }
            catch (Exception)
            {
                Response errorResponse = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, errorResponse, NetworkMessageCode.DeleteDialogResponseCode);
                throw;
            }
        }

        private byte[] ProcessSignOutRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                SignOutRequestDto signOutRequestDto = SerializationHelper.Deserialize<SignOutRequestDto>(networkMessage.Data);

                _conectionController.DisconnectUser(signOutRequestDto.UserId, networkProviderId);

                Response signOutResponse = new Response(NetworkResponseStatus.Successful);

                byte[] responseBytes = CreateNetworkMessageBytes<Response, ResponseDto>(signOutResponse, NetworkMessageCode.SignOutResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, signOutRequestDto.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.SignOutResponseCode, signOutResponse.Status);

                return responseBytes;
            }
            catch (Exception)
            {
                Response errorResponse = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, errorResponse, NetworkMessageCode.SignOutResponseCode);
                throw;
            }
        }

        private byte[] ProcessMessagesAreReadRequest(NetworkMessage networkMessage, int networkProviderId)
        {
            try
            {
                MessagesAreReadRequestDto messagesAreReadRequest = SerializationHelper.Deserialize<MessagesAreReadRequestDto>(networkMessage.Data);

                _dbService.ReadMessages(messagesAreReadRequest);

                Response response = CreateMessagesAreReadRersponse(messagesAreReadRequest, networkProviderId);

                byte[] byteResponse = CreateNetworkMessageBytes<Response, ResponseDto>(response, NetworkMessageCode.MessagesAreReadResponseCode);

                ReportPrinter.PrintRequestReport(networkProviderId, networkMessage.Code, messagesAreReadRequest.ToString());
                ReportPrinter.PrintResponseReport(networkProviderId, NetworkMessageCode.MessagesAreReadResponseCode, response.Status);

                return byteResponse;
            }
            catch (Exception)
            {
                Response errorResponse = new Response(NetworkResponseStatus.FatalError);
                SendError<Response, ResponseDto>(networkProviderId, errorResponse, NetworkMessageCode.MessagesAreReadResponseCode);
                throw;
            }
        }

        private void SendError<TResponse, TResponseDto>(int networkProviderId, TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            byte[] responseBytes = CreateNetworkMessageBytes<TResponse, TResponseDto>(response, code);
            //_conectionController.BroadcastErrorToSenderAsync(responseBytes, networkProviderId);
        }

        /// <summary>
        /// Обобщенный метод создания ответного сетевого сообщения
        /// </summary>
        /// <typeparam name="Tsource">Тип данных источника для мапинга на dto</typeparam>
        /// <typeparam name="Tdto">Тип конкретного dto</typeparam>
        /// <param name="tsource">Объект - источник для мапинга на dto</param>
        /// <param name="dto">Объект, представляющий dto</param>
        /// <param name="code">Код операции</param>
        /// <returns></returns>
        public NetworkMessage CreateNetworkMessage<Tsource, Tdto>(Tsource tsource, out Tdto dto, NetworkMessageCode code)
            where Tdto : class
        {
            dto = _mapper.Map<Tdto>(tsource);

            byte[] data = SerializationHelper.Serialize(dto);

            var message = new NetworkMessage(data, code);

            return message;
        }

        /// <summary>
        /// Отправить ответ на запрос сетевому провайдеру
        /// </summary>
        /// <typeparam name="TResponse">Тип класса ответа</typeparam>
        /// <typeparam name="TResponseDto">Тип dto класса ответа</typeparam>
        /// <param name="response">Ответ</param>
        /// <param name="code">Код операции</param>
        /// <param name="serverNetworkProvider">Сетевой провайдер</param>
        /// <returns></returns>
        private byte[] CreateNetworkMessageBytes<TResponse, TResponseDto>(TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            NetworkMessage responseMessage = CreateNetworkMessage(response, out TResponseDto responseDto, code);

            return SerializationHelper.Serialize(responseMessage);
        }

        #region Методы, создающие ответы на запросы

        private Response CreateDeleteMessageResponse(Message? message, int userId, int networkProviderId)
        {
            if (message != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(message.DialogId, userId);
                _dbService.DeleteMessage(message);

                DeleteMessageRequestForClient deleteMessageRequestForClient = new DeleteMessageRequestForClient(message.Id, message.DialogId);

                byte[] requestBytes = CreateNetworkMessageBytes<DeleteMessageRequestForClient, DeleteMessageRequestForClientDto>(deleteMessageRequestForClient, NetworkMessageCode.DeleteMessageRequestCode);

                _conectionController.BroadcastToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }

        private Response CreateDeleteDialogResponse(Dialog? dialog, int networkProviderId, int userId)
        {
            if (dialog != null)
            {
                int interlocutorId = _dbService.GetInterlocutorId(dialog.Id, userId);

                _dbService.DeleteDialog(dialog);

                DeleteDialogRequestForClient deleteDialogRequestForClient = new DeleteDialogRequestForClient(dialog.Id);

                byte[] requestBytes = CreateNetworkMessageBytes<DeleteDialogRequestForClient, DeleteDialogRequestForClientDto>(deleteDialogRequestForClient, NetworkMessageCode.DeleteDialogRequestCode);

                _conectionController.BroadcastToSenderAsync(requestBytes, userId, networkProviderId);
                _conectionController.BroadcastToInterlocutorAsync(requestBytes, interlocutorId);

                return new Response(NetworkResponseStatus.Successful);
            }

            return new Response(NetworkResponseStatus.Failed);
        }

        private Response CreateMessagesAreReadRersponse(MessagesAreReadRequestDto readRequest, int networkProviderId)
        {
            Response response = new Response(NetworkResponseStatus.Successful);

            MessagesAreReadRequestForClient messagesAreReadRequest = new MessagesAreReadRequestForClient(readRequest.MessagesId, readRequest.DialogId);
            byte[] requestBytes = CreateNetworkMessageBytes<MessagesAreReadRequestForClient, MessagesAreReadRequestForClientDto>(messagesAreReadRequest, NetworkMessageCode.MessagesAreReadRequestCode);

            _conectionController.BroadcastToSenderAsync(requestBytes, readRequest.UserId, networkProviderId);

            return response;
        }

        #endregion Методы, создающие ответы на запросы
    }
}