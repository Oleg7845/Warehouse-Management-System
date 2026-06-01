using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WarehouseManagementSystem.Domain.Exceptions;
using System.Collections.ObjectModel;
using System.Windows;
using WarehouseManagementSystem.Application.Abstractions.Security;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.Users.UserCreate;

public partial class UserCreateViewModel : ObservableObject
{
    private readonly INotificationService _notificationService;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IUserService _userService;
    private readonly IShellNavigationService _shellNavigationService;

    private bool CanCreateUser => !IsLocked;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateUserCommand))]
    private bool _isLocked;

    [ObservableProperty]
    private ObservableCollection<UserRole> _availableRoles;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private UserRole? _selectedRole;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private bool _copyUserCredentialsButtonVisibility = false;

    public UserCreateViewModel(
        INotificationService notificationService,
        IPasswordGenerator passwordGenerator,
        IUserService userService,
        IShellNavigationService shellNavigationService)
    {
        _notificationService = notificationService;
        _passwordGenerator = passwordGenerator;
        _userService = userService;
        _shellNavigationService = shellNavigationService;

        AvailableRoles = new ObservableCollection<UserRole>(
            Enum.GetValues<UserRole>()
        );

        _ = GeneratePasswordAsync();
    }

    [RelayCommand]
    private async Task GeneratePasswordAsync()
    {
        Password = _passwordGenerator.GenerateText(16, true, true, true, true);
    }

    [RelayCommand(CanExecute = nameof(CanCreateUser))]
    private async Task CreateUserAsync()
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
            await _userService.CreateUserAsync(Username, Password, SelectedRole!.Value);

            _notificationService.ShowNotification(
                text: "New user successfully created",
                type: NotifyType.Success);

            CopyUserCredentialsButtonVisibility = true;
        }
        catch (DomainException ex)
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

    [RelayCommand]
    private void CopyUserCredentials()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            _notificationService.ShowNotification(
                text: "Field Username is required",
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

        string UserCredentials = _userService.FormatUserCredentials(Username, Password);

        Clipboard.SetText(UserCredentials);

        _notificationService.ShowNotification(
                text: "User credentials have been copied",
                type: NotifyType.Success);

        Username = string.Empty;
        Password = string.Empty;
    }
}
