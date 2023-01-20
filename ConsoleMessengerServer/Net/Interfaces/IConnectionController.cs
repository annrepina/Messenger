using DtoLib.NetworkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMessengerServer.Net.Interfaces
{
    /// <summary>
    /// Интерфейс, который управляет работой с сетью
    /// </summary>
    public interface IConnectionController /*: IServerNetworkMessageHandler*/
    {
        /// <summary>
        /// Обработчик получения сетевых сообщений
        /// </summary>
        public IServerNetworkMessageHandler ServerNetworkMessageHandler { set; }

        /// <summary>
        /// Инициализировать новое подключение
        /// </summary>
        /// <param name="tcpClient">TCP клиент</param>
        public void InitializeNewConnection(TcpClient tcpClient);

        /// <summary>
        /// Отключить всех клиентов
        /// </summary>
        public void DisconnectClients();

        /// <summary>
        /// Отключить конкретного клиента
        /// </summary>
        /// <param name="clientId">Id клиента</param>
        public void DisconnectClient(int clientId);

        /// <summary>
        /// Оповестить о получении массива байтов 
        /// </summary>
        /// <param name="bytes">Массив полученных байтов</param>
        /// <param name="networkProviderId">Id сетевого провайдера</param>
        public void NotifyBytesReceived(byte[] bytes, int networkProviderId);

        public void AddNewSession(int userId, int networkProviderId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userId"></param>
        /// <param name="networkProviderId"></param>
        /// <returns></returns>
        public void SendResponseToVerifiedUser(byte[] response, int userId, int networkProviderId);

        /// <summary>
        /// Отправить ответ сетевому провайдеру за которым не числятся пользователи
        /// </summary>
        /// <param name="response"></param>
        /// <param name="networkProviderId"></param>
        public void SendResponseToNetworkProvider(byte[] response, int networkProviderId);  
    }
}