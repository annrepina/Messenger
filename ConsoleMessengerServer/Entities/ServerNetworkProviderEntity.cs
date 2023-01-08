using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс - сущность сетевого провайдера на стороне сервера
    /// </summary>
    public class ServerNetworkProviderEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Данные о текущем пользователе
        /// </summary>
        public User? User { get; set; }    

        /// <summary>
        /// Идентификатор текущего пользователя
        /// </summary>
        public int? UserId { get; set; } 
    }
}