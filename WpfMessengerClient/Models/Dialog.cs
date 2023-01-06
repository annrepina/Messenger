using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Класс, который представляет диалог между двумя юзерами в мессенджере
    /// </summary>
    public class Dialog : BaseNotifyPropertyChanged
    {
        #region Константы

        /// <summary>
        /// Константное кол-во пользователей в диалоге
        /// </summary>
        private const int NumberOfAccounts = 2;

        #endregion Константы

        #region Приватные поля

        /// <summary>
        /// Поле - идентификатор диалога
        /// </summary>
        private int _id;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Свойство - идентификатор диалога
        /// </summary>
        public int Id 
        { 
            get => _id;

            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Свойство - обозреваемая коллекция данных о пользователях, участвующих в диалоге
        /// </summary>
        public ObservableCollection<UserData> UserDataCollection { get; set; }

        /// <summary>
        /// Обозреваемая коллекция сообщений в диалоге
        /// </summary>
        public ObservableCollection<Message> Messages { get; set; }

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Dialog(UserData senderUser, UserData receiverUser)
        {
            Id = 0;

            UserDataCollection = new ObservableCollection<UserData>();
            UserDataCollection.CollectionChanged += OnUserDataCollectionChanged;
            UserDataCollection.Add(senderUser);
            UserDataCollection.Add(receiverUser);   

            Messages = new ObservableCollection<Message>();
            Messages.CollectionChanged += OnMessagesChanged;
        }

        #endregion Конструкторы

        #region Обработчики событий

        /// <summary>
        /// Обработчик события изменения обозреваемой коллекции данных пользователей участвующих в диалоге
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие</param>
        /// <param name="e">Содержит информацию о событии</param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnUserDataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= OnUserDataChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += OnUserDataChanged;
                }
            }
        }

        /// <summary>
        /// Обработчик события изменения данных конкретного пользователя участвующего в диалоге
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие</param>
        /// <param name="e">Содержит информацию о событии</param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnUserDataChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обработчик события изменения обозреваемой коллекции сообщений
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие</param>
        /// <param name="e">Содержит информацию о событии</param>
        private void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= OnMessageChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += OnMessageChanged;
                }
            }
        }

        /// <summary>
        /// Обработчик события измения конкретного сообщения в обозреваемой коллекции сообщений
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие</param>
        /// <param name="e">Содержит информацию о событии</param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnMessageChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion Обработчики событий
    }
}