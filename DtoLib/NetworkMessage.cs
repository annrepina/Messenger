using DtoLib.Dto;
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
    public class NetworkMessage
    {
        public IDataTransferObject DataTransferObject { get; set; }

        public OperationCode CurrentCode { get; set;}

        public enum OperationCode
        {
            Registration,
            Authorization
        }
    }
}
