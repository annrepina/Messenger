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
    public class SignUpObserver 
    {
        /// <summary>
        /// Свойство - идентификатор зарегистрированного пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Посредник между пользователем и сетью
        /// </summary>
        protected readonly NetworkMessageHandler _networkProviderUserDataMediator;

        /// <summary>
        /// 
        /// </summary>
        protected readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator"></param>
        /// <param _name="completionSource"></param>
        public SignUpObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) /*: base(networkProviderUserDataMediator, completionSource)*/
        {
            _networkProviderUserDataMediator = networkProviderUserDataMediator;
            _completionSource = completionSource;
            _networkProviderUserDataMediator.SignUp += OnSignUp;
        }

        /// <summary>
        /// Обработчик события регистрации пользователя в мессенджере
        /// </summary>
        /// <param name="userId">Идентификатор зарегистрированного пользователя</param>
        private void OnSignUp(int userId)
        {
            UserId = userId;

            _networkProviderUserDataMediator.SignUp -= OnSignUp;

            _completionSource.SetResult();
        }
    }
}
