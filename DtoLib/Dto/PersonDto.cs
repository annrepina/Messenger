using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DtoLib.Interfaces;
using ProtoBuf;

namespace DtoLib.Dto
{
    /// <summary>
    /// Data transfer object класса Person
    /// </summary>
    [ProtoContract]
    public class PersonDto : Serializable/*, IDeserializableDto*/
    {
        /// <summary>
        /// Свойство - имя
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// Свойство - фамилия
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(2)]
        public string? Surname { get; set; }

        /// <summary>
        /// Свойство - номер телефона
        /// Атрибут - для сереализации/десереализации, задает интовый идентификатор для свойства
        /// </summary>
        [ProtoMember(3)]
        public string PhoneNumber { get; set; }

        //public IDeserializableDto Deserialize(byte[] buffer)
        //{
        //    try
        //    {
        //        using(var stream = new MemoryStream(buffer))
        //        {
        //            var obj = Serializer.Deserialize<PersonDto>(stream);
        //            return obj;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //
        //        throw;
        //    }
        //}

        //public byte[] SerializeDto()
        //{
        //    try
        //    {
        //        using(var memoryStream = new MemoryStream())
        //        {
        //            Serializer.Serialize(memoryStream, this);
        //            return memoryStream.ToArray();
        //        }
        //    }
        //    catch (SerializationException ex)
        //    {
        //        //
        //        throw;
        //    }
        //}
    }
}
