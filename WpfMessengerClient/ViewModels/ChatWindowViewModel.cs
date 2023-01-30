//#define Debug
using CommonLib.Dto.Requests;
using CommonLib.NetworkServices;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;
using WpfMessengerClient.Models.Requests;
using WpfMessengerClient.Models.Responses;
using WpfMessengerClient.NetworkMessageProcessing;
using WpfMessengerClient.NetworkServices.Interfaces;
using WpfMessengerClient.Obsevers;
using WpfMessengerClient.Services;

namespace WpfMessengerClient.ViewModels
{
    /// <summary>
    /// ViewModel для окна чатов
    /// </summary>
    public class ChatWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// Максимальная длина сообщения
        /// </summary>
        private const int MessageMaxLength = 500;

        #region Приватные поля

        #region Поля связанные с активным диалогом

        /// <inheritdoc cref="ActiveDialog"/>
        private Dialog? _activeDialog;

        /// <inheritdoc cref="SelectedMessage"/>
        private Message _selectedMessage;

        /// <inheritdoc cref="Message"/>
        private Message _message;

        /// <inheritdoc cref="IsActiveDialogNull"/>
        private bool _isActiveDialogNull;

        /// <inheritdoc cref="WasMessageSelected"/>
        private bool _wasMessageSelected;

        #endregion Поля связанные с активным диалогом

        #region Поля связанные с поиском пользователя

        /// <inheritdoc cref="SelectedSearchedUser"/>
        private User? _selectedSearchedUser;

        /// <inheritdoc cref="GreetingMessage"/>
        private Message _greetingMessage;

        /// <inheritdoc cref="IsGreetTextBoxEnabled"/>
        private bool _isGreetTextBoxEnabled;

        /// <inheritdoc cref="GreetButtonText"/>
        private string _greetButtonText;

        /// <inheritdoc cref="WasSearchedUserSelected"/>
        private bool _wasSearchedUserSelected;

        /// <inheritdoc cref="HasNotSearchResult"/>
        private bool _hasNotSearchResult;

        /// <inheritdoc cref="IsGreetTextBoxVisible"/>
        private bool _isGreetTextBoxVisible;

        /// <inheritdoc cref="IsGreetButtonEnabled"/>
        private bool _isGreetButtonEnabled;

        #endregion Поля связанные с поиском пользователя

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User CurrentUser { get; init; }

        /// <summary>
        /// Команда по выходу из приложения
        /// </summary>
        public DelegateCommand SignOutCommand { get; init; }

        #region Свойства связанные с активным диалогом

        /// <summary>
        /// Свойство - активный диалог
        /// </summary>
        public Dialog? ActiveDialog
        {
            get => _activeDialog;

            set
            {
                _activeDialog = value;

                CheckActiveDialog();

                OnPropertyChanged(nameof(ActiveDialog));
            }
        }

        /// <summary>
        /// Обозреваемая коллекция диалогов пользователя
        /// </summary>
        public ObservableCollection<Dialog> Dialogs { get; private set; }

        /// <summary>
        /// Команда удаления выбранного сообщении
        /// </summary>
        public DelegateCommand DeleteMessageCommand { get; init; }

        /// <summary>
        /// Команда отправки сообщения в активном диалоге
        /// </summary>
        public DelegateCommand SendMessageCommand { get; init; }

        /// <summary>
        /// Текущее сообщение в активном диалоге
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
        /// Активный диалог равен null?
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

        /// <summary>
        /// Выбранное сообщение в чате
        /// </summary>
        public Message SelectedMessage
        {
            get => _selectedMessage;

            set
            {
                _selectedMessage = value;

                CheckSelectedMessage();

                OnPropertyChanged(nameof(SelectedMessage));
            }
        }

        /// <summary>
        /// Было выбрано сообщение в чате?
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

        #endregion Свойства связанные с активным диалогом

        #region Свойства связанные с поиском пользователя

