using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Categories.CategoryList;

public partial class CategoryListViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Categories";

    public CategoryListViewModel()
    {

    }
}
