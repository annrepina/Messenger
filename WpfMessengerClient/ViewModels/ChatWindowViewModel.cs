//#define Debug
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
using System.Windows;
using WpfMessengerClient.Obsevers;
using WpfMessengerClient.Models.Requests;
using DtoLib.NetworkServices;
using WpfMessengerClient.Models.Responses;
using DtoLib.Dto.Requests;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// Вьюмодель окна чатов
    /// </summary>
    public class ChatWindowViewModel : BaseNotifyPropertyChanged
    {
        #region Приватные поля

        /// <summary>
        /// Посредник между сетевым провайдером и данными пользователя
        /// </summary>
        private readonly NetworkMessageHandler _networkMessageHandler;

        /// <inheritdoc cref="ActiveDialog"/>
        private Dialog? _activeDialog;

        /// <inheritdoc cref="SelectedUser"/>
        private User? _selectedUser;

        /// <summary>
        /// Данные о поисковом запросе
        /// <inheritdoc cref="SearchingRequest"/>
        /// </summary>
        private UserSearchRequest _searchingRequest;

        /// <summary>
        /// Кнопка поиска доступгна для нажатия?
        /// </summary>
        private bool _isSearchingButtonFree;

        /// <summary>
        /// Приветственное сообщение
        /// </summary>
        private Message _greetingMessage;

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private User _currentUser;

        /// <summary>
        /// Текстбокс с приветственным сообщением доступен?
        /// </summary>
        private bool _isGreetingMessageTextBoxAvailable;

        /// <summary>
        /// Текст кнопки, которая становится доступна после удачного поиска
        /// </summary>
        private string _openDialogButtonText;

        /// <summary>
        /// Был выбран пользователь среди найденных результатов поиска?
        /// </summary>
        private bool _wasUserSelected;

        /// <summary>
        /// Нет успешного результата поиска?
        /// </summary>
        private bool _hasNotSearchResult;


        /// <inheritdoc cref="IsGreetingMessageTextBoxVisible"/>
        private bool _isGreetingMessageTextBoxVisible;

        /// <inheritdoc cref="IsOpenDialogButtonAvailable"/>
        private bool _isOpenDialogButtonAvailable;

        /// <inheritdoc cref="SelectedMessage"/>
        private Message _selectedMessage;

        /// <inheritdoc cref="Message"/>
        private Message _message;

        /// <inheritdoc cref="IsMainMessageBoxAvailable"/>
        private bool _isMainMessageBoxAvailable;
        private bool _isSendButtonAvailable;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Менеджер окон приложения
        /// </summary>
        public MessengerWindowsManager MessengerWindowsManager { get; init; }

        /// <summary>
        /// Свойство - текущий пользователь
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

                OnPropertyChanged(nameof(ActiveDialog));
            }
        }

        /// <summary>
        /// Обозреваемая коллекция 
        /// </summary>
        public ObservableCollection<Dialog> Dialogs { get; set; }

        ///// <summary>
        ///// Обозреваемая коллекция 
        ///// </summary>
        //public List<Dialog> Dialogs { get; set; }

        /// <summary>
        /// Выбранный пользователь 
        /// </summary>
        public User? SelectedUser
        {
            get => _selectedUser;

            set
            {
                _selectedUser = value;

                //WasUserSelected = true;
                //IsOpenDialogButtonAvailable = true;

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

                OnPropertyChanged(nameof(SelectedMessage));
            }

        }

        /// <summary>
        /// Обозреваемая коллекция пользователей, которых ищет текущий пользователь
        /// </summary>
        public ObservableCollection<User> SearchUserResults { get; set; }

        /// <summary>
        /// Обозреваемая коллекция пользователей, которых ищет текущий пользователь
        /// </summary>
        //public List<User> SearchUserResults { get; set; }

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
        /// Есть текст поискового запроса?
        /// </summary>
        public bool IsSearchingButtonFree
        {
            get => _isSearchingButtonFree;

            set
            {
                _isSearchingButtonFree = value;

                OnPropertyChanged(nameof(IsSearchingButtonFree));
            }
        }

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

        /// <summary>
        /// Главный текст бокс доступен?
        /// </summary>
        public bool IsMainMessageBoxAvailable
        {
            get => _isMainMessageBoxAvailable;

            set
            {
                _isMainMessageBoxAvailable = value;

                OnPropertyChanged(nameof(IsMainMessageBoxAvailable));
            }
        }

        /// <summary>
        /// Кнопка отправки сообщения доступна?
        /// </summary>
        public bool IsSendButtonAvailable 
        { 
            get => _isSendButtonAvailable; 

            set
            {
                _isSendButtonAvailable = value;

                OnPropertyChanged(nameof(IsSendButtonAvailable));
            }
        }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator">Посредник между сетевым провайдером и данными пользователя</param>
        /// <param _name="messengerWindowsManager">Менеджер окон приложения</param>
        public ChatWindowViewModel(NetworkMessageHandler networkProviderUserDataMediator, MessengerWindowsManager messengerWindowsManager, User user)
        {
            _networkMessageHandler = networkProviderUserDataMediator;
            _networkMessageHandler.GotCreateDialogRequest += OnGotCreateDialogRequest;
            MessengerWindowsManager = messengerWindowsManager;
            CurrentUser = user;

            Dialogs = new ObservableCollection<Dialog>();
            Dialogs.CollectionChanged += OnDialogsChanged;
            ActiveDialog = Dialogs.FirstOrDefault();

            SearchUserResults = new ObservableCollection<User>();
            //SearchUserResults.CollectionChanged += OnSearchUserResultsChanged;
            //SearchUserResults = new List<User>();
            SelectedUser = SearchUserResults.FirstOrDefault();
            SearchCommand = new DelegateCommand(async () => await OnSearchCommandAsync());
            IsSearchingButtonFree = true;
            SearchingRequest = new UserSearchRequest();
            HasNotSearchResult = false;
            WasUserSelected = false;

            OpenDialogCommand = new DelegateCommand(async () => await OnOpenDialogAsync());
            OpenDialogButtonText = "Поприветствовать";
            IsOpenDialogButtonAvailable = false;

            Message = new Message("", CurrentUser, true);
            DeleteMessageCommand = new DelegateCommand(async () => await OnDeleteMessageCommand());
            SendMessageCommand = new DelegateCommand(async () => await OnSendMessageCommand());

            GreetingMessage = new Message("", CurrentUser, true);
            IsGreetingMessageTextBoxAvailable = false;
            IsGreetingMessageTextBoxVisible = false;

            IsMainMessageBoxAvailable = true;
            IsSendButtonAvailable = true;
        }



        #endregion Конструкторы

        #region Обработчики событий

        /// <summary>
        /// Обработчик события измения обозреваемой коллекции Диалогов
        /// </summary>
        /// <param _name="sender">Объект, вызвавший событие</param>
        /// <param _name="e">Содержит информацию о событии</param>
        private void OnDialogsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= OnDialogChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += OnDialogChanged;
                }
            }
        }

        /// <summary>
        /// Обработчик события изменения конкретного диалога
        /// </summary>
        /// <param _name="sender">Объект, вызвавший событие</param>
        /// <param _name="e">Содержит информацию о событии</param>
        private void OnDialogChanged(object? sender, PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

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

        /// <summary>
        /// Обрабатывает событие обработчика сетевых сообщений - получен запрос на создание нового диалога
        /// </summary>
        /// <param name="dialog">Диалог</param>
        private void OnGotCreateDialogRequest(Dialog dialog)
        {
            if(dialog.Messages.First().UserSender.Id == CurrentUser.Id)
                dialog.Messages.First().IsCurrentUserMessage = false;

            else
                dialog.Messages.First().IsCurrentUserMessage = true;

            dialog.CurrentUser = CurrentUser;
            Application.Current.Dispatcher.Invoke(() => Dialogs.Insert(0, dialog));

            //Dialogs.Insert(0, dialog);
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
                IsSearchingButtonFree = false;

                TaskCompletionSource completionSource = new TaskCompletionSource();

                var observer = new SearchResultReceivedObserver(_networkMessageHandler, completionSource);

                await _networkMessageHandler.SenRequestAsync<UserSearchRequest, UserSearchRequestDto>(SearchingRequest, NetworkMessageCode.SearchUserRequestCode);

                await completionSource.Task;

                IsSearchingButtonFree = true;

                ProcessSearchResult(observer.UserSearchResult);
            }
        }

        /// <summary>
        /// Обрабатывает команду открытия диалога
        /// </summary>
        /// <returns></returns>
        private async Task OnOpenDialogAsync()
        {
            // если еще не общаемся с этим пользователем
            if (Dialogs.FirstOrDefault(d => d.Users.Contains(SelectedUser)) == null)
            {
                IsGreetingMessageTextBoxAvailable = false;

                if (String.IsNullOrEmpty(GreetingMessage.Text))
                {
                    MessageBox.Show("Сначала введите приветственное сообщение =)");
                    IsGreetingMessageTextBoxAvailable = true;
                }

                else
                {
                    IsGreetingMessageTextBoxAvailable = false;
                    IsOpenDialogButtonAvailable = false;

                    Dialog dialog = new Dialog(CurrentUser, SelectedUser);
                    dialog.CurrentUser = CurrentUser;

                    Message message = (Message)GreetingMessage.Clone();
                    dialog.Messages.Add(message);

                    TaskCompletionSource completionSource = new TaskCompletionSource();
                    var observer = new DialogCreatedObserver(_networkMessageHandler, completionSource);

                    await _networkMessageHandler.SenRequestAsync<Dialog, CreateDialogRequestDto>(dialog, NetworkMessageCode.CreateDialogCode);
                    await completionSource.Task;

                    ProcessSuccessfulDialogCreatedResponse(observer.CreateDialogResponse, dialog);

                    CheckSelectedUser();

                }//else
            }
            else
            {
                //
            }
        }

        private async Task OnDeleteMessageCommand()
        {
            throw new NotImplementedException();
        }

        #endregion Обработчики событий

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
                IsMainMessageBoxAvailable = false;
                IsSendButtonAvailable = false;

                Message newMessage = (Message)Message.Clone();

                SendMessageRequest sendMessageRequest = new SendMessageRequest(newMessage, ActiveDialog.Id);

                TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
                var observer = new MessageDeliveredObserver(_networkMessageHandler, taskCompletionSource);

                await _networkMessageHandler.SenRequestAsync<SendMessageRequest, SendMessageRequestDto>(sendMessageRequest, NetworkMessageCode.SendMessageCode);
                await taskCompletionSource.Task;

                ProcessMessageDeliveredResponse(observer.MessageId);

                IsMainMessageBoxAvailable = true;
                IsSendButtonAvailable = true;
            }
        }

        private void ProcessMessageDeliveredResponse(int messageId)
        {
            throw new NotImplementedException();
        }

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
        /// Обработать полученный результат поиска
        /// </summary>
        /// <param name="userSearchResult">Результат поиска пользователей</param>
        private void ProcessSearchResult(UserSearchResponse? userSearchResult)
        {
            if (userSearchResult != null)
                AddSearchResults(userSearchResult);

            else
                ResetSearchResults();
        }

        /// <summary>
        /// Обработать ответ на запрос о создании диалога
        /// </summary>
        /// <param name="createDialogResponse">Ответ на запрос о создании диалога</param>
        private void ProcessSuccessfulDialogCreatedResponse(CreateDialogResponse createDialogResponse, Dialog dialog)
        {
            dialog.Id = createDialogResponse.DialogId;
            dialog.Messages.First().Id = createDialogResponse.MessageId;

            Dialogs.Insert(0, dialog);
            ActiveDialog = dialog;
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

                // если уже общаемся с этим пользователем
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
    }
}