        /// <summary>
        /// Выбранный пользователь среди результатов поиска
        /// </summary>
        public User? SelectedSearchedUser
        {
            get => _selectedSearchedUser;

            set
            {
                _selectedSearchedUser = value;

                CheckSearchControls();

                OnPropertyChanged(nameof(SelectedSearchedUser));
            }
        }

        /// <summary>
        /// Обозреваемая коллекция пользователей, которые являются результатом поиска пользователя
        /// </summary>
        public ObservableCollection<User> SearchUserResults { get; init; }

        /// <summary>
        /// Поисковой запрос
        /// </summary>
        public SearchRequest SearchRequest { get; init; }

        /// <summary>
        /// Команда по нажатию кнопки поиска
        /// </summary>
        public DelegateCommand SearchCommand { get; init; }

        /// <summary>
        /// Команда по нажатию на кнопку отправки приветственного сообщения
        /// </summary>
        public DelegateCommand SendGreetingMesCommand { get; init; }

        /// <summary>
        /// Был выбран пользователь среди найденных результатов поиска?
        /// </summary>
        public bool WasSearchedUserSelected
        {
            get => _wasSearchedUserSelected;

            set
            {
                _wasSearchedUserSelected = value;

                OnPropertyChanged(nameof(WasSearchedUserSelected));
            }
        }

        /// <summary>
        /// Кнопка отправки приветственного сообщения включена?
        /// </summary>
        public bool IsGreetButtonEnabled
        {
            get => _isGreetButtonEnabled;

            set
            {
                _isGreetButtonEnabled = value;

                OnPropertyChanged(nameof(IsGreetButtonEnabled));
            }
        }

        /// <summary>
        /// Нет результатов поиска?
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
        /// Приветственное сообщение 
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
        /// Контент кнопки, которая отправляет приветственное сообщение, если текущий пользователь не знаком с искомым, или открывает диалог, если знаком
        /// </summary>
        public string GreetButtonText
        {
            get => _greetButtonText;

            set
            {
                _greetButtonText = value;

                OnPropertyChanged(nameof(GreetButtonText));
            }
        }

        /// <summary>
        /// Текстбокс с приветственным сообщением доступен?
        /// </summary>
        public bool IsGreetTextBoxEnabled
        {
            get => _isGreetTextBoxEnabled;

            set
            {
                _isGreetTextBoxEnabled = value;

                OnPropertyChanged(nameof(IsGreetTextBoxEnabled));
            }
        }

        /// <summary>
        /// Текстбокс с приветственным сообщением видимый?
        /// </summary>
        public bool IsGreetTextBoxVisible
        {
            get => _isGreetTextBoxVisible;

            set
            {
                _isGreetTextBoxVisible = value;

                OnPropertyChanged(nameof(IsGreetTextBoxVisible));
            }
        }

        #endregion Свойства связанные с поиском пользователя

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами, используется при попадании на окно чатов при регистрации пользователя
        /// </summary>
        /// <param name="windowsManager">Менеджер окон приложения</param>
        /// <param name="networkMessageHandler">Обработчик сетевых сообщений</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        /// <param name="user">Текущий пользователь мессенджера</param>
        public ChatWindowViewModel(WindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider, User user) 
            : base(windowsManager, networkMessageHandler, networkProvider)
        {
            _networkMessageHandler.CreateDialogRequestReceived.EventOccurred += OnCreateDialogRequestReceived;
            _networkMessageHandler.DialogReceivedNewMessage.EventOccurred += OnDialogReceivedNewMessage;
            _networkMessageHandler.DeleteMessageRequestReceived.EventOccurred += OnDeleteMessageRequestReceived;
            _networkMessageHandler.DeleteDialogRequestReceived.EventOccurred += OnDeleteDialogRequestReceived;
            _networkMessageHandler.ReadMessagesRequestReceived.EventOccurred += OnReadMessagesRequestReceived;

            CurrentUser = user;
            SignOutCommand = new DelegateCommand(async () => await OnSignOutCommand());

            Dialogs = new ObservableCollection<Dialog>();
            ActiveDialog = Dialogs.FirstOrDefault();
            Message = new Message("", CurrentUser, true);
            DeleteMessageCommand = new DelegateCommand(async () => await OnDeleteMessageCommand());
            SendMessageCommand = new DelegateCommand(async () => await OnSendMessageCommand());
            WasMessageSelected = false;

            SearchUserResults = new ObservableCollection<User>();
            SelectedSearchedUser = SearchUserResults.FirstOrDefault();
            SearchCommand = new DelegateCommand(async () => await OnSearchCommandAsync());
            SearchRequest = new SearchRequest();
            HasNotSearchResult = false;
            WasSearchedUserSelected = false;

            SendGreetingMesCommand = new DelegateCommand(async () => await OnSendGreetingMessageAsync());
            GreetButtonText = "Поприветствовать";
            IsGreetButtonEnabled = false;
            GreetingMessage = new Message("", CurrentUser, true);
            IsGreetTextBoxEnabled = false;
            IsGreetTextBoxVisible = false;
        }

