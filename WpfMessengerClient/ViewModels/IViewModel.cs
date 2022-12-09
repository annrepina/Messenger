using DtoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.ViewModels
{
    public interface IViewModel
    {
        public void ProcessNetworkMessage(NetworkMessage message);
    }
}
