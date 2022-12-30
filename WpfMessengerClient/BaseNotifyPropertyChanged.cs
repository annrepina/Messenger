﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient
{
    /// <summary>
    /// Базовый класс, который реализует интерфейс INotifyPropertyChanged
    /// </summary>
    public class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие - изменение свойства класса
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Метод, вызывающий событие PropertyChanged
        /// </summary>
        /// <param name="propName">Имя свойства</param>
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}