using AutoMapper;
using DtoLib.Dto.Requests;
using DtoLib.NetworkServices;
using Prism.Commands;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Mapping;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.NetworkServices.Interfaces;
using WpfMessengerClient.Obsevers;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// ViewModel для окна регистрации
    /// </summary>
    public class SignUpWindowViewModel : BaseSignUpSignInViewModel
    {
        /// <summary>
        /// Маппер для мапинга DTO
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Команда по нажатию кнопки регистрации
        /// </summary>
        public DelegateCommand SignUpCommand { get; init; }

        /// <summary>
        /// Запрос о регистрации нового пользователя
        /// </summary>
        public SignUpRequest Request { get; init; }

        /// <summary>
        /// Конструктор с параметрамиы
        /// </summary>
        /// <param name="windowsManager">Менеджер окон</param>
        /// <param name="networkMessageHandler">Обработчик сетевого сообщения</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        public SignUpWindowViewModel(WindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider) 
            : base(windowsManager, networkMessageHandler, networkProvider)
        {
            SignUpCommand = new DelegateCommand(async () => await OnSignUpCommand());
            Request = new SignUpRequest();

            MessengerMapper mapper = MessengerMapper.GetInstance();
            _mapper = mapper.CreateIMapper();
        }

        /// <summary>
        /// Обработка команды регистрации нового пользователя
        /// </summary>
        private async Task OnSignUpCommand()
        {
            try
            {
                if (Request.HasNotErrors() == true)
                {
                    AreControlsAvailable = false;

                    TaskCompletionSource completionSource = new TaskCompletionSource();
                    var observer = new Observer<SignUpResponse>(completionSource, _networkMessageHandler.SignUpResponseReceived);

                    _networkProvider.SendBytesAsync(RequestConverter<SignUpRequest, SignUpRequestDto>.Convert(Request, NetworkMessageCode.SignUpRequestCode));

                    await completionSource.Task;

                    ProcessSignUpResponse(observer.Response);

                    AreControlsAvailable = true;
                }

                else
                    MessageBox.Show(Request.GetError());
            }
            catch (Exception)
            {
                CloseWindow();
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

                _messengerWindowsManager.SwitchToChatWindow(_networkMessageHandler, _networkProvider, user);
            }
            else if (response.Status == NetworkResponseStatus.Failed)
            {
                Request.PhoneNumber = "";

                MessageBox.Show("Пользователь с таким телефоном уже существует. Введите другой номер или войдите в мессенджер.");
            }
            else
                CloseWindow();
        }      
    }
}