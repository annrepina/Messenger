using Common.NetworkServices.Interfaces;
using System;
using WpfMessengerClient.NetworkMessageProcessing;

namespace WpfMessengerClient.NetworkServices.Interfaces
{
    /// <summary>
    /// Интерфейс, который представляет собой сетевого провайдера в клиентском приложении
    /// Отвечает за подключение к сети и обмен данными в сети
    /// </summary>
    public interface IClientNetworkProvider : INetworkProvider
    {
        /// <summary>
        /// Интерфейс - обработчик сетевых сообщений
        /// </summary>
        public INetworkMessageHandler NetworkMessageHandler { set; }

        /// <summary>
        /// Событие отключения от сети
        /// </summary>
        public event Action Disconnected;
    }
}