using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMessengerClient.Models;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Класс, который наблюдает за событием SignUp у NetworkProviderUserDataMediator
    /// </summary>
    public class SearchResultReceivedObserver
    {
        /// <summary>
        /// Посредник между пользователем и сетью
        /// </summary>
        protected readonly NetworkProviderUserDataMediator _networkProviderUserDataMediator;

        /// <summary>
        /// 
        /// </summary>
        protected readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Результат поиска пользователя в мессенджере
        /// </summary>
        public UserSearchResult UserSearchResult { get; private set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator"></param>
        /// <param _name="completionSource"></param>
        public SearchResultReceivedObserver(NetworkProviderUserDataMediator networkProviderUserDataMediator, TaskCompletionSource completionSource)
        {
            _networkProviderUserDataMediator = networkProviderUserDataMediator;
            _completionSource = completionSource;
            _networkProviderUserDataMediator.SearchResultReceived += OnSearchResultReceived;
        }
        private void OnSearchResultReceived(UserSearchResult userSearchResult)
        {
            _completionSource.SetResult();

            _networkProviderUserDataMediator.SearchResultReceived -= OnSearchResultReceived;

            UserSearchResult = userSearchResult;
        }
    }
}
