using DtoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.ViewModels;
using DtoLib.NetworkServices;

namespace WpfMessengerClient.Services
{
    //public class ConnectionService
    //{
    //    ///// <summary>
    //    ///// Клиент приложения
    //    ///// </summary>
    //    //public FrontClient Client { get; init; }

    //    /////// <summary>
    //    /////// Получатель сообщений от сервера
    //    /////// </summary>
    //    ////public Receiver Receiver { get; init; }

    //    /////// <summary>
    //    /////// Отправитель сообщений на сервер
    //    /////// </summary>
    //    ////public Sender Sender { get; init; }

    //    //public IViewModel ViewModel { get; set; }

    //    public ConnectionService(IViewModel viewModel)
    //    {
    //        Client = new FrontClient();
    //        //Receiver = new Receiver(Client);
    //        //Sender = new Sender(Client);
    //        ViewModel = viewModel;
    //        Client.OnNetworkMessageGot += SendMessageToViewModel;
    //    }

    //    public async Task ConnectAsync(NetworkMessage message)
    //    {
    //        await Client.ConnectAsync(message);
    //    }

    //    public async Task SendMessageToViewModel(NetworkMessage message)
    //    {
    //        await Task.Run(() => ViewModel.ProcessNetworkMessage(message));
    //    }
    //}
}
