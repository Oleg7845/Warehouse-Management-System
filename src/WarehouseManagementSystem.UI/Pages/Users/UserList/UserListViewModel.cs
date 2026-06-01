using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Pages.Users.UserCreate;
using WarehouseManagementSystem.UI.Pages.Users.UserView;
using WarehouseManagementSystem.UI.Resources.Constants;

namespace WarehouseManagementSystem.UI.Pages.Users.UserList;

public partial class UserListViewModel
    : ObservableObject,
    IPageInfo
{
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;
    private readonly IUserService _userService;

    public string PageTitle => "Users";

    [ObservableProperty]
    private ObservableCollection<User>? _users = [];

    [ObservableProperty]
    private User? _selectedUser;

    private bool _isLoading;

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private int _totalPages = 1;

    public IReadOnlyList<int> PageSizeOptions { get; } =
    [
        50,
        100,
        150
    ];

    [ObservableProperty]
    private ObservableCollection<UserRole> _availableRoles;

    [ObservableProperty]
    private int _pageSize;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private UserRole? _selectedRole;

    [ObservableProperty]
    private bool _isDialogOpen;

    [ObservableProperty]
    private object? _dialogContent;

    [ObservableProperty]
    private bool _isFiltersOpen = false;

    [ObservableProperty]
    private string _filtersButtonIcon;

    [ObservableProperty]
    private bool _isFiltersUsed = false;

    public UserListViewModel(
        INotificationService notificationService,
        IDialogService dialogService,
        IUserService userService)
    {
        _notificationService = notificationService;
        _dialogService = dialogService;
        _userService = userService;

        PageSize = PageSizeOptions.FirstOrDefault();
        LoadUsersCommand.Execute(null);

        FiltersButtonIcon = UiConstants.ArrowDownIconPath;

        AvailableRoles = new ObservableCollection<UserRole>(
            Enum.GetValues<UserRole>()
        );
    }

    [RelayCommand]
    private void ToggleFilters()
    {
        IsFiltersOpen = !IsFiltersOpen;

        if (IsFiltersOpen)
        {
            FiltersButtonIcon = UiConstants.ArrowUpIconPath;
        }
        else
        {
            FiltersButtonIcon = UiConstants.ArrowDownIconPath;
        }
    }

    [RelayCommand]
    private void ClearFilters()
    {
        SelectedRole = null;

        IsFiltersUsed = false;
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await LoadUsersAsync();
    }

    partial void OnCurrentPageChanged(int value)
    {
        LoadUsersCommand.Execute(null);
    }

    partial void OnPageSizeChanged(int value)
    {
        LoadUsersCommand.Execute(null);
    }

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrEmpty(value)) LoadUsersCommand.Execute(null);
    }

    partial void OnSelectedRoleChanged(UserRole? value)
    {
        IsFiltersUsed = true;
        LoadUsersCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadUsersAsync()
    {
        if (_isLoading) return;

        _isLoading = true;

        try
        {
            var query = new UserSearchQuery()
            {
                Search = SearchText,
                Role = SelectedRole,
                Offset = PageSize * (CurrentPage - 1),
                Limit = PageSize
            };

            int totalUsers = await _userService.GetUsersCountAsync(query);
            TotalPages = (totalUsers + PageSize - 1) / PageSize;

            List<User> users = await _userService.GetUsersAsync(query);

            if (users == null) return;

            Users!.Clear();

            foreach (User user in users)
            {
                Users.Add(user);
            }
        }
        catch (DomainException ex)
        {
            _notificationService.ShowNotification(
                text: ex.Message,
                type: NotifyType.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    [RelayCommand]
    private async Task OpenCreateUserDialogAsync()
    {
        object? result = await _dialogService.ShowDialogAsync<UserCreateViewModel>();

        if (result is bool isSaved && isSaved)
        {
            await LoadUsersAsync();
        }
        else
        {
            await LoadUsersAsync();
        }
    }

    [RelayCommand]
    private async Task OpenViewUserDialogAsync(User? user)
    {
        if (user is null)
            return;

        await Task.Delay(150);  // Fix dialog closing

        object? result = await _dialogService.ShowDialogAsync<UserViewModel>(vm =>
        {
            vm.Initialize(user);
        });

        if (result is bool isSaved && isSaved)
        {
            await LoadUsersAsync();
        }
        else
        {
            await LoadUsersAsync();
        }
    }
}
