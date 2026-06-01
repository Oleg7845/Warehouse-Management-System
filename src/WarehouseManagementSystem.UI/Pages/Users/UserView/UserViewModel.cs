using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using WarehouseManagementSystem.Application.Abstractions.Security;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Messages;
using WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogList;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketList;

namespace WarehouseManagementSystem.UI.Pages.Users.UserView;

public partial class UserViewModel : ObservableObject
{
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IUserService _userService;
    private readonly IAuditLogService _auditLogService;
    private readonly ISupportTicketService _supportTicketService;
    private readonly IShellNavigationService _shellNavigationService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private User? _currentUser;

    [ObservableProperty]
    private string? _userPassword;

    [ObservableProperty]
    private int _totalAuditLogs;

    [ObservableProperty]
    private int _totalSupportTickets;

    [ObservableProperty]
    private bool _copyUserCredentialsButtonVisibility = false;

    public UserViewModel(
        IPasswordGenerator passwordGenerator,
        IUserService userService,
        IAuditLogService auditLogService,
        ISupportTicketService supportTicketService,
        IShellNavigationService shellNavigationService,
        IDialogService dialogService)
    {
        _passwordGenerator = passwordGenerator;
        _userService = userService;
        _auditLogService = auditLogService;
        _supportTicketService = supportTicketService;
        _shellNavigationService = shellNavigationService;
        _dialogService = dialogService;
    }

    public void Initialize(User user)
    {
        CurrentUser = user;

        InitializeUserDataCommand.Execute(null);
    }

    [RelayCommand]
    private async Task InitializeUserDataAsync()
    {
        var auditLogQuery = new AuditLogSearchQuery() { Search = CurrentUser!.Username };
        TotalAuditLogs = await _auditLogService.GetAuditLogsCountAsync(auditLogQuery);

        var supportTicketQuery = new SupportTicketSearchQuery() { Search = CurrentUser.Username };
        TotalSupportTickets = await _supportTicketService.GetSupportTicketsCountAsync(supportTicketQuery);
    }

    [RelayCommand]
    private async Task ActivateUserAsync()
    {
        await _userService.ActivateUserAsync(CurrentUser!.Username);

        CurrentUser.Activate();
    }

    [RelayCommand]
    private async Task DeactivateUserAsync()
    {
        await _userService.DeactivateUserAsync(CurrentUser!.Username);

        CurrentUser.Deactivate();
    }

    [RelayCommand]
    private async Task ChangeUserRoleAsync()
    {
        // _shellNavigationService.NavigateTo<ChangeUserRoleViewModel>();
    }

    [RelayCommand]
    private async Task ResetFailedLoginAttemptsAsync()
    {
        await _userService.ResetFailedLoginAttemptsAsync(CurrentUser!.Username);

        CurrentUser.ResetFailedLoginAttempts();
    }

    [RelayCommand]
    private async Task UnlockUserAccessAsync()
    {
        await _userService.UnlockUserAsync(CurrentUser!.Username);

        CurrentUser.Unlock();
    }

    [RelayCommand]
    private async Task ResetUserPasswordAsync()
    {
        UserPassword = _passwordGenerator.GenerateText(16, true, true, true, true);

        await _userService.ResetPasswordAsync(CurrentUser!.Username, UserPassword);

        CurrentUser.ResetMustChangePassword();

        CopyUserCredentialsButtonVisibility = true;
    }

    [RelayCommand]
    private void CopyUserCredentials()
    {
        string UserCredentials = _userService.FormatUserCredentials(CurrentUser!.Username, UserPassword!);

        Clipboard.SetText(UserCredentials);

        CopyUserCredentialsButtonVisibility = false;

        UserPassword = string.Empty;
    }

    [RelayCommand]
    private async Task OpenUserAuditLogsAsync()
    {
        _shellNavigationService.NavigateTo<AuditLogListViewModel>();
        await _dialogService.CloseDialogAsync();

        var query = new AuditLogSearchQuery() { Search = CurrentUser!.Username };
        WeakReferenceMessenger.Default.Send(new AuditLogSearchQueryMessage(query));
    }

    [RelayCommand]
    private async Task OpenUserSupportTicketsAsync()
    {
        _shellNavigationService.NavigateTo<SupportTicketListViewModel>();
        await _dialogService.CloseDialogAsync();

        var auery = new SupportTicketSearchQuery() { Search = CurrentUser!.Username };
        WeakReferenceMessenger.Default.Send(new SupportTicketSearchQueryMessage(auery));
    }
}
