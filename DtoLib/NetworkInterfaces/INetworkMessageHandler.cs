using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkInterfaces
{
    public interface INetworkMessageHandler
    {
        public Task ProcessNetworkMessageAsync(NetworkMessage message);
    }
}
