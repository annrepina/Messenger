using DtoLib.Dto;
using DtoLib.Serialization;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
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

        //public enum OperationCode : byte
        //{
        //    RegistrationCode,
        //    SuccessfulRegistrationCode,
        //    RegistrationFailedCode,
        //    AuthorizationCode,
        //    SearchUserCode,
        //    SuccessfulSearchCode,
        //    SearchFailedCode,
        //    SendingMessageCode,
        //    ExitCode,
        //}

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