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
//using DtoLib;

namespace WpfMessengerClient.ViewModels
{
    public class RegistrationWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private UserAccount _currentUserAccount;
        //private string _firstPassword;
        //private string _secondPassword;
        //private bool _hasCurrentUserAccountName;
        //private bool _arePasswordsCorrect;
        //private bool _canUserRegisterAccount;

        private readonly IMapper mapper;



        public UserAccount CurrentUserAccount
        {
            get => _currentUserAccount;

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));  
            }
        }

        public DelegateCommand OnRegisterInMessengerCommand { get; set; }

        public RegistrationWindowViewModel()
        {
            CurrentUserAccount = new UserAccount();
            OnRegisterInMessengerCommand = new DelegateCommand(OnRegisterInMessenger);

            var config = new MapperConfiguration(cnf =>
            {
                cnf.CreateMap<UserAccount, UserAccountDto>();
            });
        }

        /// <summary>
        /// Метод, вызывающий событие PropertyChanged
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void OnRegisterInMessenger()
        {
            // если ошибок нет
            //if(!String.IsNullOrEmpty(CurrentUserAccount.Error))
            //{
                MessengerMapper map = MessengerMapper.GetInstance();

                var mapper = map.CreateIMapper();

                UserAccountDto user = mapper.Map<UserAccountDto>(CurrentUserAccount);

                var a = user;

                //UserAccountDto user = 

                // подключаемся
            //}
        }
    }
}
