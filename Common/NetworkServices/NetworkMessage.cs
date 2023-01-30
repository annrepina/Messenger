using ProtoBuf;

namespace Common.NetworkServices
{
    /// <summary>
    /// Сетевое сообщение, которое может отправляться от клиентского приложения к серверному и обратно
    /// </summary>
    [ProtoContract]
    public class NetworkMessage
    {
        /// <summary>
        /// Код сетевого сообщения
        /// </summary>
        [ProtoMember(1)]
        public NetworkMessageCode Code { get; init; }

        /// <summary>
        /// Данные, хранящиеся в сетевом сообщении
        /// </summary>
        [ProtoMember(2)]
        public byte[]? Data { get; init; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public NetworkMessage()
        {
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="data">Данные, хранящиеся в сетевом сообщении</param>
        /// <param name="code">Код сетевого сообщения</param>
        public NetworkMessage(byte[]? data, NetworkMessageCode code)
        {
            Code = code;
            Data = data;
        }
    }
}