        /// <summary>
        /// Конструктор с параметрами, используется при попадании на окно чатов при входе пользователя
        /// </summary>
        /// <param name="windowsManager">Менеджер окон приложения</param>
        /// <param name="networkMessageHandler">Обработчик сетевых сообщений</param>
        /// <param name="networkProvider">Сетевой провайдер</param>
        /// <param name="user">Текущий пользователь мессенджера</param>
        /// <param name="dialogs">Диалоги, которые есть у текущего пользователя</param>
        public ChatWindowViewModel(WindowsManager windowsManager, NetworkMessageHandler networkMessageHandler, IClientNetworkProvider networkProvider, User user, List<Dialog> dialogs) 
            : this(windowsManager, networkMessageHandler, networkProvider, user)
        {
            SetDialogs(dialogs);
        }

        #endregion Конструкторы

        #region Методы обрабатывающие команды и события _networkMessageHandler

        /// <summary>
        /// Обрабатывает команду поиска
        /// </summary>
        private async Task OnSearchCommandAsync()
        {
            if (SearchRequest.HasNotErrors() == true)
            {
                AreControlsAvailable = false;

                var response = await SendRequestAsync<SearchRequest, SearchRequestDto, SearchResponse>(SearchRequest, _networkMessageHandler.SearchResponseReceived, NetworkMessageCode.SearcRequestCode);

                ProcessSearchResponse(response);

                AreControlsAvailable = true;
            }

            else
                MessageBox.Show(SearchRequest.GetError());
        }

        /// <summary>
        /// Обрабатывает команду отправки приветственного сообщения
        /// </summary>
        private async Task OnSendGreetingMessageAsync()
        {
            if (!HasDialogWithSelectedUser())
                await CreateNewDialogAsync();

            else
            {
                ActiveDialog = Dialogs.First(dial => dial.Users.Find(user => user.Id == _selectedSearchedUser.Id) != null);

                await ReadMessages(ActiveDialog);
            }
        }

        /// <summary>
        /// Создать новый диалог с выбранным пользователем
        /// </summary>
        private async Task CreateNewDialogAsync()
        {
            IsGreetTextBoxEnabled = false;

            if (String.IsNullOrEmpty(GreetingMessage.Text))
            {
                MessageBox.Show("Сначала введите приветственное сообщение =)");
                IsGreetTextBoxEnabled = true;
            }

            else
            {
                IsGreetButtonEnabled = false;

                Dialog dialog = new Dialog(CurrentUser, SelectedSearchedUser);
                dialog.CurrentUser = CurrentUser;
                dialog.Messages.Add(GreetingMessage);

                GreetingMessage = new Message("", CurrentUser, true);

                var response = await SendRequestAsync<Dialog, CreateDialogRequestDto, CreateDialogResponse>(dialog, _networkMessageHandler.CreateDialogResponseReceived, NetworkMessageCode.CreateDialogRequestCode);

                ProcessCreateDialogResponse(response, dialog);
                CheckSearchControls();

                dialog.HasUnreadMessages = false;
            }
        }

