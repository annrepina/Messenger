namespace CommonLib.NetworkServices
{
    /// <summary>
    /// Представляет статус ответа за запрос о стевом сообщении
    /// </summary>
    public enum NetworkResponseStatus : byte
    {
        Successful,
        Failed,
        FatalError
    }
}