using System;

namespace WpfMessengerClient
{
    /// <summary>
    /// Класс, который является обёрткой над событием в классе NetworkMessageHandler - обработчике сетевых сообщений
    /// </summary>
    /// <typeparam name="TData">Данные, хранящиеся в сетевом сообщении</typeparam>
    public class NetworkMessageHandlerEvent<TData>
        where TData : class
    {
        /// <summary>
        /// Событие - сетевое сообщение получено
        /// </summary>
        public event Action<TData> EventOccurred;

        /// <summary>
        /// Вызов обработчиков события
        /// </summary>
        /// <param name="data">Данные, хранящиеся в сетевом сообщении</param>
        public void Invoke(TData data)
        {
            EventOccurred?.Invoke(data);
        }
    }
}