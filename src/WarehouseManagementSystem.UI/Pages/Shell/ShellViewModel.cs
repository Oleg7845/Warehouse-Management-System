using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogList;
using WarehouseManagementSystem.UI.Pages.Dashboard;
using WarehouseManagementSystem.UI.Pages.Authentication.Login;
using WarehouseManagementSystem.UI.Pages.Settings;
using WarehouseManagementSystem.UI.Pages.SupportCenter;
using WarehouseManagementSystem.UI.Pages.SupportPanel;
using WarehouseManagementSystem.UI.Pages.Users.UserList;
using WarehouseManagementSystem.UI.Pages.Products.ProductList;
using WarehouseManagementSystem.UI.Pages.Categories.CategoryList;
using WarehouseManagementSystem.UI.Pages.StockOperations.StockOperationList;
using WarehouseManagementSystem.UI.Pages.Suppliers.SupplierList;


namespace WarehouseManagementSystem.UI.Pages.Shell;

public partial class ShellViewModel
    : ObservableObject,
    IWindowSettings
{
    private readonly INavigationService _navigationService;
    private readonly IShellNavigationService _shellNavigationService;
    private readonly IUserService _userService;
    private readonly IUserSessionService _userSessionService;

    public double MinWindowWidth => 1000;
    public double MinWindowHeight => 600;


    [ObservableProperty]
    private ObservableObject? _currentView;

    [ObservableProperty]
    private string? _pageTitle;

    [ObservableProperty]
    private bool _isAdmin;

    public ShellViewModel(
        INavigationService navigationService,
        IShellNavigationService shellNavigationService,
        IUserService userService,
        IUserSessionService userSessionService)
    {
        _navigationService = navigationService;
        _shellNavigationService = shellNavigationService;
        _userService = userService;
        _userSessionService = userSessionService;

        _shellNavigationService.CurrentViewChanged += viewModel =>
        {
            CurrentView = viewModel;

            if (viewModel is IPageInfo pageInfo)
                PageTitle = pageInfo.PageTitle;
            else
                PageTitle = string.Empty;
        };

        IsAdmin = _userSessionService.CurrentUser?.Role == Domain.Enums.UserRole.Admin;

        // Set start page
        NavigateToDashboard();
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        _shellNavigationService.NavigateTo<DashboardViewModel>();
    }

    [RelayCommand]
    private void NavigateToSupportPanel()
    {
        _shellNavigationService.NavigateTo<SupportPanelViewModel>();
    }

    [RelayCommand]
    private void NavigateToAuditLog()
    {
        _shellNavigationService.NavigateTo<AuditLogListViewModel>();
    }

    [RelayCommand]
    private void NavigateToUserList()
    {
        _shellNavigationService.NavigateTo<UserListViewModel>();
    }

    [RelayCommand]
    private void NavigateToCategories()
    {
        _shellNavigationService.NavigateTo<CategoryListViewModel>();
    }

    [RelayCommand]
    private void NavigateToProductList()
    {
        _shellNavigationService.NavigateTo<ProductListViewModel>();
    }

    [RelayCommand]
    private void NavigateToStockOperations()
    {
        _shellNavigationService.NavigateTo<StockOperationListViewModel>();
    }

    [RelayCommand]
    private void NavigateToSuppliers()
    {
        _shellNavigationService.NavigateTo<SupplierListViewModel>();
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        _shellNavigationService.NavigateTo<SettingsViewModel>();
    }

    [RelayCommand]
    private void NavigateToSupportCenter()
    {
        _shellNavigationService.NavigateTo<SupportCenterViewModel>();
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _userService.LogoutAsync();
        _navigationService.NavigateTo<LoginViewModel>();
    }
}
