using DtoLib.Dto;
using DtoLib.Serialization;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib
{
    /// <summary>
    /// Сетевое сообщение, которое будет отправляться от клиентского приложения к серверному и обратно
    /// </summary>
    [ProtoContract]
    public class NetworkMessage 
    {
        [ProtoMember(1)]
        public OperationCode CurrentCode { get; set;}

        [ProtoMember(2)]
        public byte[]? Data { get; set;}

        public enum OperationCode : byte
        {
            RegistrationCode,
            SuccessfulRegistrationCode,
            RegistrationFailedCode,
            AuthorizationCode,
            SendingMessageCode,
            ExitCode,
        }

        public NetworkMessage(byte[]? data, OperationCode operationCode)
        {
            CurrentCode = operationCode;
            Data = data;
        }
    }
}