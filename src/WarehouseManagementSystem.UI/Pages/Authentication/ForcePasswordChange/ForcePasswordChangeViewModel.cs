using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Messages;
using WarehouseManagementSystem.UI.Pages.Authentication.Login;
using WarehouseManagementSystem.UI.Pages.Shell;

namespace WarehouseManagementSystem.UI.Pages.Authentication.ForcePasswordChange;

public partial class ForcePasswordChangeViewModel
    : ObservableObject,
    IRecipient<UserDataMessage>
{
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;

    private bool CanChange => !IsLocked;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmChangeCommand))]
    private bool _isLocked;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _newPassword;

    [ObservableProperty]
    private string? _confirmPassword;

    public ForcePasswordChangeViewModel(
        INotificationService notificationService,
        IUserService userService,
        INavigationService navigationService)
    {
        _notificationService = notificationService;
        _userService = userService;
        _navigationService = navigationService;

        WeakReferenceMessenger.Default.Register<UserDataMessage>(this);
    }

    public void Receive(UserDataMessage message)
    {
        Username = message.user.Username;
    }

    [RelayCommand(CanExecute = nameof(CanChange))]
    private async Task ConfirmChangeAsync()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            _notificationService.ShowNotification(
                text: "Unauthorized access denied",
                type: NotifyType.Error);

            return;
        }
        else if (string.IsNullOrWhiteSpace(NewPassword))
        {
            _notificationService.ShowNotification(
                text: "Field New password is required",
                type: NotifyType.Error);

            return;
        }
        else if (string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            _notificationService.ShowNotification(
                text: "Field Confirm password is required",
                type: NotifyType.Error);

            return;
        }
        else if (!NewPassword.Equals(ConfirmPassword))
        {
            _notificationService.ShowNotification(
                text: "Passwords do not match",
                type: NotifyType.Error);

            return;
        }

        IsLocked = true;

        try
        {
            User user = await _userService.ForceChangePasswordWithLoginAsync(Username, NewPassword);
            await _userService.SetUserSession(user);

            _navigationService.NavigateTo<ShellViewModel>();

            Username = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
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
    private void NavigateToLogin()
    {
        _navigationService.NavigateTo<LoginViewModel>();
    }
}
