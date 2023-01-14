using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMessengerClient.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        private static InverseBooleanConverter _converter = null;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new InverseBooleanConverter();
            return _converter;
        }
    }
}
