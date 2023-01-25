﻿using DtoLib.NetworkServices;
using DtoLib.NetworkServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models.Responses
{
    /// <summary>
    /// Базовый класс ответа на запрос
    /// </summary>
    public class Response : IResponse
    {
        public NetworkResponseStatus Status { get; set; }

        public Response(NetworkResponseStatus status)
        {
            Status = status;
        }

        //public Response()
        //{
        //    Status = NetworkResponseStatus.Successful;
        //}
    }
}
