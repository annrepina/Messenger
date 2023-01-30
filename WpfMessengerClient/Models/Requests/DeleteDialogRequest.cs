using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос от сервера для клиентского приложения об удалении диалога 
    /// </summary>
    public class DeleteDialogRequest
    {
        /// <summary>
        /// Id диалога
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="dialogId">Id клиента</param>
        public DeleteDialogRequest(int dialogId)
        {
            DialogId = dialogId;
        }
    }
}