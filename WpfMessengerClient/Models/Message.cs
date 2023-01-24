using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMessengerClient.Models
{
    /// <summary>
    /// Сообщения отправляемые пользователями
    /// </summary>
    public class Message : BaseNotifyPropertyChanged
    {
        #region Приватные поля

        /// <summary>
        /// Текст сообщения
        /// </summary>
        private string _text;

        /// <summary>
        /// Прочитано сообщение?
        /// </summary>
        private bool _isRead;

        /// <summary>
        /// Дата и время отправки сообщения
        /// </summary>
        private DateTime _dateTime;

        /// <summary>
        /// Данные о пользователе - отправителе сообщения
        /// </summary>
        private User? _userSender;

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        private int _id;

        /// <inheritdoc cref="IsCurrentUserMessage"/>
        private bool _isCurrentUserMessage;

        #endregion Приватные поля

        #region Свойства

        /// <summary>
        /// Свойство - идентификатор сообщения
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
        /// Свойство - текст сообщения
        /// </summary>
        public string Text
        {
            get => _text;

            set
            {
                _text = value;

                OnPropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Свойство - данные о пользователе - отправителе сообщения
        /// </summary>
        public User? UserSender
        {
            get => _userSender;

            set
            {
                _userSender = value;

                OnPropertyChanged(nameof(UserSender));
            }
        }

        /// <summary>
        /// Свойство - прочитано сообщение?
        /// </summary>
        public bool IsRead
        {
            get => _isRead;

            set
            {
                _isRead = value;

                OnPropertyChanged(nameof(IsRead));
            }
        }

        /// <summary>
        /// Свойство - дата и время отправки сообщения
        /// </summary>
        public DateTime DateTime
        {
            get => _dateTime;

            set
            {
                _dateTime = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        /// <summary>
        /// Время отправки сообщения
        /// </summary>
        public string Time
        {
            get => DateTime.ToString("dd-MM-yyyy hh:mm:ss");
        }

        /// <summary>
        /// Является ли это сообщение сообщением текущего пользователя
        /// </summary>
        public bool IsCurrentUserMessage 
        { 
            get => _isCurrentUserMessage;
            
            set
            {
                _isCurrentUserMessage = value;

                OnPropertyChanged(nameof(IsCurrentUserMessage));
            }
        }

        /// <summary>
        /// Горизонтальное выравнивание
        /// </summary>
        public TextAlignment TextAlignment
        {
            get => IsCurrentUserMessage ? TextAlignment.Right : TextAlignment.Left;
        }

        /// <summary>
        /// Горизонтальное выравнивание
        /// </summary>
        public HorizontalAlignment HorizontalAlignment
        {
            get => IsCurrentUserMessage ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        //public string MessagePresentation => $"{Time}\n{Text}";

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Message()
        {
            Id = 0;
            Text = "";
            UserSender = null;
            IsRead = false;
            DateTime = DateTime.Now;
            IsCurrentUserMessage = false;
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="text">Текст сообщения</param>
        /// <param _name="senderUserAccount">Данные о пользователе - отправителе сообщения</param>
        /// <param _name="dialog">Диалог, в котором существует сообщение</param>
        public Message(string text, User senderUserAccount, bool isCurrentUserMessage, bool isRead = false)
        {
            Id = 0;
            Text = text;
            UserSender = senderUserAccount;
            IsRead = isRead;
            DateTime = DateTime.Now;
            IsCurrentUserMessage = isCurrentUserMessage;
        }

        #endregion Конструкторы
    }
}