using DtoLib.NetworkServices;
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
        public static void PrintRequestReport(NetworkMessageCode code, string info)
        {
            Console.WriteLine($"Запрос: код операции: {code}. " + info);
        }

        public static void PrintResponseReport(NetworkMessageCode code, NetworkResponseStatus status, string info)
        {
            Console.WriteLine($"Код операции: {code}. Статус операции: {status}. " + info);
        }
    }
}
