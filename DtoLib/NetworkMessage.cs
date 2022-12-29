using DtoLib.Dto;
using DtoLib.Interfaces;
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
    public class NetworkMessage /*: Serializable, IDeserializableDto*/
    {
        //[ProtoMember(1)]
        //public Serializable SerializableDto { get; set; }

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

        public NetworkMessage()
        {
        }

        public NetworkMessage(byte[]? data, OperationCode operationCode)
        {
            //SerializableDto = serializable;
            CurrentCode = operationCode;
            Data = data;
        }

        //public IDeserializableDto Deserialize(byte[] buffer)
        //{
        //    try
        //    {
        //        using (var stream = new MemoryStream(buffer))
        //        {
        //            var obj = Serializer.Deserialize<NetworkMessage>(stream);
        //            return obj;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //
        //        throw;
        //    }
        //}

        //public override byte[] SerializeDto()
        //{
        //    try
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            Serializer.Serialize(memoryStream, this);
        //            return memoryStream.ToArray();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //        throw;
        //    }
        //}
    }
}
