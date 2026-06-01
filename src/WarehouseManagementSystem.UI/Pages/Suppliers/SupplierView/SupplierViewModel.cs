using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Suppliers.SupplierView;

public partial class SupplierViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Suppliers";

    public SupplierViewModel()
    {

    }
}
