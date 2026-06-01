using System.Windows.Controls;
using System.Windows.Input;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogList;

/// <summary>
/// Interaction logic for AuditLogListView.xaml
/// </summary>
public partial class AuditLogListView : UserControl
{
    public AuditLogListView()
    {
        InitializeComponent();
    }

    private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGridRow row && row.Item is AuditLog log)
        {
            if (DataContext is AuditLogListViewModel vm)
            {
                vm.OpenViewAuditLogDialogCommand.Execute(log);
            }
        }
    }
}
