using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; }

        public string Text { get; set; }    

        public UserAccountEntity SendingUserAccountEntity { get; set; }

        public UserAccountEntity ReceivingUserEntity { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateTime { get; set; } 
    }
}
