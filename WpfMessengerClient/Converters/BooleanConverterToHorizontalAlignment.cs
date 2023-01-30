using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMessengerClient.Converters
{
    /// <summary>
    /// Конвертер Boolean переменной в HorizontalAlignment
    /// MarkupExtension - Предоставляет базовый класс для реализации расширений разметки XAML, 
    /// которые могут поддерживаться службами .NET XAML и другими средствами чтения и записи XAML.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(HorizontalAlignment))]
    public class BooleanConverterToHorizontalAlignment : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Конвертер
        /// </summary>
        private static BooleanConverterToHorizontalAlignment _converter = null;

        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <param name="value">Конвертируемое значение</param>
        /// <param name="targetType">Целевой тип конвертации</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Информация о культуре для форматирования</param>
        /// <returns>Возвращает значение TextAlignment</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(HorizontalAlignment))
                throw new InvalidOperationException("The target must be a HorizontalAlignment");

            else if (value is bool val)
            {
                if (val == true)
                    return HorizontalAlignment.Right;

                return HorizontalAlignment.Left;
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
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BooleanConverterToHorizontalAlignment();
            return _converter;
        }
    }
}