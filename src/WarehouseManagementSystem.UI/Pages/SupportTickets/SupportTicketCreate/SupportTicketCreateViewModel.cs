using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.UI.Abstractions;

namespace WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketCreate;

public partial class SupportTicketCreateViewModel : ObservableObject
{
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;
    private readonly ISupportTicketService _supportTicketService;

    private bool CanCreateUser => !IsLocked;

    [ObservableProperty]
    private bool _isGuestMode = false;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendRequestCommand))]
    private bool _isLocked;

    [ObservableProperty]
    private ObservableCollection<TicketPriority> _availablePriorities;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private TicketPriority _selectedPriority;

    public SupportTicketCreateViewModel(
        INotificationService notificationService,
        IDialogService dialogService,
        ISupportTicketService supportTicketService)
    {
        _notificationService = notificationService;
        _dialogService = dialogService;
        _supportTicketService = supportTicketService;

        AvailablePriorities = new ObservableCollection<TicketPriority>(
            Enum.GetValues<TicketPriority>()
        );
    }

    public void SetGuestMode()
    {
        IsGuestMode = true;
    }

    [RelayCommand(CanExecute = nameof(CanCreateUser))]
    private async Task SendRequestAsync()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            _notificationService.ShowNotification(
                text: "Field Title is required",
                type: NotifyType.Error);

            return;
        }
        else if (string.IsNullOrWhiteSpace(SelectedPriority.ToString()))
        {
            _notificationService.ShowNotification(
                text: "Field Priority is required",
                type: NotifyType.Error);

            return;
        }

        IsLocked = true;

        try
        {
            await _supportTicketService.CreateSupportTicket(Username, Title, Description, SelectedPriority);

            await _dialogService.CloseDialogAsync();

            _notificationService.ShowNotification(
                text: "Your request has been sent",
                type: NotifyType.Success);
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
}
