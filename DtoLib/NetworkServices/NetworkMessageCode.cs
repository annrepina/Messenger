using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.NetworkServices
{
    public enum NetworkMessageCode : byte
    {
        SignUpRequestCode,
        SignUpResponseCode,
        SignInRequestCode,
        SignInResponseCode,
        SearcRequestCode,
        SearchUserResponseCode,
        CreateDialogRequestCode,
        CreateDialogResponseCode,
        SendMessageRequestCode,
        SendMessageResponseCode,
        DeleteMessageRequestCode,
        DeleteMessageResponseCode,
        DeleteDialogRequestCode,
        DeleteDialogResponseCode,
        SignOutRequestCode,
        SignOutResponseCode,
        MessagesAreReadRequestCode,
        MessagesAreReadResponseCode
    }
}
