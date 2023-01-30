using Prism.Commands;
using WpfMessengerClient.NetworkServices;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Базовый класс для ViewModel окон входа и регистрации
    /// </summary>
    public class BaseSignUpSignInViewModel : BaseViewModel
    {
        /// <summary>
        /// Команда по нажатию кнопки "назад"
        /// </summary>
        public DelegateCommand BackCommand { get; init; }


        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="windowsManager">Менеджер окон</param>
        /// <param name="networkMessageHandler">Обработчик сетевых сообщений</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public BaseSignUpSignInViewModel(WindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider)
            : base(windowsManager, networkMessageHandler, networkProvider)
        {
            BackCommand = new DelegateCommand(GoBack);
        }

        /// <summary>
        /// Вернуться назад в предыдущее окно
        /// </summary>
        private void GoBack()
        {
            _messengerWindowsManager.ReturnToStartWindow();
        }
    }
}