using ProtoBuf;

namespace Common.Serialization
{
    /// <summary>
    /// Класс, который осуществляет сериализацию/десериализацию объектов с помощью Protobuf
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Десериализует массив байтов в объект типа T
        /// </summary>
        /// <typeparam name="T">Тип класса, объект которого возвращает метод</typeparam>
        /// <param name="data">Данные в виде массива байтов</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
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
                Console.WriteLine("Массив байтов невозможно десериализовать");
                throw;
            }
        }

        /// <summary>
        /// Сериализует объект и возвращает массив байтов
        /// </summary>
        /// <typeparam name="T">Тип класса, объект которого сериализует метод</typeparam>
        /// <param name="obj">Объект, который сериализует метод</param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T obj)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    Serializer.Serialize(memoryStream, obj);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Не существует протокола сериализации для объекта данного типа");
                throw;
            }
        }
    }
}