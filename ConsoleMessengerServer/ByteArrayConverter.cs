using AutoMapper;
using ConsoleMessengerServer.Entities.Mapping;
using CommonLib.NetworkServices;
using CommonLib.Serialization;

namespace ConsoleMessengerServer
{
    public static class ByteArrayConverter<TResponse, TResponseDto>
    {
        /// <summary>
        /// Маппер для мапинга моделей на DTO и обратно
        /// </summary>
        private static readonly IMapper _mapper = DataBaseMapper.GetInstance().CreateIMapper();

        public static byte[] Convert(TResponse response, NetworkMessageCode code)
        {
            NetworkMessage networkMessage = ConvertToNetworkMessage(response, code);

            return SerializationHelper.Serialize(networkMessage);
        }

        private static NetworkMessage ConvertToNetworkMessage(TResponse response, NetworkMessageCode code)
        {
            TResponseDto dto = _mapper.Map<TResponseDto>(response);

            byte[] data = SerializationHelper.Serialize(dto);

            return new NetworkMessage(data, code);
        }
    }
}