using DtoLib.Dto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Serialization
{
    /// <summary>
    /// Статический класс - десериализатор
    /// </summary>
    public static class Deserializer
    {
        /// <summary>
        /// Десериализует массив байтов в объект типа T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">данные в виде массива байтов</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
            where T : class
        {
            try
            {
                using (var stream = new MemoryStream(data))
                {
                    T obj = Serializer.Deserialize<T>(stream);
                    return obj;
                }
            }
            catch (Exception)
            {
                //
                throw;
            }
        }
    }
}
