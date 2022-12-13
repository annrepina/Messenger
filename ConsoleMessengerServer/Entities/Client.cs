using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public UserAccount? UserAccount { get; set; }    

        public int? UserAccountId { get; set; } 
    }
}
