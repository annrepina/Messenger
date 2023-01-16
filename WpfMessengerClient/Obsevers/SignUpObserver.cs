using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    ///// <summary>
    ///// Класс, который наблюдает за событием SignUpResponseReceived у NetworkMessageHandler
    ///// </summary>
    //public class SignUpObserver : Observer<SignUpResponse>
    //{
    //    ///// <summary>
    //    ///// Ответ на запрос о регистрации пользователя
    //    ///// </summary>
    //    //public SignUpResponse RegistrationResponse { get; set; }

    //    ///// <summary>
    //    ///// Конструктор с параметрами
    //    ///// </summary>
    //    ///// <param _name="networkMessageHandler"></param>
    //    ///// <param _name="completionSource"></param>
    //    //public SignUpObserver(NetworkMessageHandler networkMessageHandler, TaskCompletionSource completionSource) : base(networkMessageHandler, completionSource)
    //    //{
    //    //    _networkMessageHandler.SignUpResponseReceived += OnEventOccured;
    //    //}

    //    //protected override void OnEventOccured(SignUpResponse response)
    //    //{
    //    //    Response = response;

    //    //    _networkMessageHandler.SignUpResponseReceived -= OnEventOccured;

    //    //    _completionSource.SetResult();
    //    //}

    //    ///// <summary>
    //    ///// Обработчик события регистрации пользователя в мессенджере
    //    ///// </summary>
    //    ///// <param name="response">Ответ на запрос о регистрации пользователя</param>
    //    //private void OnSignUp(SignUpResponse response)
    //    //{
    //    //    RegistrationResponse = response;

    //    //    _networkMessageHandler.SignUpResponseReceived -= OnSignUp;

    //    //    _completionSource.SetResult();
    //    //}
    //}
}
