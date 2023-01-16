using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    ///// <summary>
    ///// Обозреватель события SendMessageResponseReceived у NetworkMessageHandler
    ///// </summary>
    //public class MessageDeliveredObserver : Observer<SendMessageResponse>
    //{
    //    /// <summary>
    //    /// Идентификатор сообщения
    //    /// </summary>
    //    public SendMessageResponse SendMessageResponse;

    //    public MessageDeliveredObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
    //    {
    //        _networkMessageHandler.SendMessageResponseReceived += OnEventOccured;
    //    }

    //    protected override void OnEventOccured(SendMessageResponse response)
    //    {
    //        Response = response;

    //        _networkMessageHandler.SendMessageResponseReceived -= OnEventOccured;

    //        _completionSource.SetResult();
    //    }

    //    ///// <summary>
    //    ///// Обработчик события SendMessageResponseReceived у NetworkMessageHandler
    //    ///// </summary>
    //    ///// <param name="messageId">Идентификатор сообщения</param>
    //    //private void OnMessageDelivered(SendMessageResponse sendMessageResponse)
    //    //{
    //    //    SendMessageResponse = sendMessageResponse;

    //    //    _networkMessageHandler.SendMessageResponseReceived -= OnMessageDelivered;

    //    //    _completionSource.SetResult();
    //    //}
    //}
}
