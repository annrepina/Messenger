using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Interfaces
{
    /// <summary>
    /// Интерфейс сериализуемого Dto
    /// </summary>
    public interface ISerializableDto
    {
        /// <summary>
        /// Сериализует себя и возвращает 
        /// </summary>
        /// <returns></returns>
        public byte[] SerializeDto();
    }
}
