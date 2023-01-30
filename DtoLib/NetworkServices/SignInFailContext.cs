namespace CommonLib.NetworkServices
{
    /// <summary>
    /// Контекст ошибки во время входа в мессенджер
    /// </summary>
    public enum SignInFailContext : byte
    {
        PhoneNumber,
        Password
    }
}