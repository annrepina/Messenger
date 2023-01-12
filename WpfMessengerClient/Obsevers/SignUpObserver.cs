using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Класс, который наблюдает за событием SignUp у NetworkMessageHandler
    /// </summary>
    public class SignUpObserver : Observer
    {
        /// <summary>
        /// Ответ на запрос о регистрации пользователя
        /// </summary>
        public RegistrationResponse RegistrationResponse { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkMessageHandler"></param>
        /// <param _name="completionSource"></param>
        public SignUpObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource) : base(networkMessageHandler, completionSource)
        {
            _networkMessageHandler.SignUp += OnSignUp;
        }

        /// <summary>
        /// Обработчик события регистрации пользователя в мессенджере
        /// </summary>
        /// <param name="response">Ответ на запрос о регистрации пользователя</param>
        private void OnSignUp(RegistrationResponse response)
        {
            RegistrationResponse = response;

            _networkMessageHandler.SignUp -= OnSignUp;

            _completionSource.SetResult();
        }
    }
}
