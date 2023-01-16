using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    ///// <summary>
    ///// Обозреватель, который следит за событием получения ответа на запрос об удалении диалога
    ///// </summary>
    //public class GotDeleteDialogResponseObserver : Observer
    //{
    //    public DeleteDialogResponse Response { get; set; }

    //    public GotDeleteDialogResponseObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource) : base(networkMessageHandler, completionSource)
    //    {
    //        _networkMessageHandler.DeleteDialogResponseReceived += OnGotDeleteDialogResponse;
    //    }

    //    private void OnGotDeleteDialogResponse(DeleteDialogResponse response)
    //    {
    //        Response = response;

    //        _networkMessageHandler.DeleteDialogResponseReceived -= OnGotDeleteDialogResponse;

    //        _completionSource.SetResult();
    //    }
    //}
}
