using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Categories.CategoryView;

public partial class CategoryViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Category view";

    public CategoryViewModel()
    {

    }
}
