using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Обозреватель события удаления сообщения у класса NetworkMessageHandler
    /// </summary>
    public class MessageDeletedObserver : Observer
    {
        public MessageDeletedObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource) : base(networkMessageHandler, completionSource)
        {
            _networkMessageHandler.MessageDeletedResponse += OnMessageDeleted;
        }

        private void OnMessageDeleted()
        {
            _networkMessageHandler.MessageDeletedResponse -= OnMessageDeleted;

            _completionSource.SetResult();
        }
    }
}
