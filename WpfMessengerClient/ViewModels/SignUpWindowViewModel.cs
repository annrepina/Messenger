using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Mapping;
using AutoMapper;
using DtoLib.Dto;
using WpfMessengerClient.Services;
using DtoLib;
using DtoLib.Serialization;
using WpfMessengerClient.Obsevers;
using System.Windows;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using DtoLib.NetworkServices;
using DtoLib.Dto.Requests;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель для окна регистрации
    /// </summary>
    public class SignUpWindowViewModel : BaseSignUpSignInViewModel
    {
        /// <summary>
        /// Команда по нажатию кнопки регистрации
        /// </summary>
        public DelegateCommand OnSignUpCommand { get; init; }

        /// <summary>
        /// Данные о регистрации нового пользователя
        /// </summary>
        public SignUpRequest Request { get; init; }

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param _name="messengerWindowsManager">Менеджер окон в приложении</param>
        public SignUpWindowViewModel(MessengerWindowsManager messengerWindowsManager) : base(messengerWindowsManager)
        {
            OnSignUpCommand = new DelegateCommand(async () => await RegisterNewUserAsync());
            Request = new SignUpRequest(); 
        }

        #endregion Конструкторы

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        /// <returns></returns>
        private async Task RegisterNewUserAsync()
        {
            //// если ошибок нет
            //if (String.IsNullOrEmpty(SignUpRequest.Error))
            //{

            IsControlsAvailable = false;

            TaskCompletionSource completionSource = new TaskCompletionSource();

            var observer = new SignUpObserver(_networkMessageHandler, completionSource);

            _networkMessageHandler.SendRequestAsync<SignUpRequest, SignUpRequestDto>(Request, NetworkMessageCode.SignUpRequestCode);

            await completionSource.Task;

            ProcessRegistrationResponse(observer.RegistrationResponse);

            IsControlsAvailable = true;

            //}

            //else
            //{
            //    MessageBox.Show(SignUpRequest.Error);
            //}
        }

        /// <summary>
        /// Обработать ответ на запрос о регистрации
        /// </summary>
        private void ProcessRegistrationResponse(SignUpResponse response)
        {
            if(response.Status == NetworkResponseStatus.Successful)
            {
                User user = _mapper.Map<User>(Request);
                user.Id = response.UserId;

                SwitchToChatWindow(user);
            }
            else
            {
                Request.PhoneNumber = "";

                MessageBox.Show("Пользователь с таким телефоном уже существует. Введите другой номер или войдите в мессенджер.");
            }
        }

        /// <summary>
        /// Изменить окно на окно чата
        /// </summary>
        private void SwitchToChatWindow(User user)
        {
            _messengerWindowsManager.SwitchToChatWindow(_networkMessageHandler, user);
        }
    }
}