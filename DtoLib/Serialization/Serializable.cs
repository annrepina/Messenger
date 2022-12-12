using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Serialization
{
    /// <summary>
    /// Абстрактный класс, который может сереализовать себя
    /// </summary>
    [ProtoContract]
    public abstract class Serializable
    {
        /// <summary>
        /// Сериализует себя и возвращает массив байтов
        /// </summary>
        /// <returns></returns>
        public virtual byte[] SerializeDto()
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    Serializer.Serialize(memoryStream, this);
                    return memoryStream.ToArray();
                }
            }
            catch (SerializationException ex)
            {
                //
                throw;
            }
        }
    }
}
