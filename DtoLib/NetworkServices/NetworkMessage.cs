using CommonLib.Dto;
using CommonLib.Serialization;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.NetworkServices
{
    /// <summary>
    /// Сетевое сообщение, которое будет отправляться от клиентского приложения к серверному и обратно
    /// </summary>
    [ProtoContract]
    public class NetworkMessage
    {
        [ProtoMember(1)]
        public NetworkMessageCode Code { get; set; }

        [ProtoMember(2)]
        public byte[]? Data { get; set; }

        public NetworkMessage()
        {

        }

        public NetworkMessage(byte[]? data, NetworkMessageCode code)
        {
            Code = code;
            Data = data;
        }
    }
}