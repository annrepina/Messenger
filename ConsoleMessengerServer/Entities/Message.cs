using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }    

        public UserAccount SendingUserAccount { get; set; }

        public UserAccount ReceivingUserAccount { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateTime { get; set; } 
    }
}
