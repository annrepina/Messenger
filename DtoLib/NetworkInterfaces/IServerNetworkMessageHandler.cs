using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkInterfaces
{
    public interface IServerNetworkMessageHandler : INetworkMessageHandler
    {
        public void ProcessNetworkMessage(NetworkMessage message, int networkProviderId);
    }
}
