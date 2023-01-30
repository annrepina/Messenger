using AutoMapper;
using Common.NetworkServices;
using Common.Serialization;
using ConsoleMessengerServer.Entities.Mapping;

namespace ConsoleMessengerServer.RequestProcessing
{
    /// <summary>
    /// Статический конвертер сетевого сообщения для клиента в массив байт
    /// </summary>
    /// <typeparam name="TMessageData">Тип объекта, представляющего данные для сетевого сообщения</typeparam>
    /// <typeparam name="TMessageDataDto">Тип DTO - представляющего данные для сетевого сообщения</typeparam>
    public static class NetworkMessageConverter<TMessageData, TMessageDataDto>
    {
        /// <summary>
        /// Маппер для мапинга DTO
        /// </summary>
        private static readonly IMapper _mapper = ServerMapper.GetInstance().CreateIMapper();

        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <param name="messageData">Данные для сетевого сообщения</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns>Сетевое сообщение в виде массива байт</returns>
        public static byte[] Convert(TMessageData messageData, NetworkMessageCode code)
        {
            NetworkMessage networkMessage = ConvertToNetworkMessage(messageData, code);

            return SerializationHelper.Serialize(networkMessage);
        }

        /// <summary>
        /// Конвертировать данные в сетевое сообщение
        /// </summary>
        /// <param name="messageData">Данные для сетевого сообщения</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns>Сетевое сообщение</returns>
        private static NetworkMessage ConvertToNetworkMessage(TMessageData messageData, NetworkMessageCode code)
        {
            TMessageDataDto dto = _mapper.Map<TMessageDataDto>(messageData);

            byte[] data = SerializationHelper.Serialize(dto);

            return new NetworkMessage(data, code);
        }
    }
}