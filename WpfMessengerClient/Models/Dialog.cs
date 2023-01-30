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
        private const int NumberOfUsers = 2;

        #endregion Константы

        #region Приватные поля

        /// <inheritdoc cref="Id"/>
        private int _id;

        /// <inheritdoc cref=" CurrentUser"/>
        private User _currentUser;

        /// <inheritdoc cref="HasUnreadMessages"/>
        private bool _hasUnreadMessages;

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
        /// Свойство - название диалога, которым явдляется имя пользователя, которым не является текущий пользователь
        /// </summary>
        public string Title => Users.First(n => n.Id != _currentUser.Id).Name;

        public bool HasUnreadMessages
        {
            get
            {
                return _hasUnreadMessages;
            }

            set
            {
                _hasUnreadMessages = value;

                OnPropertyChanged(nameof(HasUnreadMessages));
            }
        }

        /// <summary>
        /// Свойство - обозреваемая коллекция данных о пользователях, участвующих в диалоге
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// Обозреваемая коллекция сообщений в диалоге
        /// </summary>
        public ObservableCollection<Message> Messages { get; set; }

        #endregion Свойства

        #region Конструкторы

        public Dialog()
        {
            Id = 0;
            //Users = new ObservableCollection<User>();
            //Users.CollectionChanged += OnUserDataCollectionChanged;
            Users = new List<User>();

            Messages = new ObservableCollection<Message>();
            Messages.CollectionChanged += OnMessagesChanged;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        public Dialog(User user1, User user2) : this()
        {
            Users.Add(user1);
            Users.Add(user2);
        }

        #endregion Конструкторы

        #region Обработчики событий

        ///// <summary>
        ///// Обработчик события изменения обозреваемой коллекции данных пользователей участвующих в диалоге
        ///// </summary>
        ///// <param _name="sender">Объект, который вызвал событие</param>
        ///// <param _name="e">Содержит информацию о событии</param>
        ///// <exception cref="NotImplementedException"></exception>
        //private void OnUserDataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.OldItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.OldItems)
        //        {
        //            item.PropertyChanged -= OnUserDataChanged;
        //        }
        //    }

        //    if (e.NewItems != null)
        //    {
        //        foreach (INotifyPropertyChanged item in e.NewItems)
        //        {
        //            item.PropertyChanged += OnUserDataChanged;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Обработчик события изменения данных конкретного пользователя участвующего в диалоге
        ///// </summary>
        ///// <param _name="sender">Объект, который вызвал событие</param>
        ///// <param _name="e">Содержит информацию о событии</param>
        ///// <exception cref="NotImplementedException"></exception>
        //private void OnUserDataChanged(object? sender, PropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Обработчик события изменения обозреваемой коллекции сообщений
        /// </summary>
        /// <param _name="sender">Объект, который вызвал событие</param>
        /// <param _name="e">Содержит информацию о событии</param>
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

            //CheckLastMessagesRead();
        }

        /// <summary>
        /// Обработчик события измения конкретного сообщения в обозреваемой коллекции сообщений
        /// </summary>
        /// <param _name="sender">Объект, который вызвал событие</param>
        /// <param _name="e">Содержит информацию о событии</param>
        private void OnMessageChanged(object? sender, PropertyChangedEventArgs e)
        {
            //if(e.PropertyName == nameof(Message.IsRead))
            //    CheckLastMessagesRead();                
        }

        #endregion Обработчики событий

        /// <summary>
        /// Проверить прочитаны ли последние сообщения от собеседника
        /// </summary>
        public void CheckLastMessagesRead()
        {
            HasUnreadMessages = Messages.Count > 0 && Messages.Last().IsCurrentUserMessage == false && Messages.Last().IsRead == false;
        }
    }
}