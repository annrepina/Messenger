using System.Threading.Tasks;
using WpfMessengerClient.NetworkMessageProcessing;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Обобщенный класс - обозреватель, который следит за событиями класса NetworkMessageHandler - обработчика сетевых сообщений
    /// События - получения ответов от сервера  
    /// События представлены классом-оберткой NetworkMessageHandlerEvent
    /// </summary>
    /// <typeparam name="TResponse">Тип объекта, который является ответом на запрос серверу</typeparam>
    public class Observer<TResponse>
        where TResponse : class
    {
        /// <summary>
        /// Ответ на запрос серверу
        /// </summary>
        public TResponse Response { get; protected set; }

        /// <summary>
        /// Представляет сторону производителя задач Task<TResult>, не привязанных к делегату
        /// </summary>
        private readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Событие у класса NetworkMessageHandler, на которое нужно подписаться
        /// </summary>
        private readonly NetworkMessageHandlerEvent<TResponse> _event;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="completionSource">Представляет сторону производителя задач Task<TResult>, не привязанных к делегату</param>
        /// <param name="netMessageReceivedEvent">Событие получения сетевого сообщения</param>
        public Observer(TaskCompletionSource completionSource, NetworkMessageHandlerEvent<TResponse> netMessageReceivedEvent)
        {
            _completionSource = completionSource;

            _event = netMessageReceivedEvent;

            _event.EventOccurred += OnEventOccured;
        }

        /// <summary>
        /// Обработчик вызова события
        /// </summary>
        /// <param name="response">Ответ на запрос серверу</param>
        protected void OnEventOccured(TResponse response)
        {
            Response = response;

            _event.EventOccurred -= OnEventOccured;

            _completionSource.SetResult();
        }
    }
}