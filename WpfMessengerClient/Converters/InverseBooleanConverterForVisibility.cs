using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMessengerClient.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanConverterForVisibility : MarkupExtension, IValueConverter
    {
        private static InverseBooleanConverterForVisibility _converter = null;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a Visibility");

            else if ((bool)value == true)
                return Visibility.Hidden;


            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new InverseBooleanConverterForVisibility();
            return _converter;
        }
    }
}
