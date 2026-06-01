using System.Globalization;
using System.Windows.Data;

namespace WarehouseManagementSystem.UI.Converters;

public class UtcToLocalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTimeOffset time)
        {
            return time.ToLocalTime().DateTime;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
