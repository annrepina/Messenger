using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient
{
    /// <summary>
    /// Строитель для обработчика сетевых сообщений
    /// </summary>
    public class NetworkMessageHandlerBuilder : Builder<NetworkMessageHandler>
    {
        public override void Build()
        {
            SetConnectionController();
        }

        public NetworkMessageHandlerBuilder() : base()
        {

        }

        public void SetConnectionController()
        {
            ConnectionController connectionController = new ConnectionController();

            _element.ConnectionController = connectionController;

            connectionController.NetworkMessageHandler = _element;
        }
    }
}
