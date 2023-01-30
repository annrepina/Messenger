using DtoLib.NetworkServices.Interfaces;
using System;

namespace WpfMessengerClient.NetworkServices.Interfaces
{
    /// <summary>
    /// Интерфейс, который представляет собой сетевого провайдера в клиентском приложении
    /// Отвечает за подключение к сети и обмен данными в сети
    /// </summary>
    public interface IClientNetworkProvider : INetworkProvider
    {
        public NetworkMessageHandler NetworkMessageHandler { set; }

        /// <summary>
        /// Событие отключения от сети
        /// </summary>
        public event Action Disconnected;

        /// <summary>
        /// Событие получения массива байт по сети
        /// </summary>
        public event Action<byte[]> BytesReceived;
    }
}