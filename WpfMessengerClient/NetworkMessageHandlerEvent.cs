using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient
{
    /// <summary>
    /// Класс, который является оберткой над событием в классе NetworkMessageHandler
    /// </summary>
    public class NetworkMessageHandlerEvent<TResponse>
        where TResponse : class
    {
        public event Action<TResponse> ResponseReceived;

        public void Invoke(TResponse response)
        {
            ResponseReceived?.Invoke(response);
        }
    }
}
