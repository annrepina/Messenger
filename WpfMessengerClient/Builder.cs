using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMessengerClient
{
    /// <summary>
    /// Обобщенный абстрактный класс Билдер
    /// </summary>
    public abstract class Builder<T>
        where T : class, new()
    {
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Builder()
        {
            Reset();
        }

        /// <summary>
        /// Строящийся элемент
        /// </summary>
        protected T? _element;

        /// <summary>
        /// Сбросить строящийся элемент
        /// </summary>
        protected void Reset()
        {
            _element = new T();
        }

        /// <summary>
        /// Метож возвращающий результат строительства
        /// </summary>
        /// <returns></returns>
        public T GetResult()
        {
            T result = _element;

            Reset();

            return result;
        }

        /// <summary>
        /// Построить элемент
        /// </summary>
        public abstract void Build();
    }
}