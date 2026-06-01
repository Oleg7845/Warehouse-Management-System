using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Dashboard;

public partial class DashboardViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Dashboard";

    public DashboardViewModel()
    {
       
    }
}
