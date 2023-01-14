using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMessengerClient.Converters
{
    [ValueConversion(typeof(String), typeof(double))]
    public class FontSizeForNameConverter : MarkupExtension, IValueConverter
    {
        private const double MinFontSizeForUserName = 13.0;
        private const double MidFontSizeForUserName = 18.0;
        private const double MaxFontSizeForUserName = 26.0;
        private const int SecondTerminalNameLength = 20;
        private const int FirstTerminalNameLength = 15;

        private static FontSizeForNameConverter _converter = null;

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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new FontSizeForNameConverter();
            return _converter;
        }
    }
}