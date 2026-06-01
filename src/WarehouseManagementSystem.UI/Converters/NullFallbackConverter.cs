using System.Globalization;
using System.Windows.Data;

namespace WarehouseManagementSystem.UI.Converters;

public class NullFallbackConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 0)
            return null;

        return values[0] ?? values[1];
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
