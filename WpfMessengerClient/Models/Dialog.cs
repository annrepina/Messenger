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
        /// Свойство - название диалога, которым является имя пользователя, которым не является текущий пользователь
        /// </summary>
        public string Title => Users.First(n => n.Id != _currentUser.Id).Name;

        /// <summary>
        /// У диалога есть непрочитанные сообщения?
        /// </summary>
        public bool HasUnreadMessages
        {
            get => _hasUnreadMessages;

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
        public ObservableCollection<Message> Messages { get; init; }

        #endregion Свойства

        #region Конструкторы

        public Dialog()
        {
            Id = 0;
            Users = new List<User>();
            Messages = new ObservableCollection<Message>();
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

        /// <summary>
        /// Проверить прочитаны ли последние сообщения от собеседника
        /// </summary>
        public void CheckLastMessagesRead()
        {
            HasUnreadMessages = Messages.Count > 0 && Messages.Last().IsCurrentUserMessage == false && Messages.Last().IsRead == false;
        }
    }
}