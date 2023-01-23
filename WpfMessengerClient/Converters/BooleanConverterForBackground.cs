using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfMessengerClient.Converters
{
    [ValueConversion(typeof(bool), typeof(Brush))]
    internal class BooleanConverterForBackground : MarkupExtension, IValueConverter
    {
        private static BooleanConverterForBackground _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target must be a Brush");

            else if (value is bool val)
            {
                if (val == false)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c4856"));

                return new SolidColorBrush(Colors.GhostWhite);
            }

            else
                throw new ArgumentException("The source must be a bool");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BooleanConverterForBackground();
            return _converter;
        }
    }
}
