﻿using Common.NetworkServices;
using ProtoBuf;

namespace Common.Dto.Responses
{
    /// <summary>
    /// Data transfer object класса, который ответ на запрос серверу от клиента
    /// </summary>
    [ProtoContract]
    public class ResponseDto
    {
        /// <summary>
        /// Статус ответа
        /// </summary>
        [ProtoMember(1)]
        public NetworkResponseStatus Status { get; init; }
    }
}