﻿using DtoLib.NetworkServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer
{
    /// <summary>
    /// Класс, который печатает отчет о запросах на сервер и ответах с сервера
    /// </summary>
    public static class ReportPrinter
    {
        public static void PrintRequestReport(int networkProviderId, NetworkMessageCode code, string info)
        {
            Console.WriteLine($"Id клиента: {networkProviderId}. Запрос: код операции: {code}. " + info);
        }

        public static void PrintResponseReport(int networkProviderId, NetworkMessageCode code, NetworkResponseStatus status, string info)
        {
            Console.WriteLine($"Id клиента: {networkProviderId}. Ответ: код операции: {code}. Статус ответа: {status}. " + info);
        }

        public static void PrintResponseReport(int networkProviderId, NetworkMessageCode code, NetworkResponseStatus status)
        {
            Console.WriteLine($"Id клиента: {networkProviderId}. Ответ: код операции: {code}. Статус ответа: {status}.");
        }
    }
}
