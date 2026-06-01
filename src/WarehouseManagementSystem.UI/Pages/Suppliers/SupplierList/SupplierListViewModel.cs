using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Suppliers.SupplierList;

public partial class SupplierListViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Suppliers";

    public SupplierListViewModel()
    {

    }
}
