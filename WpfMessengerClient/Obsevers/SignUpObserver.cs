using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Класс, который наблюдает за событием SignUp у NetworkProviderUserDataMediator
    /// </summary>
    public class SignUpObserver
    {
        private readonly NetworkProviderUserDataMediator _networkProviderUserDataMediator;
        private readonly TaskCompletionSource _completionSource;

        public SignUpObserver(NetworkProviderUserDataMediator networkProviderUserDataMediator, TaskCompletionSource completionSource)
        {
            _networkProviderUserDataMediator = networkProviderUserDataMediator;
            _completionSource = completionSource;
            _networkProviderUserDataMediator.SignUp += OnSignUp;
        }

        private void OnSignUp()
        {
            _completionSource.SetResult();

            _networkProviderUserDataMediator.SignUp -= OnSignUp;
        }
    }
}
