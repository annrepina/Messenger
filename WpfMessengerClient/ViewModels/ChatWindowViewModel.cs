﻿//#define Debug
using DtoLib.Dto.Requests;
using DtoLib.NetworkServices;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.Obsevers;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель окна чатов
    /// </summary>
    public class ChatWindowViewModel : BaseViewModel
    {
        #region Приватные поля

        ///// <summary>
        ///// Посредник между сетевым провайдером и данными пользователя
        ///// </summary>
        //protected readonly NetworkMessageHandler _networkMessageHandler;

        ///// <summary>
        ///// Менеджер окон приложения
        ///// </summary>
        //protected readonly MessengerWindowsManager _messengerWindowsManager;

        /// <inheritdoc cref="ActiveDialog"/>
        private Dialog? _activeDialog;

        /// <inheritdoc cref="SelectedUser"/>
        private User? _selectedUser;

        /// <inheritdoc cref="SearchingRequest"/>
        private UserSearchRequest _searchingRequest;

        ///// <inheritdoc cref="IsSearchingButtonAvailable"/>
        //private bool _isSearchingButtonAvailable;

        /// <inheritdoc cref="GreetingMessage"/>
        private Message _greetingMessage;

        /// <inheritdoc cref="CurrentUser"/>
        private User _currentUser;

        /// <inheritdoc cref="IsGreetingMessageTextBoxAvailable"/>
        private bool _isGreetingMessageTextBoxAvailable;

        /// <inheritdoc cref="OpenDialogButtonText"/>
        private string _openDialogButtonText;

        /// <inheritdoc cref="WasUserSelected"/>
        private bool _wasUserSelected;

        /// <inheritdoc cref="HasNotSearchResult"/>
        private bool _hasNotSearchResult;

        /// <inheritdoc cref="IsGreetingMessageTextBoxVisible"/>
        private bool _isGreetingMessageTextBoxVisible;

        /// <inheritdoc cref="IsOpenDialogButtonAvailable"/>
        private bool _isOpenDialogButtonAvailable;

        /// <inheritdoc cref="SelectedMessage"/>
        private Message _selectedMessage;

        /// <inheritdoc cref="Message"/>
        private Message _message;

        ///// <inheritdoc cref="IsMainMessageBoxAvailable"/>
        //private bool _isMainMessageBoxAvailable;

        ///// <inheritdoc cref="IsSendButtonAvailable"/>
        //private bool _isSendButtonAvailable;

        /// <inheritdoc cref="IsActiveDialogNull"/>
        private bool _isActiveDialogNull;

        /// <inheritdoc cref="WasMessageSelected"/>
        private bool _wasMessageSelected;

        ///// <inheritdoc cref="WasDeleteButtonClicked"/>
        //private bool _wasDeleteButtonClicked;

        ///// <inheritdoc cref="IsExitButtonAvailable"/>
        //private bool _isExitButtonAvailable;

        ///// <inheritdoc cref="IsDialogsEnabled"/>
        //private bool _isDialogsEnabled;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User CurrentUser
        {
            get => _currentUser;

            set
            {
                _currentUser = value;

                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        /// <summary>
        /// Свойство - активный диалог
        /// </summary>
        public Dialog? ActiveDialog
        {
            get => _activeDialog;

            set
            {
                _activeDialog = value;

                if (_activeDialog != null)
                {
                    IsActiveDialogNull = false;

                    ReadMessages(ActiveDialog);
                }

                else
                    IsActiveDialogNull = true;

                OnPropertyChanged(nameof(ActiveDialog));
            }
        }

        /// <summary>
        /// Обозреваемая коллекция 
        /// </summary>
        public ObservableCollection<Dialog> Dialogs { get; set; }

        //public bool IsDialogsEnabled
        //{
        //    get => _isDialogsEnabled;

        //    set
        //    {
        //        _isDialogsEnabled = value;

        //        OnPropertyChanged(nameof(IsDialogsEnabled));
        //    }
        //}

        /// <summary>
        /// Выбранный пользователь 
        /// </summary>
        public User? SelectedUser
        {
            get => _selectedUser;

            set
            {
                _selectedUser = value;

                CheckSelectedUser();

                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        /// <summary>
        /// Выбранное сообщение
        /// </summary>
        public Message SelectedMessage
        {
            get => _selectedMessage;

            set
            {
                _selectedMessage = value;

                if (_selectedMessage != null)
                    WasMessageSelected = true;

                else
                    WasMessageSelected = false;

                OnPropertyChanged(nameof(SelectedMessage));
            }
        }

        /// <summary>
        /// Было выбрано сообщение
        /// </summary>
        public bool WasMessageSelected
        {
            get => _wasMessageSelected;

            set
            {
                _wasMessageSelected = value;

                OnPropertyChanged(nameof(WasMessageSelected));
            }
        }

        /// <summary>
        /// Обозреваемая коллекция пользователей, которых ищет текущий пользователь
        /// </summary>
        public ObservableCollection<User> SearchUserResults { get; set; }

        /// <summary>
        /// Данные о поисковом запросе
        /// </summary>
        public UserSearchRequest SearchingRequest
        {
            get => _searchingRequest;

            set
            {
                _searchingRequest = value;

                OnPropertyChanged(nameof(SearchingRequest));
            }
        }

        /// <summary>
        /// Команда по нажатию кнопки поиска
        /// </summary>
        public DelegateCommand SearchCommand { get; init; }

        /// <summary>
        /// Команда по нажатию на кнопку 
        /// </summary>
        public DelegateCommand OpenDialogCommand { get; init; }

        /// <summary>
        /// Команда по нажатию Delete на выбранном сообщении
        /// </summary>
        public DelegateCommand DeleteMessageCommand { get; init; }

        /// <summary>
        /// Команда по нажатию на кнопку "отпрвить сообщение"
        /// </summary>
        public DelegateCommand SendMessageCommand { get; init; }

        /// <summary>
        /// Команда по выходу из приложения
        /// </summary>
        public DelegateCommand ExitCommand { get; init; }

        ///// <summary>
        ///// Кнопка выхода доступна?
        ///// </summary>
        //public bool IsExitButtonAvailable
        //{
        //    get => _isExitButtonAvailable;

        //    set
        //    {
        //        _isExitButtonAvailable = value;

        //        OnPropertyChanged(nameof(IsExitButtonAvailable));
        //    }
        //}

        ///// <summary>
        ///// Доступна кнопка поиска?
        ///// </summary>
        //public bool IsSearchingButtonAvailable
        //{
        //    get => _isSearchingButtonAvailable;

        //    set
        //    {
        //        _isSearchingButtonAvailable = value;

        //        OnPropertyChanged(nameof(IsSearchingButtonAvailable));
        //    }
        //}

        /// <summary>
        /// Был выбран пользователь среди найденных результатов поиска?
        /// </summary>
        public bool WasUserSelected
        {
            get => _wasUserSelected;

            set
            {
                _wasUserSelected = value;

                OnPropertyChanged(nameof(WasUserSelected));
            }
        }

        /// <summary>
        /// Кнопка открытия нового диалога доступна?
        /// </summary>
        public bool IsOpenDialogButtonAvailable
        {
            get => _isOpenDialogButtonAvailable;

            set
            {
                _isOpenDialogButtonAvailable = value;

                OnPropertyChanged(nameof(IsOpenDialogButtonAvailable));
            }
        }

        /// <summary>
        /// Нет успешного результата поиска?
        /// </summary>
        public bool HasNotSearchResult
        {
            get => _hasNotSearchResult;

            set
            {
                _hasNotSearchResult = value;

                OnPropertyChanged(nameof(HasNotSearchResult));
            }
        }


        /// <summary>
        /// Свойство - приветственное сообщение 
        /// </summary>
        public Message GreetingMessage
        {
            get => _greetingMessage;

            set
            {
                _greetingMessage = value;

                OnPropertyChanged(nameof(GreetingMessage));
            }
        }

        /// <summary>
        /// Текущее сообщение
        /// </summary>
        public Message Message
        {
            get => _message;

            set
            {
                _message = value;

                OnPropertyChanged(nameof(Message));
            }
        }

        /// <summary>
        /// Текст кнопки, которая отвечает за открытие диалога с новым или уже существующим собеседником
        /// </summary>
        public string OpenDialogButtonText
        {
            get => _openDialogButtonText;

            set
            {
                _openDialogButtonText = value;

                OnPropertyChanged(nameof(OpenDialogButtonText));
            }
        }

        /// <summary>
        /// Текстбокс с приветственным сообщением доступен?
        /// </summary>
        public bool IsGreetingMessageTextBoxAvailable
        {
            get => _isGreetingMessageTextBoxAvailable;

            set
            {
                _isGreetingMessageTextBoxAvailable = value;

                OnPropertyChanged(nameof(IsGreetingMessageTextBoxAvailable));
            }
        }

        /// <summary>
        /// Текстбокс с приветственным сообщением видимый?
        /// </summary>
        public bool IsGreetingMessageTextBoxVisible
        {
            get => _isGreetingMessageTextBoxVisible;

            set
            {
                _isGreetingMessageTextBoxVisible = value;

                OnPropertyChanged(nameof(IsGreetingMessageTextBoxVisible));
            }
        }

        ///// <summary>
        ///// Главный текст бокс доступен?
        ///// </summary>
        //public bool IsMainMessageBoxAvailable
        //{
        //    get => _isMainMessageBoxAvailable;

        //    set
        //    {
        //        _isMainMessageBoxAvailable = value;

        //        OnPropertyChanged(nameof(IsMainMessageBoxAvailable));
        //    }
        //}

        ///// <summary>
        ///// Кнопка отправки сообщения доступна?
        ///// </summary>
        //public bool IsSendButtonAvailable
        //{
        //    get => _isSendButtonAvailable;

        //    set
        //    {
        //        _isSendButtonAvailable = value;

        //        OnPropertyChanged(nameof(IsSendButtonAvailable));
        //    }
        //}

        /// <summary>
        /// Активный диалог равен?
        /// </summary>
        public bool IsActiveDialogNull
        {
            get => _isActiveDialogNull;

            set
            {
                _isActiveDialogNull = value;

                OnPropertyChanged(nameof(IsActiveDialogNull));
            }
        }

        ///// <summary>
        ///// Была ли нажата кнопка удаления?
        ///// </summary>
        //public bool WasDeleteButtonClicked
        //{
        //    get => _wasDeleteButtonClicked;

        //    set
        //    {
        //        _wasDeleteButtonClicked = value;

        //        OnPropertyChanged(nameof(WasDeleteButtonClicked));
        //    }
        //}

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkMessageHandler">Посредник между сетевым провайдером и данными пользователя</param>
        /// <param _name="messengerWindowsManager">Менеджер окон приложения</param>
        public ChatWindowViewModel(NetworkMessageHandler networkMessageHandler, MessengerWindowsManager messengerWindowsManager, User user) : base(messengerWindowsManager, networkMessageHandler)
        {
            //_networkMessageHandler = networkMessageHandler;
            _networkMessageHandler.CreateDialogRequestReceived.ResponseReceived += OnCreateDialogRequestReceived;
            _networkMessageHandler.DialogReceivedNewMessage.ResponseReceived += OnDialogReceivedNewMessage;
            _networkMessageHandler.DeleteMessageRequestForClientReceived.ResponseReceived += OnDeleteMessageRequestForClientReceived;
            _networkMessageHandler.DeleteDialogRequestForClientReceived.ResponseReceived += OnDeleteDialogRequestForClientReceived;
            _networkMessageHandler.MessagesAreReadRequestForClientReceived.ResponseReceived += OnMessagesAreReadRequestForClientReceived;

            //_messengerWindowsManager = messengerWindowsManager;
            CurrentUser = user;

            Dialogs = new ObservableCollection<Dialog>();
            //Dialogs.CollectionChanged += OnDialogsChanged;
            ActiveDialog = Dialogs.FirstOrDefault();
            //IsDialogsEnabled = true;

            SearchUserResults = new ObservableCollection<User>();
            //SearchUserResults.CollectionChanged += OnSearchUserResultsChanged;
            //SearchUserResults = new List<User>();
            SelectedUser = SearchUserResults.FirstOrDefault();
            SearchCommand = new DelegateCommand(async () => await OnSearchCommandAsync());
            //IsSearchingButtonAvailable = true;
            SearchingRequest = new UserSearchRequest();
            HasNotSearchResult = false;
            WasUserSelected = false;

            OpenDialogCommand = new DelegateCommand(async () => await OnOpenDialogAsync());
            OpenDialogButtonText = "Поприветствовать";
            IsOpenDialogButtonAvailable = false;

            Message = new Message("", CurrentUser, true);
            DeleteMessageCommand = new DelegateCommand(async () => await OnDeleteMessageCommand());
            SendMessageCommand = new DelegateCommand(async () => await OnSendMessageCommand());

            ExitCommand = new DelegateCommand(async () => await OnSignOutCommand());

            GreetingMessage = new Message("", CurrentUser, true);
            IsGreetingMessageTextBoxAvailable = false;
            IsGreetingMessageTextBoxVisible = false;

            //IsMainMessageBoxAvailable = true;
            //IsSendButtonAvailable = true;

            //WasDeleteButtonClicked = false;
            WasMessageSelected = false;

            //IsExitButtonAvailable = true;
        }

        public ChatWindowViewModel(NetworkMessageHandler networkMessageHandler, MessengerWindowsManager messengerWindowsManager, User user, List<Dialog> dialogs) : this(networkMessageHandler, messengerWindowsManager, user)
        {
            foreach (Dialog dialog in dialogs)
            {
                dialog.CurrentUser = CurrentUser;

                foreach (Message message in dialog.Messages)
                {
                    if (message.UserSender?.Id == CurrentUser.Id)
                        message.IsCurrentUserMessage = true;

                    else
                        message.IsCurrentUserMessage = false;
                }
            }

            //Dialogs.CollectionChanged -= OnDialogsChanged;
            Dialogs = new ObservableCollection<Dialog>(dialogs.OrderByDescending(d => d.Messages.Last().DateTime));
            //Dialogs.CollectionChanged += OnDialogsChanged;

            foreach (var dialog in Dialogs)
            {
                dialog.CheckLastMessagesToUser(CurrentUser.Id);
            }
        }

        #endregion Конструкторы

        #region Обработчики событий

        ///// <summary>
        ///// Обработчик события измения обозреваемой коллекции Диалогов
        ///// </summary>
        ///// <param _name="sender">Объект, вызвавший событие</param>
        ///// <param _name="e">Содержит информацию о событии</param>
        //private void OnDialogsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.OldItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.OldItems)
        //        {
        //            item.PropertyChanged -= OnDialogChanged;
        //        }
        //    }

        //    if (e.NewItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.NewItems)
        //        {
        //            item.PropertyChanged += OnDialogChanged;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Обработчик события изменения конкретного диалога
        ///// </summary>
        ///// <param _name="sender">Объект, вызвавший событие</param>
        ///// <param _name="e">Содержит информацию о событии</param>
        //private void OnDialogChanged(object? sender, PropertyChangedEventArgs e)
        //{

        //    //throw new NotImplementedException();
        //}

        ///// <summary>
        ///// Обрабатывает события измения обозреваемой коллекции результатов поиска пользователей
        ///// </summary>
        ///// <param name="sender">Объект, вызвавший событие</param>
        ///// <param name="e">Содержит информацию о событии</param>
        //private void OnSearchUserResultsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.OldItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.OldItems)
        //        {
        //            item.PropertyChanged -= OnUserResultChanged;
        //        }
        //    }

        //    if (e.NewItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.NewItems)
        //        {
        //            item.PropertyChanged += OnUserResultChanged;
        //        }
        //    }
        //}
        ///// <summary>
        ///// Обрабатывает изменение конкретного результата поиска пользователя
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnUserResultChanged(object? sender, PropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void OnMessagesAreReadRequestForClientReceived(MessagesAreReadRequestForClient messagesAreReadRequestForClient)
        {
            var dialog = Dialogs.First(d => d.Id == messagesAreReadRequestForClient.DialogId);

            ReadMessages(messagesAreReadRequestForClient.MessagesId, dialog);

            dialog.CheckLastMessagesToUser(CurrentUser.Id);
        }

        /// <summary>
        /// Обрабатывает событие обработчика сетевых сообщений - получен запрос на создание нового диалога
        /// </summary>
        /// <param name="dialog">Диалог</param>
        private void OnCreateDialogRequestReceived(Dialog dialog)
        {
            if (dialog.Messages.First().UserSender.Id == CurrentUser.Id)
                dialog.Messages.First().IsCurrentUserMessage = true;

            else
                dialog.Messages.First().IsCurrentUserMessage = false;

            dialog.CurrentUser = CurrentUser;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Dialogs.Insert(0, dialog);
                SystemSounds.Hand.Play();
            });

            dialog.CheckLastMessagesToUser(CurrentUser.Id);
        }

        /// <summary>
        /// Обрабатывает событие обработчика сетевых сообщений - диалог получил новое сообщение
        /// </summary>
        /// <param name="sendMessageRequest">Запрос на отправку сообещения</param>
        private void OnDialogReceivedNewMessage(SendMessageRequest sendMessageRequest)
        {
            Message message = sendMessageRequest.Message;
            Dialog dialog = Dialogs.First(dial => dial.Id == sendMessageRequest.DialogId);

            int dialogIndex = Dialogs.IndexOf(dialog);

            if (message.UserSender.Id != CurrentUser.Id)
                message.IsCurrentUserMessage = false;

            else
                message.IsCurrentUserMessage = true;

            Application.Current.Dispatcher.Invoke(() =>
            {
                dialog.Messages.Add(message);

                if (ActiveDialog != null && ActiveDialog.Id == dialog.Id)
                {
                    Dialogs.Move(dialogIndex, 0);
                    ActiveDialog = dialog;
                }

                else
                    Dialogs.Move(dialogIndex, 0);

                SystemSounds.Hand.Play();
            });

            if (HasActiveDialogNewMessage(dialog, message))
            {
                List<int> messagesId = new List<int> { message.Id };

                SendMessagesAreReadRequest(messagesId, dialog);
            }

            dialog.CheckLastMessagesToUser(CurrentUser.Id);
        }

        private async Task SendMessagesAreReadRequest(List<int> messagesId, Dialog dialog)
        {
            MessagesAreReadRequest messageIsReadRequest = new MessagesAreReadRequest(messagesId, CurrentUser.Id, dialog.Id);

            var response = await SendRequestAsync<MessagesAreReadRequest, MessagesAreReadRequestDto, Response>(messageIsReadRequest, _networkMessageHandler.MessageIsReadResponseReceived, NetworkMessageCode.MessagesAreReadRequestCode);

            ProcessMessagesAreReadResponse(response, messagesId, dialog);
        }

        /// <summary>
        /// Обработывает нажатия кнопки поиска
        /// </summary>
        private async Task OnSearchCommandAsync()
        {
            if (String.IsNullOrEmpty(SearchingRequest.Name) && String.IsNullOrEmpty(SearchingRequest.PhoneNumber))
                MessageBox.Show("Заполните имя и/или телефон для поиска собеседника");

            else
            {
                //IsSearchingButtonAvailable = false;
                AreControlsAvailable = false;

                var response = await SendRequestAsync<UserSearchRequest, UserSearchRequestDto, UserSearchResponse>(SearchingRequest, _networkMessageHandler.UserSearchResponseReceived, NetworkMessageCode.SearchUserRequestCode);

                ProcessSearchResponse(response);

                //IsSearchingButtonAvailable = true;
                AreControlsAvailable = true;
            }
        }

        /// <summary>
        /// Обрабатывает команду открытия диалога
        /// </summary>
        /// <returns></returns>
        private async Task OnOpenDialogAsync()
        {
            if (HasNotChatWithSelectedUser())
                await CreateNewDialog();

            else
            {
                ActiveDialog = Dialogs.First(dial => dial.Users.Find(user => user.Id == _selectedUser.Id) != null);

                await ReadMessages(ActiveDialog);
            }
        }

        private async Task ReadMessages(Dialog dialog)
        {
            var unreadMessagesId = ActiveDialog.Messages.Reverse().TakeWhile(mes => mes.IsCurrentUserMessage == false && mes.IsRead == false).Select(mes => mes.Id).ToList();

            if (unreadMessagesId.Count > 0)
                await SendMessagesAreReadRequest(unreadMessagesId, dialog);
        }

        private void ReadMessages(List<int> messagesId, Dialog dialog)
        {
            foreach (int id in messagesId)
            {
                dialog.Messages.First(mes => mes.Id == id).IsRead = true;
            }
        }

        private async Task CreateNewDialog()
        {
            IsGreetingMessageTextBoxAvailable = false;

            if (String.IsNullOrEmpty(GreetingMessage.Text))
            {
                MessageBox.Show("Сначала введите приветственное сообщение =)");
                IsGreetingMessageTextBoxAvailable = true;
            }

            else
            {
                IsOpenDialogButtonAvailable = false;

                Dialog dialog = new Dialog(CurrentUser, SelectedUser);
                dialog.CurrentUser = CurrentUser;
                dialog.Messages.Add(GreetingMessage);

                GreetingMessage = new Message("", CurrentUser, true);

                var response = await SendRequestAsync<Dialog, CreateDialogRequestDto, CreateDialogResponse>(dialog, _networkMessageHandler.CreateDialogResponseReceived, NetworkMessageCode.CreateDialogRequestCode);

                ProcessCreateDialogResponse(response, dialog);

                CheckSelectedUser();
                dialog.HasUnreadMessages = false;
            }
        }

        /// <summary>
        /// Обработать нажатие кнопки отправки сообщения
        /// </summary>
        /// <returns></returns>
        private async Task OnSendMessageCommand()
        {
            if (string.IsNullOrEmpty(Message.Text))
                MessageBox.Show("Сначала введите сообщение =)");

            else
            {
                //IsMainMessageBoxAvailable = false;
                AreControlsAvailable = false;
                //IsSendButtonAvailable = false;

                Message newMessage = Message;
                Message = new Message("", CurrentUser, true);

                SendMessageRequest sendMessageRequest = new SendMessageRequest(newMessage, ActiveDialog.Id);

                var response = await SendRequestAsync<SendMessageRequest, SendMessageRequestDto, SendMessageResponse>(sendMessageRequest, _networkMessageHandler.SendMessageResponseReceived, NetworkMessageCode.SendMessageRequestCode);

                ProcessMessageDeliveredResponse(response, newMessage);

                //IsMainMessageBoxAvailable = true;
                AreControlsAvailable = true;
                //IsSendButtonAvailable = true;
            }
        }

        /// <summary>
        /// Обработать команду удаления сообщения
        /// </summary>
        /// <returns></returns>
        private async Task OnDeleteMessageCommand()
        {
            //WasDeleteButtonClicked = true;
            //IsDialogsEnabled = false;
            AreControlsAvailable = false;

            if (ActiveDialog.Messages.Count > 1)
            {
                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest(SelectedMessage.Id, ActiveDialog.Id, CurrentUser.Id);
                var response = await SendRequestAsync<DeleteMessageRequest, DeleteMessageRequestDto, Response>(deleteMessageRequest, _networkMessageHandler.DeleteMessageResponseReceived, NetworkMessageCode.DeleteMessageRequestCode);
                ProcessMessageDeletedResponse(response);
            }
            else
            {
                DeleteDialogRequest deleteDialogRequest = new DeleteDialogRequest(ActiveDialog.Id, CurrentUser.Id);
                var response = await SendRequestAsync<DeleteDialogRequest, DeleteDialogRequestDto, Response>(deleteDialogRequest, _networkMessageHandler.DeleteDialogResponseReceived, NetworkMessageCode.DeleteDialogRequestCode);
                ProcessDeleteDialogResponse(response);
            }

            AreControlsAvailable = true;
            //IsDialogsEnabled = true;
            //WasDeleteButtonClicked = false;
        }

        private async Task<TResponse> SendRequestAsync<TRequest, TRequestDto, TResponse>(TRequest request, NetworkMessageHandlerEvent<TResponse> eventWrapper, NetworkMessageCode code)
            where TResponse : class
            where TRequestDto : class
        {
            TaskCompletionSource taskCompletionSource = new TaskCompletionSource();

            var observer = new Observer<TResponse>(taskCompletionSource, eventWrapper);

            await _networkMessageHandler.SendRequestAsync<TRequest, TRequestDto>(request, code);
            await taskCompletionSource.Task;

            return observer.Response;
        }

        /// <summary>
        /// Обрабатывает событие получение запроса на удаление сообщения
        /// </summary>
        /// <param name="deleteMessageRequest">Запрос на удаление сообщения</param>
        private void OnDeleteMessageRequestForClientReceived(DeleteMessageRequestForClient deleteMessageRequest)
        {
            int id = CurrentUser.Id;

            Dialog dialog = Dialogs.First(dial => dial.Id == deleteMessageRequest.DialogId);

            Message message = dialog.Messages.First(mes => mes.Id == deleteMessageRequest.MessageId);

            Application.Current.Dispatcher.Invoke(() =>
            {
                bool res = dialog.Messages.Remove(message);

                if (res == false)
                    MessageBox.Show("Не удалось удалить сообщение");
            });

            dialog.CheckLastMessagesToUser(CurrentUser.Id);
        }

        /// <summary>
        /// Обрабатывает событие получения запроса на удаление диалога
        /// </summary>
        /// <param name="obj"></param>
        private void OnDeleteDialogRequestForClientReceived(DeleteDialogRequestForClient deleteDialogRequest)
        {
            Dialog dialog = Dialogs.First(dial => dial.Id == deleteDialogRequest.DialogId);

            Application.Current.Dispatcher.Invoke(() =>
            {
                bool res = Dialogs.Remove(dialog);

                if (res == false)
                    MessageBox.Show("Не удалось удалить диалог");
            });
        }

        /// <summary>
        /// Обработчик нажатия кнопки выхода
        /// </summary>
        /// <returns></returns>
        private async Task OnSignOutCommand()
        {
            AreControlsAvailable = false;

            SignOutRequest signOutRequest = new SignOutRequest(CurrentUser.Id);

            var response = await SendRequestAsync<SignOutRequest, SignOutRequestDto, Response>(signOutRequest, _networkMessageHandler.SignOutResponseReceived, NetworkMessageCode.SignOutRequestCode);

            ProcessSignOutResponse(response);
        }

        #endregion Обработчики событий

        /// <summary>
        /// Добавляет список найденных пользователей в список результатов поиска пользователя
        /// </summary>
        /// <param name="userSearchResult">Представляет результат поиска пользователя</param>
        private void AddSearchResults(UserSearchResponse userSearchResult)
        {
            try
            {
                List<User> relevantUsers = userSearchResult.RelevantUsers;

                relevantUsers.RemoveAll(user => user.Id == CurrentUser.Id);

                SearchUserResults.Clear();

                foreach (User user in relevantUsers)
                {
                    SearchUserResults.Add(user);
                }

                HasNotSearchResult = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Обработать ответ на запрос о прочтении сообщения
        /// </summary>
        /// <param name="response"></param>
        private void ProcessMessagesAreReadResponse(Response response, List<int> messagesId, Dialog dialog)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                ReadMessages(messagesId, dialog);
                dialog.CheckLastMessagesToUser(CurrentUser.Id);
            }

            else
                CloseWindow();
        }

        /// <summary>
        /// Обработать ответ на запрос об удалении сообщения
        /// </summary>
        private void ProcessMessageDeletedResponse(Response response)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                bool res = ActiveDialog.Messages.Remove(SelectedMessage);

                if (res == false)
                    MessageBox.Show("Не удалось удалить сообщение. Попробуйте позже.");
            }

            else if (response.Status == NetworkResponseStatus.Failed)
                MessageBox.Show("Не удалось удалить сообщение. Попробуйте позже.");

            else
                CloseWindow();
        }

        /// <summary>
        /// Обработать ответ на запрос об удалении диалога
        /// </summary>
        /// <param name="response">Ответ</param>
        private void ProcessDeleteDialogResponse(Response response)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                if (AreSelectedUserAndActiveDialogUserEqual())
                {
                    IsGreetingMessageTextBoxAvailable = true;
                    IsGreetingMessageTextBoxVisible = true;
                    OpenDialogButtonText = "Поприветствовать";
                }

                /*bool res = */
                Dialogs.Remove(ActiveDialog);

                ActiveDialog = null;

                //if (res == false)
                //    MessageBox.Show("Не удалось удалить диалог");
            }
            else if (response.Status == NetworkResponseStatus.Failed)
                MessageBox.Show("Не удалось удалить диалог");

            else
                CloseWindow();
        }

        /// <summary>
        /// Обработать полученный результат поиска
        /// </summary>
        /// <param name="userSearchResult">Результат поиска пользователей</param>
        private void ProcessSearchResponse(UserSearchResponse userSearchResult)
        {
            if (userSearchResult.Status == NetworkResponseStatus.Successful)
                AddSearchResults(userSearchResult);

            else if (userSearchResult.Status == NetworkResponseStatus.Failed)
                ResetSearchResults();

            else
                CloseWindow();
        }

        /// <summary>
        /// Обработать ответ на запрос о создании диалога
        /// </summary>
        /// <param name="response">Ответ на запрос о создании диалога</param>
        private void ProcessCreateDialogResponse(CreateDialogResponse response, Dialog dialog)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                dialog.Id = response.DialogId;
                dialog.Messages.First().Id = response.MessageId;

                Dialogs.Insert(0, dialog);
                ActiveDialog = dialog;
            }
            else
                CloseWindow();
        }

        /// <summary>
        /// Обработать ответ, который означает, что отправленное сообщение доставлено
        /// </summary>
        /// <param name="response">Ответ на запрос об отправке сообщения</param>
        /// <param name="message">Сообщение, которое отправили</param>
        private void ProcessMessageDeliveredResponse(SendMessageResponse response, Message message)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                message.Id = response.MessageId;

                ActiveDialog.Messages.Add(message);

                int dialogIndex = Dialogs.IndexOf(ActiveDialog);

                Dialogs.Move(dialogIndex, 0);

                ActiveDialog = Dialogs.First();
            }
            else
                CloseWindow();
        }

        private void ProcessSignOutResponse(Response response)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                AreControlsAvailable = true;
                _messengerWindowsManager.SwitchToSignUpSignInWindow();
            }

            else
                CloseWindow();
        }

        /// <summary>
        /// Сбросить параметры
        /// </summary>
        private void ResetSearchResults()
        {
            SearchUserResults.Clear();

            WasUserSelected = false;
            IsOpenDialogButtonAvailable = false;
            HasNotSearchResult = true;
            IsGreetingMessageTextBoxAvailable = false;
            IsGreetingMessageTextBoxVisible = false;
        }

        /// <summary>
        /// Проверить выбранного пользователя
        /// </summary>
        private void CheckSelectedUser()
        {
            if (_selectedUser != null)
            {
                IsOpenDialogButtonAvailable = true;
                WasUserSelected = true;

                if (Dialogs.FirstOrDefault(d => d.Users.Any(user => user.Id == _selectedUser.Id)) != null)
                {
                    IsGreetingMessageTextBoxVisible = false;
                    OpenDialogButtonText = "Открыть диалог";
                }

                else
                {
                    IsGreetingMessageTextBoxAvailable = true;
                    IsGreetingMessageTextBoxVisible = true;
                    OpenDialogButtonText = "Поприветствовать";
                }
            }
            else
            {
                IsGreetingMessageTextBoxVisible = false;
                WasUserSelected = false;
            }
        }

        internal void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        #region Предикаты

        /// <summary>
        /// У текущего пользователя еще нет чата с выбранным пользователем
        /// </summary>
        /// <returns></returns>
        private bool HasNotChatWithSelectedUser()
        {
            return Dialogs.FirstOrDefault(d => d.Users.Find(user => user.Id == _selectedUser.Id) != null) == null;
        }

        /// <summary>
        /// Активный диалог получил новое сообщение от собеседника
        /// </summary>
        /// <param name="dialog">Диалог, который получил новое сообщение</param>
        /// <param name="message">Новое сообщение</param>
        /// <returns></returns>
        private bool HasActiveDialogNewMessage(Dialog dialog, Message message)
        {
            return ActiveDialog != null && ActiveDialog.Id == dialog.Id && message.UserSender.Id != CurrentUser.Id;
        }

        /// <summary>
        /// Равны ли выбранный пользователь и собеседник активного диалога?
        /// </summary>
        /// <returns></returns>
        private bool AreSelectedUserAndActiveDialogUserEqual()
        {
            return SelectedUser != null && SelectedUser.Id == ActiveDialog.Users.First(user => user.Id != ActiveDialog.CurrentUser.Id).Id;
        }

        #endregion Предикаты

        /// <summary>
        /// Закрывает текущее окно
        /// </summary>
        private void CloseWindow()
        {
            MessageBox.Show("Ой, кажется что-то пошло не так.\nМы уже работаем над решением проблемы, попробуйте запустить приложение позже.");

            _messengerWindowsManager.CloseCurrentWindow();
        }
    }
}