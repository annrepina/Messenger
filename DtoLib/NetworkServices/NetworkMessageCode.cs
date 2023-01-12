using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    public enum NetworkMessageCode : byte
    {
        RegistrationRequestCode,
        RegistrationResponseCode,
        AuthorizationCode,
        SuccessfulAuthorizationCode,
        AuthorizationFailedCode,
        SearchUserRequestCode,
        SearchUserResponseCode,
        CreateDialogCode,
        SuccessfulCreatingDialogCode,
        SendMessageCode,
        MessageDeliveredCode,
        ExitCode,
    }
}
