using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса NetworkProvider
    /// </summary>
    [ProtoContract]
    public class NetworkProviderDto
    {
        /// <summary>
        /// Свойство - идентификатор
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }
    }
}