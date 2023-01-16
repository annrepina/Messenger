using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfMessengerClient.Models.Responses;

namespace WpfMessengerClient.Obsevers
{
    ///// <summary>
    ///// Класс, который наблюдает за событием SignUpResponseReceived у NetworkMessageHandler
    ///// </summary>
    //public class SearchResultReceivedObserver : Observer<UserSearchResponse>
    //{
    //    /// <summary>
    //    /// Результат поиска пользователя в мессенджере
    //    /// </summary>
    //    //public UserSearchResponse? UserSearchResult { get; private set; }

    //    /// <summary>
    //    /// Конструктор с параметрами
    //    /// </summary>
    //    /// <param _name="networkProviderUserDataMediator"></param>
    //    /// <param _name="completionSource"></param>
    //    public SearchResultReceivedObserver(NetworkMessageHandler networkProviderUserDataMediator, TaskCompletionSource completionSource) : base(networkProviderUserDataMediator, completionSource)
    //    {
    //        _networkMessageHandler.UserSearchResponseReceived += OnEventOccured;
    //    }

    //    protected override void OnEventOccured(UserSearchResponse response)
    //    {
    //        Response = response;

    //        _networkMessageHandler.UserSearchResponseReceived -= OnEventOccured;

    //        _completionSource.SetResult();
    //    }

    //    //private void OnSearchResultReceived(UserSearchResponse? userSearchResponse)
    //    //{
    //    //    try
    //    //    {
    //    //        _networkMessageHandler.UserSearchResponseReceived -= OnSearchResultReceived;

    //    //        UserSearchResult = userSearchResponse;

    //    //        _completionSource.SetResult();
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        MessageBox.Show(ex.Message);
    //    //        throw;
    //    //    }
    //    //}
    //}
}