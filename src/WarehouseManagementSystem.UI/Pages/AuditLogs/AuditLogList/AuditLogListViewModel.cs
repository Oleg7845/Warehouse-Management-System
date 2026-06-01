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
using WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogView;
using WarehouseManagementSystem.UI.Resources.Constants;

namespace WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogList;

public partial class AuditLogListViewModel
    : ObservableObject,
    IPageInfo,
    IRecipient<AuditLogSearchQueryMessage>
{
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;
    private readonly IAuditLogService _auditLogService;

    public string PageTitle => "Audit Logs";

    [ObservableProperty]
    private ObservableCollection<AuditLog>? _logs = [];

    [ObservableProperty]
    private AuditLog? _selectedLog;

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
    private ObservableCollection<AuditLogActorType> _availableActorTypes;

    [ObservableProperty]
    private ObservableCollection<AuditLogAction> _availableActions;

    [ObservableProperty]
    private ObservableCollection<AuditLogSubjectType> _availableSubjectTypes;

    [ObservableProperty]
    private int _pageSize;

    [ObservableProperty]
    private string? _searchText;

    [ObservableProperty]
    private AuditLogActorType? _selectedActorType;

    [ObservableProperty]
    private AuditLogAction? _selectedAction;

    [ObservableProperty]
    private AuditLogSubjectType? _selectedSubjectType;

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

    public AuditLogListViewModel(
        INotificationService notificationService,
        IDialogService dialogService,
        IAuditLogService auditLogService)
    {
        _notificationService = notificationService;
        _dialogService = dialogService;
        _auditLogService = auditLogService;

        PageSize = PageSizeOptions.FirstOrDefault();
        LoadLogsCommand.Execute(null);

        FiltersButtonIcon = UiConstants.ArrowDownIconPath;

        AvailableActorTypes = new ObservableCollection<AuditLogActorType>(
            Enum.GetValues<AuditLogActorType>()
        );

        AvailableActions = new ObservableCollection<AuditLogAction>(
            Enum.GetValues<AuditLogAction>()
        );

        AvailableSubjectTypes = new ObservableCollection<AuditLogSubjectType>(
            Enum.GetValues<AuditLogSubjectType>()
        );

        WeakReferenceMessenger.Default.Register<AuditLogSearchQueryMessage>(this);
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
        SelectedActorType = null;
        SelectedAction = null;
        SelectedSubjectType = null;

        IsFiltersUsed = false;
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await LoadLogsAsync();
    }

    partial void OnCurrentPageChanged(int value)
    {
        LoadLogsCommand.Execute(null);
    }

    partial void OnPageSizeChanged(int value)
    {
        LoadLogsCommand.Execute(null);
    }

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrEmpty(value)) LoadLogsCommand.Execute(null);
    }

    partial void OnSelectedActorTypeChanged(AuditLogActorType? value)
    {
        IsFiltersUsed = true;
        if (!_isSilentUpdate) LoadLogsCommand.Execute(null);
    }

    partial void OnSelectedActionChanged(AuditLogAction? value)
    {
        IsFiltersUsed = true;
        if (!_isSilentUpdate) LoadLogsCommand.Execute(null);
    }

    partial void OnSelectedSubjectTypeChanged(AuditLogSubjectType? value)
    {
        IsFiltersUsed = true;
        if (!_isSilentUpdate) LoadLogsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadLogsAsync()
    {
        if (_isLoading) return;

        _isLoading = true;

        try
        {
            var query = new AuditLogSearchQuery()
            {
                Search = SearchText,
                ActorType = SelectedActorType,
                Action = SelectedAction,
                SubjectType = SelectedSubjectType,
                Offset = PageSize * (CurrentPage - 1),
                Limit = PageSize
            };

            int totalLogs = await _auditLogService.GetAuditLogsCountAsync(query);
            TotalPages = (totalLogs + PageSize - 1) / PageSize;

            List<AuditLog> logs = await _auditLogService.GetAuditLogsAsync(query);

            if (logs == null) return;

            Logs!.Clear();

            foreach (AuditLog log in logs)
            {
                Logs.Add(log);
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
    private async Task OpenViewAuditLogDialog(AuditLog? log)
    {
        if (log is null)
            return;

        await Task.Delay(150);  // Fix dialog closing

        object? result = await _dialogService.ShowDialogAsync<AuditLogViewModel>(vm =>
        {
            vm.Initialize(log);
        });

        if (result is bool isSaved && isSaved)
        {
            await LoadLogsAsync();
        }
        else
        {
            await LoadLogsAsync();
        }
    }

    public void Receive(AuditLogSearchQueryMessage message)
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
                    SelectedActorType = message.query.ActorType;
                    SelectedAction = message.query.Action;
                    SelectedSubjectType = message.query.SubjectType;
                }
                finally
                {
                    _isSilentUpdate = false;
                }

                LoadLogsCommand.Execute(null);
            });
        });






    }
}
