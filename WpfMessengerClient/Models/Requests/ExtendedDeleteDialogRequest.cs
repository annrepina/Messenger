using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Расширенный запрос на удаление диалога от клиента серверу
    /// </summary>
    public class ExtendedDeleteDialogRequest
    {
        /// <summary>
        /// Идентификатор диалога, который нужно удалить
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор пользователя, удалившего диалог
        /// </summary>
        public int UserId { get; init; }
        
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="dialogId">Id диалога</param>
        /// <param name="userId">Id пользователя</param>
        public ExtendedDeleteDialogRequest(int dialogId, int userId)
        {
            DialogId = dialogId;
            UserId = userId;
        }
    }
}