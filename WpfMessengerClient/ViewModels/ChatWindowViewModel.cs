using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Services;
using WpfMessengerClient.Models;

namespace WpfMessengerClient.ViewModels
{
    public class ChatWindowViewModel
    {
        private NetworkProviderUserDataMediator _networkProviderUserDataMediator;

        /// <summary>
        /// Событие изменения свойств
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        public NetworkProviderUserDataMediator NetworkProviderUserDataMediator
        {
            get => _networkProviderUserDataMediator;

            set
            {
                _networkProviderUserDataMediator = value;

                OnPropertyChanged(nameof(NetworkProviderUserDataMediator));
            }

        }

        ///// <summary>
        ///// Клиент приложения
        ///// </summary>
        //public FrontClient _client;

        /// <summary>
        /// Получатель сообщений от сервера
        /// </summary>
        //public Receiver _receiver;

        /// <summary>
        /// Отправитель сообщений на сервер
        /// </summary>
        //public Sender _sender;

        ///// <summary>
        ///// Текущий пользователь
        ///// </summary>
        //private UserAccount _currentUser;

        ///// <summary>
        ///// Текущий пользователь
        ///// </summary>
        //public UserAccount CurrentUser
        //{
        //    get => _currentUser;

        //    set
        //    {
        //        _currentUser = value;

        //        OnPropertyChanged(nameof(CurrentUser));
        //    }
        //}

        ///// <summary>
        ///// Пользователь не вошел в чат?
        ///// </summary>
        //private bool _didUserNotEnteredChat;

        ///// <summary>
        ///// Пользователь не вошел в чат?
        ///// </summary>
        //public bool DidUserNotEnteredChat
        //{
        //    get => _didUserNotEnteredChat;

        //    set
        //    {
        //        _didUserNotEnteredChat = value;

        //        OnPropertyChanged(nameof(DidUserNotEnteredChat));
        //    }
        //}

        ///// <summary>
        ///// Обозреваемая коллекция пользователей
        ///// </summary>
        //public ObservableCollection<UserAccount> Users { get; set; }

        ///// <summary>
        ///// Поле - была попытка ввода пустого имени?
        ///// </summary>
        //private bool _wasAttemptToEnterEmptyName;

        ///// <summary>
        ///// Свойство - была попытка ввода пустого имени?
        ///// </summary>
        //public bool WasAttemptToEnterEmptyName
        //{
        //    get => _wasAttemptToEnterEmptyName;

        //    set
        //    {
        //        _wasAttemptToEnterEmptyName = value;

        //        OnPropertyChanged(nameof(WasAttemptToEnterEmptyName));
        //    }
        //}

        ///// <summary>
        ///// Комманда по входу в чат
        ///// </summary>
        //public DelegateCommand OnEnterChatCommand { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public ChatWindowViewModel()
        {
            //Users = new ObservableCollection<UserAccount>();
            //Users.CollectionChanged += OnUsersListPropertyChanged;
            //CurrentUser = new UserAccount();
            //CurrentUser.PropertyChanged += OnCurrentUserPropertyChanged;

            //OnEnterChatCommand = new DelegateCommand(OnEnterChat);

            //_client = new FrontClient();


            //_receiver = new Receiver(_client);
            //_sender = new Sender(_client);

            //WasAttemptToEnterEmptyName = false;
            //DidUserNotEnteredChat = true;
            //HasNotTextBoxSomeName = true;
        }

        public ChatWindowViewModel(NetworkProviderUserDataMediator networkProviderUserDataMediator)
        {
            NetworkProviderUserDataMediator = networkProviderUserDataMediator;
        }

        /// <summary>
        /// Метод, вызывающий событие PropertyChanged
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));  
        }

        ///// <summary>
        ///// Когда изменяется список пользователей, добавление, изменение, перемещение элементов в списке
        ///// </summary>
        ///// <param name="sender">Объект вызвавший событие</param>
        ///// <param name="e">Содержит данные о событии</param>
        //private void OnUsersListPropertyChanged(object? sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if(e.OldItems != null)
        //    {
        //        foreach(INotifyPropertyChanged item in e.OldItems)
        //        {
        //            item.PropertyChanged -= OnUsersPropertyChanged;
        //        }
        //    }

        //    if(e.NewItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.NewItems)
        //        {
        //            item.PropertyChanged += OnUsersPropertyChanged;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Обработчик изменения определенного пользователя
        ///// </summary>
        ///// <param name="sender">Объект вызвавший событие</param>
        ///// <param name="e">Содержит данные о событии</param>
        //private void OnUsersPropertyChanged(object? sender, PropertyChangedEventArgs e)
        //{

        //    // надо подумать, что написать
        //}

        ///// <summary>
        ///// Обработчик изменения текущего пользователя
        ///// </summary>
        ///// <param name="sender">Объект вызвавший событие</param>
        ///// <param name="e">Содержит данные о событии</param>
        //private void OnCurrentUserPropertyChanged(object? sender, PropertyChangedEventArgs e)
        //{
        //    // если изменилось имя и оно не пустое
        //    if (e.PropertyName == nameof(CurrentUser.Person.Name))
        //        WasAttemptToEnterEmptyName = false;
        //}

        ///// <summary>
        ///// Обработчик команды OnEnterChatCommand
        ///// </summary>
        //private void OnEnterChat()
        //{
        //    if(!string.IsNullOrEmpty(CurrentUser.Person.Name))
        //    {


        //        //_client.Connect(_currentUser.Person.Name);

        //        Users.Add(_currentUser);

        //        WasAttemptToEnterEmptyName = false;
        //        DidUserNotEnteredChat = false;
        //        //HasNotTextBoxSomeName = false;
        //    }
        //    else
        //    {
        //        WasAttemptToEnterEmptyName = true;
        //        //HasNotTextBoxSomeName = true;
        //        DidUserNotEnteredChat = true;
        //    }
        //}
    }
}
