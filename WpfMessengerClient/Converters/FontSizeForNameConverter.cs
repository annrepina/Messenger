using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMessengerClient.Converters
{
    /// <summary>
    /// Конвертер Boolean переменной в double
    /// MarkupExtension - Предоставляет базовый класс для реализации расширений разметки XAML, 
    /// которые могут поддерживаться службами .NET XAML и другими средствами чтения и записи XAML.
    /// </summary>
    [ValueConversion(typeof(String), typeof(double))]
    public class FontSizeForNameConverter : MarkupExtension, IValueConverter
    {
        #region Константы

        /// <summary>
        /// Минимальный размер шрифта для имени юзера
        /// </summary>
        private const double MinFontSizeForUserName = 13.0;

        /// <summary>
        /// Средний размер шрифта для имени юзера
        /// </summary>
        private const double MidFontSizeForUserName = 18.0;

        /// <summary>
        /// Максимальный размер шрифта для имени юзера
        /// </summary>
        private const double MaxFontSizeForUserName = 26.0;

        /// <summary>
        /// Вторая граничная велична - количества символов в имени
        /// </summary>
        private const int SecondTerminalNameLength = 20;

        /// <summary>
        /// Первая граничная велична - количества символов в имени
        /// </summary>
        private const int FirstTerminalNameLength = 15;

        #endregion Константы

        /// <summary>
        /// Конвертер
        /// </summary>
        private static FontSizeForNameConverter _converter = null;

        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <param name="value">Конвертируемое значение</param>
        /// <param name="targetType">Целевой тип конвертации</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Информация о культуре для форматирования</param>
        /// <returns>Возвращает значение double</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(double))
                throw new InvalidOperationException("The target must be a double");

            else if (value is String name)
            {
                if (name.Length > SecondTerminalNameLength)
                    return MinFontSizeForUserName;

                else if (name.Length < SecondTerminalNameLength && name.Length > FirstTerminalNameLength)
                    return MidFontSizeForUserName;
                else
                    return MaxFontSizeForUserName;
            }

            else
                throw new ArgumentException("The source must be a string");
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
            if (_converter == null) _converter = new FontSizeForNameConverter();
            return _converter;
        }
    }
}