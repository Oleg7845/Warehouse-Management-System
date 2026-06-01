using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.StockOperations.StockOperationList;

public partial class StockOperationListViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Stock Operation";

    public StockOperationListViewModel()
    {

    }
}
