using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    public class UserAccount
    {
        public int Id { get; set; } 

        public Person Person { get; set; }

        public int PersonId { get; set; }

        public string Password { get; set; }

        public bool IsOnline { get; set; }

        public List<Dialog> Dialogs { get; set; }

        public List<Client> FrontClients { get; init; }

        public Client FrontClient { get; set; }

        public int FrontClientId { get; set; }
    }
}
