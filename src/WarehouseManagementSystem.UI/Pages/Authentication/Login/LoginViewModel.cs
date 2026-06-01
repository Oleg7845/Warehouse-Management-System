using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Messages;
using WarehouseManagementSystem.UI.Pages.Authentication.ForcePasswordChange;
using WarehouseManagementSystem.UI.Pages.Shell;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketCreate;

namespace WarehouseManagementSystem.UI.Pages.Authentication.Login;

public partial class LoginViewModel
    : ObservableObject,
    IWindowSettings
{
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;

    public double MinWindowWidth => 700;
    public double MinWindowHeight => 400;

    private bool CanLogin => !IsLocked;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private bool _isLocked;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _password;

    public LoginViewModel(
        INotificationService notificationService,
        IDialogService dialogService,
        IUserService userService,
        INavigationService navigationService,
        IServiceProvider serviceProvider)
    {
        _notificationService = notificationService;
        _dialogService = dialogService;
        _userService = userService;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
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

        IsLocked = true;

        try
        {
            User user = await _userService.GetVerifiedUserAsync(Username, Password);

            if (user.MustChangePassword)
            {
                _navigationService.NavigateTo<ForcePasswordChangeViewModel>();
                WeakReferenceMessenger.Default.Send(new UserDataMessage(user));
            }
            else
            {
                await _userService.SetUserSession(user);
                _navigationService.NavigateTo<ShellViewModel>();
            }

            Username = string.Empty;
            Password = string.Empty;
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
    private async Task OpenSupportCenterAsGuestAsync()
    {
        object? result = await _dialogService.ShowDialogAsync<SupportTicketCreateViewModel>(
            vm => vm.SetGuestMode());
    }
}
