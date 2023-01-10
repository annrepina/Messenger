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
        private NetworkProviderUserDataMediator _networkProviderUserDataMediator;

        /// <summary>
        /// Активный диалог
        /// </summary>
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

        //private bool hasSearchingRequestText;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Менеджер окон приложения
        /// </summary>
        public MessengerWindowsManager MessengerWindowsManager { get; init; }

        /// <summary>
        /// Свойство - посредник между сетевым провайдером и данными пользователя
        /// </summary>
        public NetworkProviderUserDataMediator NetworkProviderUserDataMediator
        {
            get => _networkProviderUserDataMediator;

            set
            {
                _networkProviderUserDataMediator = value;

                OnPropertyChanged(nameof(NetworkProviderUserDataMediator));
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

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator">Посредник между сетевым провайдером и данными пользователя</param>
        /// <param _name="messengerWindowsManager">Менеджер окон приложения</param>
        public ChatWindowViewModel(NetworkProviderUserDataMediator networkProviderUserDataMediator, MessengerWindowsManager messengerWindowsManager)
        {
            NetworkProviderUserDataMediator = networkProviderUserDataMediator;
            MessengerWindowsManager = messengerWindowsManager;

            Dialogs = new ObservableCollection<Dialog>();
            Dialogs.CollectionChanged += OnDialogsChanged;
            ActiveDialog = Dialogs.FirstOrDefault();

            //SelectedUser = new User();
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
            IsSearchingButtonFree = true;
            SearchingRequest = new UserSearchRequest();
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
            if(String.IsNullOrEmpty(SearchingRequest.Name) && String.IsNullOrEmpty(SearchingRequest.PhoneNumber))
                MessageBox.Show("Заполните имя и/или телефон для поиска собеседника");

            else
            {
                IsSearchingButtonFree = false;

                TaskCompletionSource completionSource = new TaskCompletionSource();

                var observer = new SearchResultReceivedObserver(_networkProviderUserDataMediator, completionSource);

                await _networkProviderUserDataMediator.SendSearchRequestAsync(SearchingRequest);

                await completionSource.Task;

                IsSearchingButtonFree = true;

                UserSearchResult userSearchResult = observer.UserSearchResult;

                AddSearchResultsToObservableCollection(userSearchResult);
            }
        }

        #endregion Обработчики событий

        private void AddSearchResultsToObservableCollection(UserSearchResult userSearchResult)
        {
            List<User> relevantUsers = userSearchResult.RelevantUsers;

            foreach(User user in relevantUsers)
            {
                SearchUserResults.Add(user);
            }
        }
    }
}