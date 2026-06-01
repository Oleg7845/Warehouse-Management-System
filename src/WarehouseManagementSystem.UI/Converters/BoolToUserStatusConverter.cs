using System.Globalization;
using System.Windows.Data;

namespace WarehouseManagementSystem.UI.Converters;

public class BoolToUserStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isActive)
        {
            return isActive ? "Active" : "Disabled";
        }

        return "Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
