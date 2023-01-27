using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient;
using WpfMessengerClient.ViewModels;

namespace WpfChatClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Свойство - Менеджер окон приложения
        /// </summary>
        public MessengerWindowsManager MessengerWindowsManager { get; set; }

        /// <summary>
        /// Переопределение метода
        /// </summary>
        /// <param _name="e">Содержит аргументы события StartUp</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MessengerWindowsManager = new MessengerWindowsManager(this);

            MessengerWindowsManager.OpenStartWindow();
        }
    }
}
