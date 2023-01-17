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
    public class Observer<TResponse>
        where TResponse : class
    {
        public TResponse Response { get; protected set; }

        ///// <summary>
        ///// Посредник между пользователем и сетью
        ///// </summary>
        //protected readonly NetworkMessageHandler _networkMessageHandler;

        /// <summary>
        /// 
        /// </summary>
        private readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Событие у _networkMessageHandler, на которое нужно подписаться
        /// </summary>
        private NetworkMessageHandlerEvent<TResponse> _event;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="completionSource"></param>
        /// <param name="eventName"></param>
        public Observer(TaskCompletionSource completionSource, NetworkMessageHandlerEvent<TResponse> eventWrapper)
        {
            _completionSource = completionSource;

            _event = eventWrapper;

            _event.ResponseReceived += OnEventOccured;
        }

        protected void OnEventOccured(TResponse response)
        {
            Response = response;

            _event.ResponseReceived -= OnEventOccured;

            _completionSource.SetResult();
        }
    }
}