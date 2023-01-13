using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Обозреватель события MessageDelivered у NetworkMessageHandler
    /// </summary>
    public class MessageDeliveredObserver : Observer
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public SendMessageResponse SendMessageResponse;

        public MessageDeliveredObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
        {
            _networkMessageHandler.MessageDelivered += OnMessageDelivered;
        }

        /// <summary>
        /// Обработчик события MessageDelivered у NetworkMessageHandler
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения</param>
        private void OnMessageDelivered(SendMessageResponse sendMessageResponse)
        {
            SendMessageResponse = sendMessageResponse;

            _networkMessageHandler.MessageDelivered -= OnMessageDelivered;

            _completionSource.SetResult();
        }
    }
}
