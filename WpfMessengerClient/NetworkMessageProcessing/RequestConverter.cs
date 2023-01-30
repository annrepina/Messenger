using AutoMapper;
using CommonLib.NetworkServices;
using CommonLib.Serialization;
using WpfMessengerClient.Models.Mapping;

namespace WpfMessengerClient.NetworkMessageProcessing
{
    /// <summary>
    /// Конвертер запроса для сервера
    /// Конвертирует запрос в массив байт
    /// </summary>
    /// <typeparam name="TRequest">Тип объекта, представляющего запрос для сервера</typeparam>
    /// <typeparam name="TRequestDto">Тип объекта, представляющего DTO запроса на сервер</typeparam>
    public static class RequestConverter<TRequest, TRequestDto>
    {
        /// <summary>
        /// Маппер для мапинга DTO 
        /// </summary>
        private static readonly IMapper _mapper = MessengerMapper.GetInstance().CreateIMapper();

        /// <summary>
        /// Конвертировать запрос в массив байт
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns>Массив байт, представляющий сетевое сообщение</returns>
        public static byte[] Convert(TRequest request, NetworkMessageCode code)
        {
            NetworkMessage networkMessage = ConvertToNetworkMessage(request, code);

            return SerializationHelper.Serialize(networkMessage);
        }

        /// <summary>
        /// Конвертировать запрос в сетевое сообщение
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns>Сетевое сообщение</returns>
        private static NetworkMessage ConvertToNetworkMessage(TRequest request, NetworkMessageCode code)
        {
            TRequestDto dto = _mapper.Map<TRequestDto>(request);

            byte[] data = SerializationHelper.Serialize(dto);

            return new NetworkMessage(data, code);
        }
    }
}