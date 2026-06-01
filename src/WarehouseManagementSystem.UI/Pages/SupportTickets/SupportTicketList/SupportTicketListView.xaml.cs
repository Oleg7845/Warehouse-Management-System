using System.Windows.Controls;
using System.Windows.Input;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketList;

/// <summary>
/// Interaction logic for SupportTicketListView.xaml
/// </summary>
public partial class SupportTicketListView : UserControl
{
    public SupportTicketListView()
    {
        InitializeComponent();
    }

    private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGridRow row && row.Item is SupportTicket user)
        {
            if (DataContext is SupportTicketListViewModel vm)
            {
                vm.OpenViewSupportTicketDialogCommand.Execute(user);
            }
        }
    }
}
