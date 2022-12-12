using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Serialization
{
    public class Serializator<T>
        where T : class
    {
        /// <summary>
        /// Сериализует объект и возвращает массив байтов
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Serialize(T obj)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    Serializer.Serialize(memoryStream, obj);

                    //var byteArray = memoryStream.ToArray();

                    //byteArray.Where()

                    //return memoryStream.ToArray();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                //
                throw;
            }
        }
    }
}
