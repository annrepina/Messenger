using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Базовый класс 
    /// </summary>
    public abstract class Observer
    {
        /// <summary>
        /// Посредник между пользователем и сетью
        /// </summary>
        protected readonly NetworkMessageHandler _networkMessageHandler;

        /// <summary>
        /// 
        /// </summary>
        protected readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator"></param>
        /// <param _name="completionSource"></param>
        public Observer(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource)
        {
            _networkMessageHandler = networkProviderUserDataMediator;
            _completionSource = completionSource;
        }
    }
}
