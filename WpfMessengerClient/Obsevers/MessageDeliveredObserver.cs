using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int MessageId;

        public MessageDeliveredObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
        {
            _networkMessageHandler.MessageDelivered += OnMessageDelivered;
        }

        /// <summary>
        /// Обработчик события MessageDelivered у NetworkMessageHandler
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения</param>
        private void OnMessageDelivered(int messageId)
        {
            MessageId = messageId;

            _networkMessageHandler.MessageDelivered -= OnMessageDelivered;

            _completionSource.SetResult();
        }
    }
}
