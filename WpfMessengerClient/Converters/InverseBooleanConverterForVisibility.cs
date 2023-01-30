using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMessengerClient.Converters
{
    /// <summary>
    /// Инвертированный конвертер Boolean переменной в Visibility
    /// MarkupExtension - Предоставляет базовый класс для реализации расширений разметки XAML, 
    /// которые могут поддерживаться службами .NET XAML и другими средствами чтения и записи XAML.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanConverterForVisibility : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Конвертер
        /// </summary>
        private static InverseBooleanConverterForVisibility _converter = null;

        #region IValueConverter Members

        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <param name="value">Конвертируемое значение</param>
        /// <param name="targetType">Целевой тип конвертации</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Информация о культуре для форматирования</param>
        /// <returns>Возвращает инвертированное значение Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a Visibility");

            else if ((bool)value == true)
                return Visibility.Hidden;


            return Visibility.Visible;
        }

        /// <summary>
        /// Обратно конвертировать
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        /// <summary>
        /// Возвращает объект, предоставленный в качестве значения целевого свойства для этого расширения разметки.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new InverseBooleanConverterForVisibility();
            return _converter;
        }
    }
}