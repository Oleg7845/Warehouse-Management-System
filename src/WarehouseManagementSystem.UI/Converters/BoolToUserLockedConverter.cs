using System.Globalization;
using System.Windows.Data;

namespace WarehouseManagementSystem.UI.Converters;

public class BoolToUserLockedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isLocked)
        {
            return isLocked ? "Yes" : "No";
        }

        return "Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
