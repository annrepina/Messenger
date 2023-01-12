using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Responses;

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
        public CreateDialogResponse CreateDialogResponse { get; private set; }

        public DialogCreatedObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
        {
            networkProviderUserDataMediator.DialogCreated += OnDialogCreated;
        }

        /// <summary>
        /// Обработчик события создания диалога
        /// </summary>
        /// <param name="createDialogResponse">Ответ на запрос о создании диалога</param>
        private void OnDialogCreated(CreateDialogResponse createDialogResponse)
        {
            CreateDialogResponse = createDialogResponse;

            _networkMessageHandler.DialogCreated -= OnDialogCreated;

            _completionSource.SetResult();
        }
    }
}
