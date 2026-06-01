using System.Windows.Controls;
using System.Windows.Input;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.UI.Pages.Users.UserList;

/// <summary>
/// Interaction logic for UserListView.xaml
/// </summary>
public partial class UserListView : UserControl
{
    public UserListView()
    {
        InitializeComponent();
    }

    private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGridRow row && row.Item is User user)
        {
            if (DataContext is UserListViewModel vm)
            {
                vm.OpenViewUserDialogCommand.Execute(user);
            }
        }
    }
}
