using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс, который представляет сущность диалога
    /// </summary>
    public class Dialog
    {
        /// <summary>
        /// Свойство - Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Свойство - список данных пользователей, участвующих в диалоге
        /// </summary>
        public List<UserData> UsersDataList { get; set; }

        /// <summary>
        /// Свойство - список сообщений
        /// </summary>
        public List<Message> Messages { get; set; }
    }
}