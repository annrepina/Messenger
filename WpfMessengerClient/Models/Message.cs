using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private User _userSender;

        ///// <summary>
        ///// Диалог, в котором существует сообщение
        ///// </summary>
        //private Dialog? _dialog;

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        private int _id;

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
        public User UserSender
        {
            get => _userSender;

            set
            {
                _userSender = value;

                OnPropertyChanged(nameof(UserSender));
            }
        }

        ///// <summary>
        ///// Свойство - диалог в котором существует сообщение
        ///// </summary>
        //public Dialog? Dialog
        //{
        //    get => _dialog;

        //    set
        //    {
        //        _dialog = value;

        //        OnPropertyChanged(nameof(Dialog));
        //    }
        //}

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

        #endregion Свойства

        #region Конструкторы

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param _name="text">Текст сообщения</param>
        /// <param _name="senderUserAccount">Данные о пользователе - отправителе сообщения</param>
        /// <param _name="dialog">Диалог, в котором существует сообщение</param>
        public Message(string text, User senderUserAccount/*, Dialog? dialog*/)
        {
            Id = 0;
            _text = text;
            _userSender = senderUserAccount;
            _isRead = false;
            _dateTime = DateTime.Now;
            //_dialog = dialog;
        }

        #endregion Конструкторы
    }
}