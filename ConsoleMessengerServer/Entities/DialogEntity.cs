using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Entities
{
    public class DialogEntity
    {
        public int Id { get; set; }

        public UserAccountEntity UserAccountEntity1 { get; set; }

        public UserAccountEntity UserAccountEntity2 { get; set; }

        public List<MessageEntity> MessagesEntities { get; set; }
    }
}
