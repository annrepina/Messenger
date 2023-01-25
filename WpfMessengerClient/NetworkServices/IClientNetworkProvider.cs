using DtoLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.NetworkServices
{
    public interface IClientNetworkProvider : INetworkProvider
    {
        public event Action Disconnected;
    }
}
