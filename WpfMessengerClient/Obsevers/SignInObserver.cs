using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Класс, который наблюдает за событием GotSignInResponse у NetworkMessageHandler
    /// </summary>
    public class SignInObserver : Observer
    {
        public SignInResponse SignInResponse { get; set; }

        public SignInObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource) : base(networkMessageHandler, completionSource)
        {
            _networkMessageHandler.GotSignInResponse += OnGotSignInResponse;
        }

        private void OnGotSignInResponse(SignInResponse signInResponse)
        {
            SignInResponse = signInResponse;

            _networkMessageHandler.GotSignInResponse -= OnGotSignInResponse;

            _completionSource.SetResult();
        }
    }
}
