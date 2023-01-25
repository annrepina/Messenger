using DtoLib.Dto.Requests;
using DtoLib.NetworkServices;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.Obsevers;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель для окна входа
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
        /// <param name="messengerWindowsManager">Менеджер окон в приложении</param>
        public SignInWindowViewModel(MessengerWindowsManager messengerWindowsManager, NetworkMessageHandler networkMessageHandler) : base(messengerWindowsManager, networkMessageHandler)
        {
            SignInCommand = new DelegateCommand(async () => await SignInAsync());

            Request = new SignInRequest();
        }

        /// <summary>
        /// Войти в приложение
        /// </summary>
        private async Task SignInAsync()
        {
            //// если ошибок нет
            //if (String.IsNullOrEmpty(SignUpRequest.Error))
            //{

            AreControlsAvailable = false;

            TaskCompletionSource completionSource = new TaskCompletionSource();

            var observer = new Observer<SignInResponse>(completionSource, _networkMessageHandler.SignInResponseReceived);

            _networkMessageHandler.SendRequestAsync<SignInRequest, SignInRequestDto>(Request, NetworkMessageCode.SignInRequestCode);

            await completionSource.Task;

            ProcessSignInResponse(observer.Response);

            AreControlsAvailable = true;

            //}

            //else
            //{
            //    MessageBox.Show(SignUpRequest.Error);
            //}
        }

        /// <summary>
        /// Обрабатывает ответ на запрос о входе в мессенджер
        /// </summary>
        /// <param name="signInResponse">Ответ на запрос о входе</param>
        private void ProcessSignInResponse(SignInResponse signInResponse)
        {
            if(signInResponse.Status == NetworkResponseStatus.Successful)
            {
                _messengerWindowsManager.SwitchToChatWindow(_networkMessageHandler, signInResponse.User, signInResponse.Dialogs);
            }
            else if(signInResponse.Status == NetworkResponseStatus.Failed)
            {
                if(signInResponse.Context == SignInFailContext.PhoneNumber)
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
            {
                MessageBox.Show("Ой, кажется что-то пошло не так.\nМы уже работаем над решением проблемы, попробуйте запустить приложение позже.");
                _messengerWindowsManager.CloseCurrentWindow();
            }
        }

        /// <summary>
        /// Изменить окно на окно чата
        /// </summary>
        public void SwitchToChatWindow(User user, List<Dialog> dialogs)
        {
            _messengerWindowsManager.SwitchToChatWindow(_networkMessageHandler, user, dialogs);
        }
    }
}