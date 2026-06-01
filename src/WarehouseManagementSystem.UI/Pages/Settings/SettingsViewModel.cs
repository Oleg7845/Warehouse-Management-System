using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Settings;

public partial class SettingsViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Settings";

    public SettingsViewModel()
    {

    }
}
