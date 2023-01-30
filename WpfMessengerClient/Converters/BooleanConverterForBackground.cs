using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfMessengerClient.Converters
{
    /// <summary>
    /// Конвертер Boolean переменной в Brush
    /// MarkupExtension - Предоставляет базовый класс для реализации расширений разметки XAML, 
    /// которые могут поддерживаться службами .NET XAML и другими средствами чтения и записи XAML.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Brush))]
    internal class BooleanConverterForBackground : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Конвертер
        /// </summary>
        private static BooleanConverterForBackground _converter = null;

        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <param name="value">Конвертируемое значение</param>
        /// <param name="targetType">Целевой тип конвертации</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Информация о культуре для форматирования</param>
        /// <returns>Возвращает значение Brush</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target must be a Brush");

            else if (value is bool val)
            {
                if (val == true)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c4856"));

                return new SolidColorBrush(Colors.Transparent);
            }

            else
                throw new ArgumentException("The source must be a bool");
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает объект, предоставленный в качестве значения целевого свойства для этого расширения разметки.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BooleanConverterForBackground();
            return _converter;
        }
    }
}