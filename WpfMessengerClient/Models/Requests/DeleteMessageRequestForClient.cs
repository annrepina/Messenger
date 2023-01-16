using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запррос на удаление сообщения для клиента
    /// </summary>
    public class DeleteMessageRequestForClient
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Идентификатор диалога, в котором существует сообщение
        /// </summary>
        public int DialogId { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DeleteMessageRequestForClient()
        {
            MessageId = 0;
            DialogId = 0;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="messageId">Id сообщения</param>
        /// <param name="dialogId">Id диалога</param>
        public DeleteMessageRequestForClient(int messageId, int dialogId)
        {
            MessageId = messageId;
            DialogId = dialogId;
        }
    }
}