        /// <summary>
        /// Прочитать сообщения
        /// </summary>
        /// <param name="dialog">Диалог, которому принадлежат сообщения</param>
        private async Task ReadMessages(Dialog dialog)
        {
            var unreadMessagesId = ActiveDialog.Messages.Reverse().TakeWhile(mes => mes.IsCurrentUserMessage == false && mes.IsRead == false).Select(mes => mes.Id).ToList();

            if (unreadMessagesId.Count > 0)
                await SendReadMessagesRequest(unreadMessagesId, dialog);
        }

        /// <summary>
        /// Отправить запрос на прочтение сообщений текущим пользователем
        /// </summary>
        /// <param name="messagesId">Список Id прочитанных сообщений</param>
        /// <param name="dialog">Диалог, которому принадлежат сообщения</param>
        private async Task SendReadMessagesRequest(List<int> messagesId, Dialog dialog)
        {
            ExtendedReadMessagesRequest messageIsReadRequest = new ExtendedReadMessagesRequest(messagesId, CurrentUser.Id, dialog.Id);

            var response = await SendRequestAsync<ExtendedReadMessagesRequest, MessagesReadRequestDto, Response>(messageIsReadRequest, _networkMessageHandler.ReadMessageResponseReceived, NetworkMessageCode.MessagesAreReadRequestCode);

            ProcessReadMessagesResponse(response, messagesId, dialog);
        }

        /// <summary>
        /// Обработать команду отправки сообщения
        /// </summary>
        private async Task OnSendMessageCommand()
        {
            if (string.IsNullOrEmpty(Message.Text))
                MessageBox.Show("Сначала введите сообщение =)");

            else if (Message.Text.Length > MessageMaxLength)
                MessageBox.Show("Сообщение не должно превышать 500 символов");

            else
            {
                AreControlsAvailable = false;

                Message newMessage = Message;
                Message = new Message("", CurrentUser, true);

                SendMessageRequest sendMessageRequest = new SendMessageRequest(newMessage, ActiveDialog.Id);

                var response = await SendRequestAsync<SendMessageRequest, SendMessageRequestDto, SendMessageResponse>(sendMessageRequest, _networkMessageHandler.SendMessageResponseReceived, NetworkMessageCode.SendMessageRequestCode);

                ProcessMessageDeliveredResponse(response, newMessage);

                AreControlsAvailable = true;
            }
        }

        /// <summary>
        /// Обработать команду удаления сообщения
        /// </summary>
        private async Task OnDeleteMessageCommand()
        {
            AreControlsAvailable = false;

            if (ActiveDialog.Messages.Count > 1)
            {
                ExtendedDeleteMessageRequest deleteMessageRequest = new ExtendedDeleteMessageRequest(SelectedMessage.Id, ActiveDialog.Id, CurrentUser.Id);
                var response = await SendRequestAsync<ExtendedDeleteMessageRequest, ExtendedDeleteMessageRequestDto, Response>(deleteMessageRequest, _networkMessageHandler.DeleteMessageResponseReceived, NetworkMessageCode.DeleteMessageRequestCode);
                ProcessDeleteMessageResponse(response);
            }
            else
            {
                ExtendedDeleteDialogRequest deleteDialogRequest = new ExtendedDeleteDialogRequest(ActiveDialog.Id, CurrentUser.Id);
                var response = await SendRequestAsync<ExtendedDeleteDialogRequest, ExtendedDeleteDialogRequestDto, Response>(deleteDialogRequest, _networkMessageHandler.DeleteDialogResponseReceived, NetworkMessageCode.DeleteDialogRequestCode);
                ProcessDeleteDialogResponse(response);
            }

            AreControlsAvailable = true;
        }

        /// <summary>
        /// Обработчик команды выхода пользователя из мессенджера
        /// </summary>
        private async Task OnSignOutCommand()
        {
            AreControlsAvailable = false;

            SignOutRequest signOutRequest = new SignOutRequest(CurrentUser.Id);

            var response = await SendRequestAsync<SignOutRequest, SignOutRequestDto, Response>(signOutRequest, _networkMessageHandler.SignOutResponseReceived, NetworkMessageCode.SignOutRequestCode);

            ProcessSignOutResponse(response);
        }

