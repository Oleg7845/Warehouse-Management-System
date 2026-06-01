using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.Domain.Exceptions;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Queries;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Messages;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketCreate;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketView;
using WarehouseManagementSystem.UI.Resources.Constants;

namespace WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketList;

public partial class SupportTicketListViewModel
    : ObservableObject,
    IRecipient<SupportTicketSearchQueryMessage>
{
    private readonly IUserSessionService _userSessionService;
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;
    private readonly ISupportTicketService _supportTicketService;

    [ObservableProperty]
    private ObservableCollection<SupportTicket>? _tickets = [];

    [ObservableProperty]
    private SupportTicket? _selectedTicket;

    private bool _isLoading;

    private bool _isSilentUpdate = false;

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
    private ObservableCollection<TicketStatus> _availableStatuses;

    [ObservableProperty]
    private ObservableCollection<TicketPriority> _availablePriorities;

    [ObservableProperty]
    private int _pageSize;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private TicketStatus? _selectedStatus;

    [ObservableProperty]
    private TicketPriority? _selectedPriority;

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

    [ObservableProperty]
    private bool _IsUserMode = false;

    public SupportTicketListViewModel(
        IUserSessionService userSessionService,
        INotificationService notificationService,
        IDialogService dialogService,
        ISupportTicketService supportTicketService)
    {
        _userSessionService = userSessionService;
        _notificationService = notificationService;
        _dialogService = dialogService;
        _supportTicketService = supportTicketService;

        PageSize = PageSizeOptions.FirstOrDefault();
        LoadTicketsCommand.Execute(null);

        FiltersButtonIcon = UiConstants.ArrowDownIconPath;

        AvailableStatuses = new ObservableCollection<TicketStatus>(
            Enum.GetValues<TicketStatus>()
        );

        AvailablePriorities = new ObservableCollection<TicketPriority>(
            Enum.GetValues<TicketPriority>()
        );

        WeakReferenceMessenger.Default.Register<SupportTicketSearchQueryMessage>(this);
    }

    public void SetUserMode()
    {
        IsUserMode = true;
    }

    partial void OnIsUserModeChanged(bool value)
    {
        _isLoading = false;
        LoadTicketsCommand.Execute(null);
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
        SelectedStatus = null;
        SelectedPriority = null;

        IsFiltersUsed = false;
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await LoadTicketsAsync();
    }

    partial void OnCurrentPageChanged(int value)
    {
        LoadTicketsCommand.Execute(null);
    }

    partial void OnPageSizeChanged(int value)
    {
        LoadTicketsCommand.Execute(null);
    }

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrEmpty(value)) LoadTicketsCommand.Execute(null);
    }

    partial void OnSelectedStatusChanged(TicketStatus? value)
    {
        IsFiltersUsed = true;
        if (!_isSilentUpdate) LoadTicketsCommand.Execute(null);
    }

    partial void OnSelectedPriorityChanged(TicketPriority? value)
    {
        IsFiltersUsed = true;
        if (!_isSilentUpdate) LoadTicketsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadTicketsAsync()
    {
        if (_isLoading) return;

        _isLoading = true;
        
        try
        {
            var query = new SupportTicketSearchQuery()
            {
                CreatedByUsername = IsUserMode ? _userSessionService.CurrentUser?.Username : null,
                Search = SearchText,
                Status = SelectedStatus,
                Priority = SelectedPriority,
                Offset = PageSize * (CurrentPage - 1),
                Limit = PageSize
            };

            int totalTickets = await _supportTicketService.GetSupportTicketsCountAsync(query);
            TotalPages = (totalTickets + PageSize - 1) / PageSize;

            List<SupportTicket> tickets = await _supportTicketService.GetSupportTicketsAsync(query);

            if (tickets == null) return;

            Tickets!.Clear();

            foreach (SupportTicket ticket in tickets)
            {
                Tickets.Add(ticket);
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
    private async Task OpenCreateSupportDialogAsync()
    {
        if(!IsUserMode) return;

        object? result = await _dialogService.ShowDialogAsync<SupportTicketCreateViewModel>();

        if (result is bool isSaved && isSaved)
        {
            await LoadTicketsAsync();
        }
        else
        {
            await LoadTicketsAsync();
        }
    }

    [RelayCommand]
    private async Task OpenViewSupportTicketDialogAsync(SupportTicket? ticket)
    {
        if (ticket is null)
            return;

        await Task.Delay(150);  // Fix dialog closing

        object? result = await _dialogService.ShowDialogAsync<SupportTicketViewModel>(
            vm => vm.Initialize(ticket));

        if (result is bool isSaved && isSaved)
        {
            await LoadTicketsAsync();
        }
        else
        {
            await LoadTicketsAsync();
        }
    }

    public void Receive(SupportTicketSearchQueryMessage message)
    {
        _ = Task.Run(async () =>
        {
            while (_isLoading)
            {
                await Task.Delay(50);
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                _isSilentUpdate = true;

                try
                {
                    SearchText = message.query.Search;
                    SelectedStatus = message.query.Status;
                    SelectedPriority = message.query.Priority;
                }
                finally
                {
                    _isSilentUpdate = false;
                }

                LoadTicketsCommand.Execute(null);
            });
        });
    }
}
