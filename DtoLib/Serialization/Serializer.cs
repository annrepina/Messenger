using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Serialization
{
    /// <summary>
    /// Статический класс - сериализатор
    /// </summary>
    /// <typeparam name="T">Тип класса, объект которого нужно сериализовать</typeparam>
    public static class Serializer<T>
        where T : class
    {
        /// <summary>
        /// Сериализует объект и возвращает массив байтов
        /// </summary>
        /// <returns></returns>
        public static byte[] Serialize(T obj)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    Serializer.Serialize(memoryStream, obj);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не существует протокола сериализации для объекта данного типа");
                throw;
            }
        }
    }
}
