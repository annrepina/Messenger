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
using DtoLib.Dto;
using DtoLib.NetworkServices;
using WpfMessengerClient.Models.Responses;

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

        /// <summary>
        /// Выбранный во время поиска пользователь
        /// </summary>
        private User? _selectedUser;

        /// <summary>
        /// Данные о поисковом запросе
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

        /// <summary>
        /// Выбранный пользователь 
        /// </summary>
        public User? SelectedUser
        {
            get => _selectedUser;

            set
            {
                _selectedUser = value;

                WasUserSelected = true;
                IsOpenDialogButtonAvailable = true;

                CheckSelectedUser();

                OnPropertyChanged(nameof(SelectedUser));
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
            MessengerWindowsManager = messengerWindowsManager;
            CurrentUser = user;

            Dialogs = new ObservableCollection<Dialog>();
            Dialogs.CollectionChanged += OnDialogsChanged;
            ActiveDialog = Dialogs.FirstOrDefault();

            SearchUserResults = new ObservableCollection<User>();
            SearchUserResults.CollectionChanged += OnSearchUserResultsChanged;
            SelectedUser = SearchUserResults.FirstOrDefault();
            SearchCommand = new DelegateCommand(async () => await OnSearchCommandAsync());
            IsSearchingButtonFree = true;
            SearchingRequest = new UserSearchRequest();
            HasNotSearchResult = false;
            WasUserSelected = false;

            OpenDialogCommand = new DelegateCommand(async () => await OnOpenDialogAsync());
            OpenDialogButtonText = "Поприветствовать";
            IsOpenDialogButtonAvailable = false;

            GreetingMessage = new Message("", CurrentUser);
            IsGreetingMessageTextBoxAvailable = false;
            IsGreetingMessageTextBoxVisible = false;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обрабатывает события измения обозреваемой коллекции результатов поиска пользователей
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="e">Содержит информацию о событии</param>
        private void OnSearchUserResultsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= OnUserResultChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += OnUserResultChanged;
                }
            }
        }

        /// <summary>
        /// Обрабатывает изменение конкретного результата поиска пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserResultChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
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

                await _networkMessageHandler.SenRequestAsync<UserSearchRequest, UserSearchRequestDto>(SearchingRequest, NetworkMessageCode.SearchUserCode);

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

                    Message message = GreetingMessage;
                    dialog.Messages.Add(message);

                    TaskCompletionSource completionSource = new TaskCompletionSource();
                    var observer = new DialogCreatedObserver(_networkMessageHandler, completionSource);

                    await _networkMessageHandler.SenRequestAsync<Dialog, CreateDialogRequestDto>(dialog, NetworkMessageCode.CreateDialogCode);

                    await completionSource.Task;

                    //Dialog.

                    //Dialogs.Add



                    //var observer = 

                }


            }
            else
            {
                //
            }


        }

        #endregion Обработчики событий

        /// <summary>
        /// Добавляет список найденных пользователей в список результатов поиска пользователя
        /// </summary>
        /// <param name="userSearchResult">Представляет результат поиска пользователя</param>
        private void AddSearchResults(UserSearchResponse userSearchResult)
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
            if(_selectedUser != null)
            {
                // если уже общаемся с этим пользователем
                if (_selectedUser != null && Dialogs.FirstOrDefault(d => d.Users.Contains(_selectedUser)) != null)
                {
                    IsGreetingMessageTextBoxAvailable = false;
                    OpenDialogButtonText = "Открыть диалог";
                }

                else
                {
                    IsGreetingMessageTextBoxAvailable = true;
                    IsGreetingMessageTextBoxVisible = true;
                    OpenDialogButtonText = "Поприветствовать";
                }
            }
        }

        internal void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}