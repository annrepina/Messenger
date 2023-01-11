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
        private readonly NetworkMessageHandler _networkProviderUserDataMediator;

        ///// <summary>
        ///// Активный диалог
        ///// </summary>

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

        /// <summary>
        /// Текстбокс с приветственным сообщением видимый?
        /// </summary>
        private bool _isGreetingMessageTextBoxVisible;

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
                _isGreetingMessageTextBoxAvailable = value;

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
            _networkProviderUserDataMediator = networkProviderUserDataMediator;
            MessengerWindowsManager = messengerWindowsManager;
            CurrentUser = user;

            Dialogs = new ObservableCollection<Dialog>();
            Dialogs.CollectionChanged += OnDialogsChanged;
            ActiveDialog = Dialogs.FirstOrDefault();

            SearchUserResults = new ObservableCollection<User>();
            SearchUserResults.CollectionChanged += OnSearchUserResultsChanged;

#if Debug
            List<User> users = new List<User>() { new User { Name = "Ann" },
                new User { Name = "Andrew" },
                new User { Name = "John" },
                new User { Name = "Василина" },
                new User { Name = "Камилла" },
                new User { Name = "Герворк" },
                new User { Name = "Илья" },
                new User { Name = "Анастасия" },
                new User { Name = "Екатерина" },
                new User { Name = "Чуиии" },
                new User { Name = "Хан" },
                new User { Name = "Соло" },
                new User { Name = "Уиииии" },
                new User { Name = "Ann" },
                new User { Name = "Ann" },
                new User { Name = "Ann" },
                new User { Name = "Ann" },
                new User { Name = "Ann" },
                new User { Name = "Ann" },
                new User { Name = "Fifafaffa" },
            };

            SearchUserResults.AddRange(users);

            SelectedUser = SearchUserResults.FirstOrDefault();
#endif
            SelectedUser = SearchUserResults.FirstOrDefault();
            SearchCommand = new DelegateCommand(async () => await OnSearchCommandAsync());
            OpenDialogCommand = new DelegateCommand(async () => await OnOpenDialogAsync());
            IsSearchingButtonFree = true;
            SearchingRequest = new UserSearchRequest();

            GreetingMessage = new Message("", CurrentUser);
            IsGreetingMessageTextBoxAvailable = false;
            IsGreetingMessageTextBoxVisible = false;

            OpenDialogButtonText = "Поприветствовать";
            WasUserSelected = false;
            HasNotSearchResult = false;
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

                var observer = new SearchResultReceivedObserver(_networkProviderUserDataMediator, completionSource);

                await _networkProviderUserDataMediator.SendSearchRequestAsync(SearchingRequest);

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
            if (SelectedUser != null && Dialogs.FirstOrDefault(d => d.Users.Contains(SelectedUser)) == null)
            {
                IsGreetingMessageTextBoxAvailable = false;


                if (String.IsNullOrEmpty(GreetingMessage.Text))
                    MessageBox.Show("Сначала введите приветственное сообщение =)");

                else
                {
                    Dialog dialog = new Dialog(CurrentUser, SelectedUser);
                    dialog.CurrentUser = CurrentUser;

                    Message message = GreetingMessage;

                    dialog.Messages.Add(message);

                    IsGreetingMessageTextBoxAvailable = false;

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
        private void AddSearchResults(UserSearchResult userSearchResult)
        {
            List<User> relevantUsers = userSearchResult.RelevantUsers;

            if (relevantUsers.Contains(CurrentUser))
                relevantUsers.Remove(CurrentUser);

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
        private void ProcessSearchResult(UserSearchResult? userSearchResult)
        {
            if (userSearchResult != null)
                AddSearchResults(userSearchResult);

            else
            {
                ResetSearchResults();
            }
        }

        /// <summary>
        /// Сбросить параметры
        /// </summary>
        private void ResetSearchResults()
        {
            SearchUserResults.Clear();
            WasUserSelected = false;
            HasNotSearchResult = true;
            IsGreetingMessageTextBoxAvailable = false;
            IsGreetingMessageTextBoxVisible = false;
        }

        /// <summary>
        /// Проверить выбранного пользователя
        /// </summary>
        private void CheckSelectedUser()
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
}