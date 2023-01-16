using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Обозреватель события получения ответа на запрос об удалении сообщения у класса NetworkMessageHandler
    /// </summary>
    public class GotDeleteMessageResponseObserver : Observer
    {
        public DeleteMessageResponse Response { get; set; }

        public GotDeleteMessageResponseObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource) : base(networkMessageHandler, completionSource)
        {
            _networkMessageHandler.GotDeleteMessageResponse += OnMessageDeleted;
        }

        private void OnMessageDeleted(DeleteMessageResponse response)
        {
            Response = response;

            _networkMessageHandler.GotDeleteMessageResponse -= OnMessageDeleted;

            _completionSource.SetResult();
        }

        //private void OnMessageDeleted()
        //{
        //    _networkMessageHandler.GotDeleteMessageResponse -= OnMessageDeleted;

        //    _completionSource.SetResult();
        //}
    }
}
