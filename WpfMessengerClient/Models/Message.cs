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
    /// Класс - текстовое сообщение, которыми обмениваются пользователи
    /// </summary>
    public class Message : BaseNotifyPropertyChanged
    {
        #region Приватные поля

        /// <inheritdoc cref="Id"/>
        private int _id;

        /// <inheritdoc cref="Text"/>
        private string _text;

        /// <inheritdoc cref="IsRead"/>
        private bool _isRead;

        /// <inheritdoc cref="DateTime"/>
        private DateTime _dateTime;

        /// <inheritdoc cref="UserSender"/>
        private User? _userSender;

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
        /// Свойство - пользователь-отправитель сообщения
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
        public string Time { get => DateTime.ToString("dd-MM-yyyy hh:mm"); }

        /// <summary>
        /// Является ли это сообщение сообщением текущего пользователя?
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

        ///// <summary>
        ///// Горизонтальное выравнивание
        ///// </summary>
        //public TextAlignment TextAlignment
        //{
        //    get => IsCurrentUserMessage ? TextAlignment.Right : TextAlignment.Left;
        //}

        ///// <summary>
        ///// Горизонтальное выравнивание
        ///// </summary>
        //public HorizontalAlignment HorizontalAlignment
        //{
        //    get => IsCurrentUserMessage ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        //}

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