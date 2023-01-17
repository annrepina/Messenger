using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.Dto.Requests
{
    /// <summary>
    /// Data transfer object для класса ExitRequest
    /// </summary>
    public class ExitRequestDto
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int UserId { get; set; }
    }
}