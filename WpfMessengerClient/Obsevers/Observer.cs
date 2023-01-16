using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Базовый класс 
    /// </summary>
    public class Observer<T>
    {
        public T Response { get; protected set; }

        /// <summary>
        /// Посредник между пользователем и сетью
        /// </summary>
        protected readonly NetworkMessageHandler _networkMessageHandler;

        /// <summary>
        /// 
        /// </summary>
        protected readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Событие у _networkMessageHandler, на которое нужно подписаться
        /// </summary>
        protected EventInfo _event;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="networkMessageHandler"></param>
        /// <param name="completionSource"></param>
        /// <param name="eventName"></param>
        public Observer(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource, string eventName)
        {
            _networkMessageHandler = networkMessageHandler;
            _completionSource = completionSource;

            _event = _networkMessageHandler.GetType().GetEvent(eventName);

            _event.AddEventHandler(_networkMessageHandler, OnEventOccured);
        }

        protected void OnEventOccured(T response)
        {
            Response = response;

            _event.RemoveEventHandler(_networkMessageHandler, OnEventOccured);

            _completionSource.SetResult();
        }
    }
}