using AutoMapper;
using ConsoleMessengerServer.DataBase;
using ConsoleMessengerServer.Net.Interfaces;
using ConsoleMessengerServer.Responses;
using DtoLib.Dto.Responses;
using DtoLib.NetworkServices;
using DtoLib.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.RequestHandlers
{
    public abstract class RequestHandler
    {
        protected readonly IMapper _mapper;

        protected readonly IConnectionController _conectionController;

        public RequestHandler(IMapper mapper, IConnectionController connectionController)
        {
            _mapper = mapper;
            _conectionController = connectionController;
        }

        public byte[] Process(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            try
            {
                return OnProcess(dbService, networkMessage, networkProvider);
            }

            catch (Exception)
            {
                OnError(networkMessage, networkProvider);
                throw;
            }
        }

        protected virtual void OnError(NetworkMessage networkMessage, IServerNetworProvider networkProvider)
        {
            Response response = new Response(NetworkResponseStatus.FatalError);
            SendError<Response, ResponseDto>(networkProvider, response, NetworkMessageCode.DeleteMessageResponseCode);
        }

        protected abstract byte[] OnProcess(DbService dbService, NetworkMessage networkMessage, IServerNetworProvider networkProvider);

        public NetworkMessage CreateNetworkMessage<Tsource, Tdto>(Tsource tsource, out Tdto dto, NetworkMessageCode code)
            where Tdto : class
        {
            dto = _mapper.Map<Tdto>(tsource);

            byte[] data = SerializationHelper.Serialize(dto);

            var message = new NetworkMessage(data, code);

            return message;
        }

        protected void SendError<TResponse, TResponseDto>(IServerNetworProvider networkProvider, TResponse response, NetworkMessageCode code)
            where TResponseDto : class
        {
            NetworkMessage responseMessage = CreateNetworkMessage(response, out TResponseDto responseDto, code);

            byte[] responseBytes = SerializationHelper.Serialize(responseMessage);

            _conectionController.BroadcastError(responseBytes, networkProvider);
        }

        protected void PrintReport(int networkProviderId, NetworkMessageCode requestCode, NetworkMessageCode responseCode, string request, NetworkResponseStatus responseStatus)
        {
            ReportPrinter.PrintRequestReport(networkProviderId, requestCode, request);
            ReportPrinter.PrintResponseReport(networkProviderId, responseCode, responseStatus);
        }
    }
}