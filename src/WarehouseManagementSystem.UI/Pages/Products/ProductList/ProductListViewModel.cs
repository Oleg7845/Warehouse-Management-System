using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Products.ProductList;

public partial class ProductListViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Products";

    public ProductListViewModel()
    {

    }
}
