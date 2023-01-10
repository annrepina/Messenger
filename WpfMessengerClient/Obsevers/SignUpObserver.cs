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
    public class SignUpObserver : Observer
    {
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator"></param>
        /// <param _name="completionSource"></param>
        public SignUpObserver(NetworkProviderUserDataMediator networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
        {
            _networkProviderUserDataMediator.SignUp += OnEventOccured;
        }

        protected override void OnEventOccured()
        {
            _completionSource.SetResult();

            _networkProviderUserDataMediator.SignUp -= OnEventOccured;
        }
    }
}
