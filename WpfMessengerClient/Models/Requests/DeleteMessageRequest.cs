using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Данные, представляющие запрос на удаление сообщения
    /// </summary>
    public class DeleteMessageRequest
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
        /// Идентификатор пользователя, удалившего сообщение
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DeleteMessageRequest()
        {
            MessageId = 0;
            DialogId = 0;
            UserId = 0;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="messageId">Идентификатор диалога</param>
        /// <param name="dialogId">Идентификатор диалога</param>
        /// <param name="userId">Идентификатор диалога</param>
        public DeleteMessageRequest(int messageId, int dialogId, int userId)
        {
            MessageId = messageId;
            DialogId = dialogId;
            UserId = userId;
        }
    }
}