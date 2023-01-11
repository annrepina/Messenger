using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models;

namespace WpfMessengerClient.Obsevers
{
    /// <summary>
    /// Класс, который наблюдает за событием SignUp у NetworkMessageHandler
    /// </summary>
    public class SearchResultReceivedObserver
    {
        /// <summary>
        /// Посредник между пользователем и сетью
        /// </summary>
        protected readonly NetworkMessageHandler _networkProviderUserDataMediator;

        /// <summary>
        /// 
        /// </summary>
        protected readonly TaskCompletionSource _completionSource;

        /// <summary>
        /// Результат поиска пользователя в мессенджере
        /// </summary>
        public UserSearchResult? UserSearchResult { get; private set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="networkProviderUserDataMediator"></param>
        /// <param _name="completionSource"></param>
        public SearchResultReceivedObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource)
        {
            _networkProviderUserDataMediator = networkProviderUserDataMediator;
            _completionSource = completionSource;
            _networkProviderUserDataMediator.SearchResultReceived += OnSearchResultReceived;
        }

        private void OnSearchResultReceived(UserSearchResult? userSearchResult)
        {
            try
            {
                _networkProviderUserDataMediator.SearchResultReceived -= OnSearchResultReceived;

                UserSearchResult = userSearchResult;

                _completionSource.SetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }


        }
    }
}
