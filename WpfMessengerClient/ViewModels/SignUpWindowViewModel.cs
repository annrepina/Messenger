using AutoMapper;
using DtoLib.Dto.Requests;
using DtoLib.NetworkServices;
using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Mapping;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.NetworkServices;
using WpfMessengerClient.Obsevers;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель для окна регистрации
    /// </summary>
    public class SignUpWindowViewModel : BaseSignUpSignInViewModel
    {
        /// <summary>
        /// Маппер для мапинга моделей на DTO и обратно
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Команда по нажатию кнопки регистрации
        /// </summary>
        public DelegateCommand SignUpCommand { get; init; }

        /// <summary>
        /// Данные о регистрации нового пользователя
        /// </summary>
        public SignUpRequest Request { get; init; }

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="windowsManager">Менеджер окон в приложении</param>
        public SignUpWindowViewModel(MessengerWindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, /*ConnectionController connectionController*/IClientNetworkProvider networkProvider) 
            : base(windowsManager, networkMessageHandler, /*connectionController*/networkProvider)
        {
            SignUpCommand = new DelegateCommand(async () => await RegisterNewUserAsync());
            Request = new SignUpRequest();

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        #endregion Конструкторы

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        /// <returns></returns>
        private async Task RegisterNewUserAsync()
        {
            try
            {
                //// если ошибок нет
                //if (String.IsNullOrEmpty(SignUpRequest.Error))
                //{

                AreControlsAvailable = false;

                TaskCompletionSource completionSource = new TaskCompletionSource();

                var observer = new Observer<SignUpResponse>(completionSource, _networkMessageHandler.SignUpResponseReceived);

                _networkProvider.SendBytesAsync(RequestConverter<SignUpRequest, SignUpRequestDto>.Convert(Request, _mapper, NetworkMessageCode.SignUpRequestCode));
                //_connectionController.SendRequestAsync(RequestConverter<SignUpRequest, SignUpRequestDto>.Convert(Request, _mapper, NetworkMessageCode.SignUpRequestCode));

                //_networkMessageHandler.SendRequestAsync<SignUpRequest, SignUpRequestDto>(Request, NetworkMessageCode.SignUpRequestCode);

                await completionSource.Task;

                ProcessSignUpResponse(observer.Response);

                AreControlsAvailable = true;

                //}

                //else
                //{
                //    MessageBox.Show(SignUpRequest.Error);
                //}
            }
            catch (Exception)
            {
                CloseWindow();
                throw;
            }
        }

        /// <summary>
        /// Обработать ответ на запрос о регистрации
        /// </summary>
        private void ProcessSignUpResponse(SignUpResponse response)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                User user = _mapper.Map<User>(Request);
                user.Id = response.UserId;

                _messengerWindowsManager.SwitchToChatWindow(_networkMessageHandler, /*_connectionController*/_networkProvider, user);
            }
            else if (response.Status == NetworkResponseStatus.Failed)
            {
                Request.PhoneNumber = "";

                MessageBox.Show("Пользователь с таким телефоном уже существует. Введите другой номер или войдите в мессенджер.");
            }
            else
            {
                MessageBox.Show("Ой, кажется что-то пошло не так.\nМы уже работаем над решением проблемы, попробуйте запустить приложение позже.");
                _messengerWindowsManager.CloseCurrentWindow();
            }
        }
    }
}