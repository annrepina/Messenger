using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    ///// <summary>
    ///// Класс, который наблюдает за событием SignInResponseReceived у NetworkMessageHandler
    ///// </summary>
    //public class SignInObserver : Observer<SignInResponse>
    //{
    //    //public SignInResponse SignInResponse { get; set; }

    //    //public SignInObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource) : base(networkMessageHandler, completionSource)
    //    //{
    //    //    _networkMessageHandler.SignInResponseReceived += OnEventOccured;
    //    //}

    //    //protected override void OnEventOccured(SignInResponse response)
    //    //{
    //    //    Response = response;

    //    //    _networkMessageHandler.SignInResponseReceived -= OnEventOccured;

    //    //    _completionSource.SetResult();
    //    //}

    //    //private void OnGotSignInResponse(SignInResponse signInResponse)
    //    //{
    //    //    SignInResponse = signInResponse;

    //    //    _networkMessageHandler.SignInResponseReceived -= OnGotSignInResponse;

    //    //    _completionSource.SetResult();
    //    //}
    //}
}
