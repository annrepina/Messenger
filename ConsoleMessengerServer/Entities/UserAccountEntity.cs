using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    public class UserAccountEntity
    {
        public int Id { get; set; } 

        public PersonEntity PersonEntity { get; set; }

        public int PersonEntityId { get; set; }

        public string Password { get; set; }

        public bool IsOnline { get; set; }

        public List<DialogEntity> DialogEntities { get; set; }

        public List<FrontClientEntity> FrontClientEntities { get; init; }

        public FrontClientEntity FrontClientEntity { get; set; }

        public int FrontClientEntityId { get; set; }
    }
}
