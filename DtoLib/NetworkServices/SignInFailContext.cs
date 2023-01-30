using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.NetworkServices
{
    /// <summary>
    /// Контекстошибки во время входа в мессенджер
    /// </summary>
    public enum SignInFailContext : byte
    {
        PhoneNumber,
        Password
    }
}
