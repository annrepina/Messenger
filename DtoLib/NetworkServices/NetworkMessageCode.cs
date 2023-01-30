namespace CommonLib.NetworkServices
{
    /// <summary>
    /// Представляет собой код сетевого сообщения
    /// </summary>
    public enum NetworkMessageCode : byte
    {
        SignUpRequestCode,
        SignUpResponseCode,
        SignInRequestCode,
        SignInResponseCode,
        SearcRequestCode,
        SearchResponseCode,
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
        ReadMessagesRequestCode,
        ReadMessagesResponseCode
    }
}