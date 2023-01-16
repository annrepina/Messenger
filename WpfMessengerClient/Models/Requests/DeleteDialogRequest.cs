﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Requests
{
    /// <summary>
    /// Запрос на удаление диалога
    /// </summary>
    public class DeleteDialogRequest
    {
        /// <summary>
        /// Идентификатор диалога, который нужно удалить
        /// </summary>
        public int DialogId { get; init; }

        /// <summary>
        /// Идентификатор пользователя, удалившего диалог
        /// </summary>
        public int UserId { get; init; }

        public DeleteDialogRequest(int dialogId, int userId)
        {
            DialogId = dialogId;
            UserId = userId;
        }
    }
}