using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Windows.Data;

namespace TradeAutomation.Helpers 
{
    public class ViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string typeName)
            {
                var targetTypeObj = Type.GetType(typeName);
                return value?.GetType() == targetTypeObj;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Или более простой вариант с дженериками:
    public class IsTypeConverter<T> : IValueConverter where T : class
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}