using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    ///// <summary>
    ///// Обозреватель события получения ответа на запрос об удалении сообщения у класса NetworkMessageHandler
    ///// </summary>
    //public class GotDeleteMessageResponseObserver : Observer<DeleteMessageResponse>
    //{
    //    //public DeleteMessageResponse Response { get; set; }

    //    public GotDeleteMessageResponseObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource, string eventName) : base(networkMessageHandler, completionSource, action)
    //    {
    //        var evetsInfo = networkMessageHandler.GetType().GetEvent()


    //        //_networkMessageHandler.



    //        _networkMessageHandler.DeleteMessageResponseReceived += OnMessageDeleted;
    //    }

    //    protected override void OnEventOccured(DeleteMessageResponse response)
    //    {
            
    //    }

    //    private void OnMessageDeleted(DeleteMessageResponse response)
    //    {

    //        var events = typeof(NetworkMessageHandler).GetEvents();

    //        var method = events[0].EventHandlerType.GetMethod("Invoke");

    //        var param = method.GetParameters();

    //        events.Any(ev => ev.MemberType())

    //        Response = response;

    //        _networkMessageHandler.DeleteMessageResponseReceived -= OnMessageDeleted;

    //        _completionSource.SetResult();
    //    }

    //    //private void OnMessageDeleted()
    //    //{
    //    //    _networkMessageHandler.DeleteMessageResponseReceived -= OnMessageDeleted;

    //    //    _completionSource.SetResult();
    //    //}
    //}
}
