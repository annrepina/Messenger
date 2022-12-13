using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    public class Dialog
    {
        public int Id { get; set; }

        //public UserAccount UserAccount1 { get; set; }

        //public UserAccount UserAccount2 { get; set; }

        public List<UserAccount> UserAccounts { get; set; }

        public List<Message> Messages { get; set; }
    }
}
