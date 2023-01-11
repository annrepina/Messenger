using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    public enum NetworkMessageCode : byte
    {
        RegistrationCode,
        SuccessfulRegistrationCode,
        RegistrationFailedCode,
        AuthorizationCode,
        SearchUserCode,
        SuccessfulSearchCode,
        SearchFailedCode,
        CreateDialogCode,
        SuccessfulCreatingDialogCode,
        SendingMessageCode,
        ExitCode,
    }
}