        /// <summary>
        /// Обрабатывает событие получения запроса на создание нового диалога
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

            dialog.CheckLastMessagesRead();
        }

        /// <summary>
        /// Обрабатывает событие - диалог получил новое сообщение
        /// </summary>
        /// <param name="messageRequest">Запрос на получение сообещения</param>
        private void OnDialogReceivedNewMessage(SendMessageRequest messageRequest)
        {
            Message message = messageRequest.Message;
            Dialog dialog = Dialogs.First(dial => dial.Id == messageRequest.DialogId);

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

            dialog.CheckLastMessagesRead();
        }

        /// <summary>
        /// Обработчик получения запроса на прочтение сообщения текущим пользователем на другом клиенте
        /// </summary>
        /// <param name="readMessagesRequest">Запрос на прочтение сообщения</param>
        private void OnReadMessagesRequestReceived(ReadMessagesRequest readMessagesRequest)
        {
            var dialog = Dialogs.First(d => d.Id == readMessagesRequest.DialogId);

            ReadMessages(readMessagesRequest.MessagesId, dialog);

            dialog.CheckLastMessagesRead();
        }

        /// <summary>
        /// Обрабатывает события получения запроса на удаление сообщения
        /// </summary>
        /// <param name="deleteMessageRequest">Запрос на удаление сообщения</param>
        private void OnDeleteMessageRequestReceived(DeleteMessageRequest deleteMessageRequest)
        {
            Dialog dialog = Dialogs.First(dial => dial.Id == deleteMessageRequest.DialogId);

            Message message = dialog.Messages.First(mes => mes.Id == deleteMessageRequest.MessageId);

            Application.Current.Dispatcher.Invoke(() => dialog.Messages.Remove(message));

            dialog.CheckLastMessagesRead();
        }

        /// <summary>
        /// Обрабатывает событие получения запроса на удаление диалога
        /// </summary>
        /// <param name="deleteDialogRequest">Запрос на удаление диалога</param>
        private void OnDeleteDialogRequestReceived(DeleteDialogRequest deleteDialogRequest)
        {
            Dialog dialog = Dialogs.First(dial => dial.Id == deleteDialogRequest.DialogId);

            Application.Current.Dispatcher.Invoke(() => Dialogs.Remove(dialog));
        }

        /// <summary>
        /// Отправить запрос асинхронно
        /// </summary>
        /// <typeparam name="TRequest">Тип объекта, представляющего запрос</typeparam>
        /// <typeparam name="TRequestDto">Тип объекта, представляющего DTO</typeparam>
        /// <typeparam name="TResponse">Тип объекта, представляющего ответ</typeparam>
        /// <param name="request">Запрос</param>
        /// <param name="responseReceivedEvent">Событие получения ответа</param>
        /// <param name="code">Код сетевого сообщения</param>
        /// <returns>Ответ на запрос</returns>
        private async Task<TResponse> SendRequestAsync<TRequest, TRequestDto, TResponse>(TRequest request, NetworkMessageHandlerEvent<TResponse> responseReceivedEvent, NetworkMessageCode code)
            where TResponse : class
            where TRequestDto : class
        {

            TaskCompletionSource taskCompletionSource = new TaskCompletionSource();

            var observer = new Observer<TResponse>(taskCompletionSource, responseReceivedEvent);

            await _networkProvider.SendBytesAsync(RequestConverter<TRequest, TRequestDto>.Convert(request, code));

            await taskCompletionSource.Task;

            return observer.Response;
        }

        #endregion Обработчики команд и событий _networkMessageHandler

        #region Методы обрабатывающие ответы на запросы

        /// <summary>
        /// Обработать полученный результат поиска
        /// </summary>
        /// <param name="userSearchResult">Результат поиска пользователей</param>
        private void ProcessSearchResponse(SearchResponse userSearchResult)
        {
            if (userSearchResult.Status == NetworkResponseStatus.Successful)
                AddSearchResults(userSearchResult);

            else if (userSearchResult.Status == NetworkResponseStatus.Failed)
                ResetSearchControls();

            else
                CloseWindow();
        }

        /// <summary>
        /// Добавляет список найденных пользователей в список результатов поиска пользователя
        /// </summary>
        /// <param name="userSearchResponse">Ответ на запрос о поиске пользователя</param>
        private void AddSearchResults(SearchResponse userSearchResponse)
        {
            List<User> relevantUsers = userSearchResponse.RelevantUsers;

            relevantUsers.RemoveAll(user => user.Id == CurrentUser.Id);

            SearchUserResults.Clear();

            foreach (User user in relevantUsers)
            {
                SearchUserResults.Add(user);
            }

            HasNotSearchResult = false;
        }

        /// <summary>
        /// Сбросить настройки элементов управления, связанных с поиском
        /// </summary>
        private void ResetSearchControls()
        {
            SearchUserResults.Clear();

            WasSearchedUserSelected = false;
            IsGreetButtonEnabled = false;
            HasNotSearchResult = true;
            IsGreetTextBoxEnabled = false;
            IsGreetTextBoxVisible = false;
        }

        /// <summary>
        /// Обработать ответ на запрос о создании диалога
        /// </summary>
        /// <param name="response">Ответ на запрос о создании диалога</param>
        /// <param name="dialog">Диалог, который будет добавлен в список диалогов</param>
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
        /// Обработать ответ на запрос о прочтении сообщения
        /// </summary>
        /// <param name="response">Ответ на запрос о прочтении сообщения</param>
        /// <param name="messagesId">Список Id прочитанных сообщений</param>
        /// <param name="dialog">Диалог, которому принадлежат сообщения</param>
        private void ProcessReadMessagesResponse(Response response, List<int> messagesId, Dialog dialog)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                ReadMessages(messagesId, dialog);
                dialog.CheckLastMessagesRead();
            }

            else
                CloseWindow();
        }

        /// <summary>
        /// Прочитать сообщения
        /// </summary>
        /// <param name="messagesId">Список Id прочитанных сообщений</param>
        /// <param name="dialog">Диалог, которому принадлежат сообщения</param>
        private void ReadMessages(List<int> messagesId, Dialog dialog)
        {
            foreach (int id in messagesId)
            {
                dialog.Messages.First(mes => mes.Id == id).IsRead = true;
            }
        }

        /// <summary>
        /// Обработать ответ на запрос об отправке ссообщения
        /// </summary>
        /// <param name="response">Ответ на запрос об отправке сообщения</param>
        /// <param name="message">Сообщение, которое отправил пользователь</param>
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

        /// <summary>
        /// Обработать ответ на запрос об удалении сообщения
        /// </summary>
        /// <param name="response">Ответ на запрос об удалении сообщения</param>
        private void ProcessDeleteMessageResponse(Response response)
        {
            if (response.Status == NetworkResponseStatus.Successful)
                ActiveDialog.Messages.Remove(SelectedMessage);


            else if (response.Status == NetworkResponseStatus.Failed)
                MessageBox.Show("Не удалось удалить сообщение. Попробуйте позже.");

            else
                CloseWindow();
        }

        /// <summary>
        /// Обработать ответ на запрос об удалении диалога
        /// </summary>
        /// <param name="response">Ответ на запрос об удалении диалога</param>
        private void ProcessDeleteDialogResponse(Response response)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                if (AreSelectedUserAndActiveDialogUserEqual())
                {
                    IsGreetTextBoxEnabled = true;
                    IsGreetTextBoxVisible = true;
                    GreetButtonText = "Поприветствовать";
                }

                Dialogs.Remove(ActiveDialog);

                ActiveDialog = null;
            }

            else if (response.Status == NetworkResponseStatus.Failed)
                MessageBox.Show("Не удалось удалить диалог.");

            else
                CloseWindow();
        }

        /// <summary>
        /// Обработать ответ на запрос о выходе пользователя из мессенджера
        /// </summary>
        /// <param name="response">Ответ на запрос о выходе пользователя из мессенджера</param>
        private void ProcessSignOutResponse(Response response)
        {
            if (response.Status == NetworkResponseStatus.Successful)
            {
                AreControlsAvailable = true;
                _messengerWindowsManager.ReturnToStartWindow();
            }

            else
                CloseWindow();
        }

        #endregion Методы обрабатывающие ответы на запросы

        #region Предикаты
        
        /// <summary>
        /// У текущего пользователя уже есть диалог с выбранным среди найденных пользователей?
        /// </summary>
        /// <returns>true - если есть диалог с выбранным пользователем, false - если нет</returns>
        private bool HasDialogWithSelectedUser()
        {
            return Dialogs.FirstOrDefault(d => d.Users.Any(user => user.Id == _selectedSearchedUser.Id)) != null;
        }

        /// <summary>
        /// Это активный диалог получил новое сообщение от собеседника?
        /// </summary>
        /// <param name="dialog">Диалог, который получил новое сообщение</param>
        /// <param name="message">Новое сообщение</param>
        /// <returns></returns>
        private bool HasActiveDialogNewMessage(Dialog dialog, Message message)
        {
            return ActiveDialog != null && ActiveDialog.Id == dialog.Id && message.UserSender.Id != CurrentUser.Id;
        }

        /// <summary>
        /// Равны ли выбранный среди результатов поиска пользователь и собеседник активного диалога?
        /// </summary>
        /// <returns>true - если равны, false - если не равны</returns>
        private bool AreSelectedUserAndActiveDialogUserEqual()
        {
            return SelectedSearchedUser != null && SelectedSearchedUser.Id == ActiveDialog.Users.First(user => user.Id != ActiveDialog.CurrentUser.Id).Id;
        }

        #endregion Предикаты

        #region Методы проверяющие свойства на null

        /// <summary>
        /// Проверить на null вактивный диалог
        /// </summary>
        private void CheckActiveDialog()
        {
            if (_activeDialog != null)
            {
                IsActiveDialogNull = false;

                ReadMessages(ActiveDialog);
            }

            else
                IsActiveDialogNull = true;
        }

        /// <summary>
        /// Проверяет на null выбранное сообщение
        /// </summary>
        private void CheckSelectedMessage()
        {
            if (_selectedMessage != null)
                WasMessageSelected = true;

            else
                WasMessageSelected = false;
        }


        #endregion Методы проверяющие свойства на null

        /// <summary>
        /// Устанавливает диалоги для ViewModel
        /// </summary>
        /// <param name="dialogs">Список диалогов текущего пользователя</param>
        private void SetDialogs(List<Dialog> dialogs)
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

            Dialogs = new ObservableCollection<Dialog>(dialogs.OrderByDescending(d => d.Messages.Last().DateTime));

            foreach (var dialog in Dialogs)
            {
                dialog.CheckLastMessagesRead();
            }
        }

        /// <summary>
        /// Проверить элементы управления поиском во view
        /// </summary>
        private void CheckSearchControls()
        {
            if (_selectedSearchedUser != null)
            {
                IsGreetButtonEnabled = true;
                WasSearchedUserSelected = true;

                if (HasDialogWithSelectedUser())
                {
                    IsGreetTextBoxVisible = false;
                    GreetButtonText = "Открыть диалог";
                }

                else
                {
                    IsGreetTextBoxEnabled = true;
                    IsGreetTextBoxVisible = true;
                    GreetButtonText = "Поприветствовать";
                }
            }
            else
            {
                IsGreetTextBoxVisible = false;
                WasSearchedUserSelected = false;
            }
        }

        /// <summary>
        /// Обработчик события закрытия окна чатов
        /// </summary>
        /// <param name="sender">Объект вызвавший события</param>
        /// <param name="e">Содержит данные о событии</param>
        public void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}