using System.ComponentModel;
using System.Windows;
using WpfMessengerClient.NetworkMessageProcessing;
using WpfMessengerClient.NetworkServices.Interfaces;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Базовый класс ViewModel
    /// </summary>
    public class BaseViewModel : BaseNotifyPropertyChanged
    {
        #region Protected поля

        /// <inheritdoc cref="AreControlsAvailable"/>
        protected bool _areControlsAvailable;

        /// <summary>
        /// Обработчик сетевого сообщения
        /// </summary>
        protected readonly NetworkMessageHandler _networkMessageHandler;

        /// <summary>
        /// Менеджер окон приложения
        /// </summary>
        protected readonly WindowsManager _messengerWindowsManager;

        /// <summary>
        /// Сетевой провайдер, отвечающий за передачу данных по сети
        /// </summary>
        protected readonly IClientNetworkProvider _networkProvider;

        #endregion Protected поля

        /// <summary>
        /// Доступны ли элементы управления на View
        /// </summary>
        public bool AreControlsAvailable
        {
            get => _areControlsAvailable;

            set
            {
                _areControlsAvailable = value;

                OnPropertyChanged(nameof(AreControlsAvailable));
            }
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="windowsManager">Менеджер окон приложения</param>
        /// <param name="networkMessageHandler">Обработчик сетевого сообщения</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public BaseViewModel(WindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider)
        {
            _messengerWindowsManager = windowsManager;
            _networkMessageHandler = networkMessageHandler;
            _networkProvider = networkProvider;
            _networkProvider.Disconnected += CloseWindowAfterError;
            AreControlsAvailable = true;
        }

        /// <summary>
        /// Закрывает текущее окно после выявления ошибки
        /// </summary>
        protected void CloseWindowAfterError()
        {
            MessageBox.Show("Ой, кажется что-то пошло не так.\nМы уже работаем над решением проблемы, попробуйте запустить приложение позже.");

            _networkProvider.CloseConnection();

            Application.Current.Dispatcher.Invoke(() => _messengerWindowsManager.CloseCurrentWindow());
        }

        /// <summary>
        /// Обработчик события закрытия окна чатов
        /// </summary>
        /// <param name="sender">Объект вызвавший события</param>
        /// <param name="e">Содержит данные о событии</param>
        public void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            _networkProvider.Disconnected -= CloseWindowAfterError;

            _networkProvider.CloseConnection();

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}