using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Обозреватель события DialogCreated у NetworkMessageHandler
    /// </summary>
    public class DialogCreatedObserver : Observer
    {
        /// <summary>
        /// Идентификатор диалога
        /// </summary>
        public int DialogId { get; private set; }

        public DialogCreatedObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
        {
            networkProviderUserDataMediator.DialogCreated += OnDialogCreated;
        }

        /// <summary>
        /// Обработчик события создания диалога
        /// </summary>
        /// <param name="dialogId">Идентификатор диалога</param>
        private void OnDialogCreated(int dialogId)
        {
            DialogId = dialogId;

            _networkMessageHandler.SignUp -= OnDialogCreated;

            _completionSource.SetResult();
        }
    }
}
