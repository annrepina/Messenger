using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoLib.NetworkServices
{
    public enum NetworkMessageCode : byte
    {
        SignUpRequestCode,
        SignUpResponseCode,
        SignInRequestCode,
        SignInResponseCode,
        SearchUserRequestCode,
        SearchUserResponseCode,
        CreateDialogRequestCode,
        CreateDialogResponseCode,
        SendMessageRequestCode,
        MessageDeliveredCode,
        DeleteMessageRequestCode,
        DeleteMessageResponseCode,
        DeleteDialogRequestCode,
        DeleteDialogResponseCode,
        SignOutRequestCode,
        SignOutResponseCode,
        MessageIsReadRequestCode
    }
}
