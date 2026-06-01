using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketView;

public partial class SupportTicketViewModel : ObservableObject
{
    private readonly IUserSessionService _userSessionService;
    private readonly ISupportTicketService _supportTicketService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanTake))]
    [NotifyPropertyChangedFor(nameof(CanRequestInfo))]
    [NotifyPropertyChangedFor(nameof(CanResolve))]
    [NotifyPropertyChangedFor(nameof(CanReply))]
    [NotifyPropertyChangedFor(nameof(CanClose))]
    [NotifyPropertyChangedFor(nameof(CanReopen))]
    [NotifyPropertyChangedFor(nameof(IsTopControlBlockVisible))]
    private SupportTicket? _currentTicket;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanTake))]
    [NotifyPropertyChangedFor(nameof(CanRequestInfo))]
    [NotifyPropertyChangedFor(nameof(CanResolve))]
    [NotifyPropertyChangedFor(nameof(CanReply))]
    [NotifyPropertyChangedFor(nameof(CanClose))]
    [NotifyPropertyChangedFor(nameof(CanReopen))]
    private bool _isAdmin;

    [ObservableProperty]
    private bool _isCommentsUpdatingOpened = false;

    [ObservableProperty]
    private string? _newCommentText;

    public bool CanTake => IsAdmin && CurrentTicket?.Status == TicketStatus.Open;

    public bool CanRequestInfo => IsAdmin && CurrentTicket?.Status == TicketStatus.InProgress;

    public bool CanResolve => IsAdmin && CurrentTicket?.Status == TicketStatus.InProgress;

    public bool CanReply => !IsAdmin && CurrentTicket?.Status == TicketStatus.WaitingForUser;

    public bool CanClose => !IsAdmin && CurrentTicket?.Status == TicketStatus.Resolved;

    public bool CanReopen => !IsAdmin && CurrentTicket?.Status == TicketStatus.Resolved;

    public bool IsTopControlBlockVisible => IsAdmin
        ? (CanTake || CanRequestInfo || CanResolve)
        : (CanReply || CanClose || CanReopen);

    public SupportTicketViewModel(
        IUserSessionService userSessionService,
        ISupportTicketService supportTicketService,
        INotificationService notificationService)
    {
        _userSessionService = userSessionService;
        _supportTicketService = supportTicketService;
        _notificationService = notificationService;

        IsAdmin = _userSessionService.CurrentUser?.Role == UserRole.Admin;
    }

    public void Initialize(SupportTicket ticket)
    {
        CurrentTicket = ticket;
    }

    [RelayCommand]
    private async Task TakeTicketAsync()
    {
        CurrentTicket?.UpdateStatusToInProgress();
        await UpdateTicket();

        _notificationService.ShowNotification(
            text: "Ticket put in progress",
            type: NotifyType.Success);
    }

    [RelayCommand]
    private void RequestInfo()
    {
        ToggleCommentUpdating();
    }

    [RelayCommand]
    private async Task Resolve()
    {
        CurrentTicket?.UpdateStatusToResolved();
        await UpdateTicket();

        _notificationService.ShowNotification(
            text: "Ticket marked as Resolved",
            type: NotifyType.Success);
    }

    [RelayCommand]
    private void Reply()
    {
        ToggleCommentUpdating();
    }

    [RelayCommand]
    private async Task Close()
    {
        CurrentTicket?.UpdateStatusToClosed();
        await UpdateTicket();

        _notificationService.ShowNotification(
            text: "Ticket closed",
            type: NotifyType.Success);
    }

    [RelayCommand]
    private async Task Reopen()
    {
        CurrentTicket?.UpdateStatusToInProgress();
        await UpdateTicket();

        _notificationService.ShowNotification(
            text: "Ticket opened again",
            type: NotifyType.Warning);
    }

    [RelayCommand]
    private async Task SubmitCommentAsync()
    {
        if (string.IsNullOrEmpty(NewCommentText))
        {
            _notificationService.ShowNotification(
                text: "Comment field is empty",
                type: NotifyType.Error);
            return;
        }

        CurrentTicket?.AppendComment(
            author: _userSessionService.CurrentUser!.Username,
            text: NewCommentText);

        if (IsAdmin)
        {
            CurrentTicket?.UpdateStatusToWaitingForUser();

            _notificationService.ShowNotification(
            text: "Info requested from user",
            type: NotifyType.Warning);
        }
        else
        {
            CurrentTicket?.UpdateStatusToInProgress();

            _notificationService.ShowNotification(
            text: "Reply sent",
            type: NotifyType.Default);
        }

        CancelComment();

        await UpdateTicket();
    }

    [RelayCommand]
    private void CancelComment()
    {
        ToggleCommentUpdating();
        NewCommentText = string.Empty;
    }

    private async Task UpdateTicket()
    {
        if (CurrentTicket != null)
        {
            OnPropertyChanged(nameof(CanTake));
            OnPropertyChanged(nameof(CanRequestInfo));
            OnPropertyChanged(nameof(CanResolve));
            OnPropertyChanged(nameof(CanReply));
            OnPropertyChanged(nameof(CanClose));
            OnPropertyChanged(nameof(CanReopen));
            OnPropertyChanged(nameof(IsTopControlBlockVisible));

            OnPropertyChanged(nameof(CurrentTicket));

            await _supportTicketService.UpdateSupportTicket(CurrentTicket);
        }
    }

    private void ToggleCommentUpdating()
    {
        IsCommentsUpdatingOpened = !IsCommentsUpdatingOpened;
    }
}
