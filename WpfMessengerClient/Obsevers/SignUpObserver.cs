using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Класс, который наблюдает за событием SignUp у NetworkMessageHandler
    /// </summary>
    public class SignUpObserver : Observer
    {
        /// <summary>
        /// Свойство - идентификатор зарегистрированного пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator"></param>
        /// <param _name="completionSource"></param>
        public SignUpObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
        {
            _networkMessageHandler.SignUp += OnSignUp;
        }

        /// <summary>
        /// Обработчик события регистрации пользователя в мессенджере
        /// </summary>
        /// <param name="userId">Идентификатор зарегистрированного пользователя</param>
        private void OnSignUp(int userId)
        {
            UserId = userId;

            _networkMessageHandler.SignUp -= OnSignUp;

            _completionSource.SetResult();
        }
    }
}
