using AutoMapper;
using DtoLib.NetworkServices;
using DtoLib.Serialization;

namespace WpfMessengerClient
{
    public static class RequestConverter<TRequest, TRequestDto>
    {
        ///// <summary>
        ///// Маппер для мапинга моделей на DTO и обратно
        ///// </summary>
        //private static readonly IMapper _mapper = ;

        public static byte[] Convert(TRequest request, IMapper mapper, NetworkMessageCode code)
        {
            NetworkMessage networkMessage = ConvertToNetworkMessage(request, mapper, code);

            return SerializationHelper.Serialize(networkMessage);
        }

        private static NetworkMessage ConvertToNetworkMessage(TRequest request, IMapper mapper, NetworkMessageCode code)
        {
            TRequestDto dto = mapper.Map<TRequestDto>(request);

            byte[] data = SerializationHelper.Serialize(dto);

            return new NetworkMessage(data, code);
        }
    }
}