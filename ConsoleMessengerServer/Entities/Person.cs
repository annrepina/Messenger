using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    /// <summary>
    /// Класс - сущность Person
    /// </summary>
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string? Surname { get; set; }

        public string PhoneNumber { get; set; }

        public UserAccount? UserAccount { get; set; }
    }
}
