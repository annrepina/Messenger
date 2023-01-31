using Common.Dto.Requests;
using Common.NetworkServices;
using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.NetworkMessageProcessing;
using WpfMessengerClient.NetworkServices.Interfaces;
using WpfMessengerClient.Obsevers;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// ViewModel для окна входа
    /// </summary>
    public class SignInWindowViewModel : BaseSignUpSignInViewModel
    {
        /// <inheritdoc cref="Request"/>
        private SignInRequest _request;

        /// <summary>
        /// Запрос на вход в мессенджер
        /// </summary>
        public SignInRequest Request
        {
            get => _request;

            set
            {
                _request = value;

                OnPropertyChanged(nameof(Request));
            }
        }

        /// <summary>
        /// Команда по нажатию кнопки "войти"
        /// </summary>
        public DelegateCommand SignInCommand { get; init; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="windowsManager">Менеджер окон</param>
        /// <param name="networkMessageHandler">Обработчик сетевого сообщения</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public SignInWindowViewModel(WindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider)
            : base(windowsManager, networkMessageHandler, networkProvider)
        {
            SignInCommand = new DelegateCommand(async () => await SignInAsync());

            Request = new SignInRequest();
        }

        /// <summary>
        /// Войти в приложение
        /// </summary>
        private async Task SignInAsync()
        {
            try
            {
                // если ошибок нет
                if (Request.HasNotErrors() == true)
                {
                    AreControlsAvailable = false;

                    TaskCompletionSource completionSource = new TaskCompletionSource();

                    var observer = new Observer<SignInResponse>(completionSource, _networkMessageHandler.SignInResponseReceived);

                    _networkProvider.SendBytesAsync(RequestConverter<SignInRequest, SignInRequestDto>.Convert(Request, NetworkMessageCode.SignInRequestCode));

                    await completionSource.Task;

                    ProcessSignInResponse(observer.Response);

                    AreControlsAvailable = true;
                }

                else
                    MessageBox.Show(Request.GetError());
            }
            catch (Exception)
            {
                CloseWindowAfterError();
                throw;
            }
        }

        /// <summary>
        /// Обрабатывает ответ на запрос о входе в мессенджер
        /// </summary>
        /// <param name="signInResponse">Ответ на запрос о входе</param>
        private void ProcessSignInResponse(SignInResponse signInResponse)
        {
            if (signInResponse.Status == NetworkResponseStatus.Successful)
            {
                _networkProvider.Disconnected -= CloseWindowAfterError;
                _messengerWindowsManager.SwitchToChatWindow(_networkMessageHandler, _networkProvider, signInResponse.User, signInResponse.Dialogs);
            }

            else if (signInResponse.Status == NetworkResponseStatus.Failed)
            {
                if (signInResponse.Context == SignInFailContext.PhoneNumber)
                {
                    Request.PhoneNumber = "";
                    MessageBox.Show("Пользователя с данным телефоном не существует.\nВведите другой номер или зарегистрируйтесь");
                }
                else
                {
                    Request.Password = "";
                    MessageBox.Show("Неверный пароль");
                }
            }

            else
                CloseWindowAfterError();
        }
    }
}