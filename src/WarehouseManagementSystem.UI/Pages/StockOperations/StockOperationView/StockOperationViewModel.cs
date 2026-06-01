using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.StockOperations.StockOperationView;

public partial class StockOperationViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Stock Operations";

    public StockOperationViewModel()
    {

    }
}
