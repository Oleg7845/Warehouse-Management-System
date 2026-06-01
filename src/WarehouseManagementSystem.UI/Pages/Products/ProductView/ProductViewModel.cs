using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Products.ProductView;

public partial class ProductViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Product view";

    public ProductViewModel()
    {

    }
}
