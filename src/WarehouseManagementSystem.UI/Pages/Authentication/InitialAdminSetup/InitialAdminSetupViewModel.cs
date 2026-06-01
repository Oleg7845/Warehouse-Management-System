using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WarehouseManagementSystem.Domain.Exceptions;
using System.Collections.ObjectModel;
using WarehouseManagementSystem.Application.Abstractions.Security;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Pages.Authentication.Login;

namespace WarehouseManagementSystem.UI.Pages.Authentication.InitialAdminSetup;

public partial class InitialAdminSetupViewModel
    : ObservableObject,
    IWindowSettings
{
    private readonly INotificationService _notificationService;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;

    public double MinWindowWidth => 700;
    public double MinWindowHeight => 400;

    private bool CanCreateAdmin => !IsLocked;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateInitialAdminCommand))]
    private bool _isLocked;

    [ObservableProperty]
    private ObservableCollection<UserRole> _availableRoles;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private UserRole _selectedRole;

    [ObservableProperty]
    private string? _password;

    public InitialAdminSetupViewModel(
        INotificationService notificationService,
        IPasswordGenerator passwordGenerator,
        IUserService userService,
        INavigationService navigationService)
    {
        _notificationService = notificationService;
        _passwordGenerator = passwordGenerator;
        _userService = userService;
        _navigationService = navigationService;

        AvailableRoles = new ObservableCollection<UserRole>(
            Enum.GetValues<UserRole>()
        );

        SelectedRole = UserRole.Admin;

        _ = GeneratePasswordAsync();
    }

    [RelayCommand]
    private async Task GeneratePasswordAsync()
    {
        Password = _passwordGenerator.GenerateText(16, true, true, true, true);
    }

    [RelayCommand(CanExecute = nameof(CanCreateAdmin))]
    private async Task CreateInitialAdminAsync()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            _notificationService.ShowNotification(
                text: "Field Username is required",
                type: NotifyType.Error);

            return;
        }
        else if (string.IsNullOrWhiteSpace(SelectedRole.ToString()))
        {
            _notificationService.ShowNotification(
                text: "Field Role is required",
                type: NotifyType.Error);

            return;
        }
        else if (string.IsNullOrWhiteSpace(Password))
        {
            _notificationService.ShowNotification(
                text: "Field Password is required",
                type: NotifyType.Error);

            return;
        }

        IsLocked = true;

        try
        {
            await _userService.CreateUserAsync(Username, Password, UserRole.Admin, UserCreationContext.InitialAdminSetup);
            _navigationService.NavigateTo<LoginViewModel>();

            Username = string.Empty;
            Password = string.Empty;
        } catch (DomainException ex)
        {
            _notificationService.ShowNotification(
                text: ex.Message,
                type: NotifyType.Error);
        }
        finally
        {
            IsLocked = false;
        }
    }
}
