using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    public enum NetworkResponseStatus : byte
    {
        Successful,
        Failed,
        FatalError
    }
}
