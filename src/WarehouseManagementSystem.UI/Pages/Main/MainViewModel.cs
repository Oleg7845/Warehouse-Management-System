using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using WarehouseManagementSystem.Application.Abstractions.Services;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.UI.Enums;
using WarehouseManagementSystem.UI.Messages;
using WarehouseManagementSystem.UI.Pages.Shell;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketCreate;
using WpfApp = System.Windows.Application;

namespace WarehouseManagementSystem.UI.Pages.Main;

public partial class MainViewModel
    : ObservableRecipient,
    IRecipient<ShowNotificationMessage>
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableObject? currentView;

    [ObservableProperty]
    private WindowMode windowMode;

    public ISnackbarMessageQueue SnackbarMessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

    [ObservableProperty]
    private Brush _snackbarBackground = Brushes.Gray;

    [ObservableProperty]
    private Brush _snackbarForeground = Brushes.White;

    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        _navigationService.CurrentViewChanged += viewModel =>
        {
            CurrentView = viewModel;
            UpdateWindowSettings(viewModel);
        };

        WeakReferenceMessenger.Default.Register(this);
    }

    private void UpdateWindowSettings(ObservableObject? viewModel)
    {
        WindowMode = viewModel switch
        {
            SupportTicketCreateViewModel => WindowMode.Resizable,
            ShellViewModel => WindowMode.Resizable,
            _ => WindowMode.AutoSize
        };
    }

    private void ShowNotification(string message, NotifyType type)
    {
        SnackbarBackground = type switch
        {
            NotifyType.Success => (Brush)WpfApp.Current.FindResource("LightGreen"),
            NotifyType.Warning => (Brush)WpfApp.Current.FindResource("LightOrange"),
            NotifyType.Error => (Brush)WpfApp.Current.FindResource("LightRed"),
            NotifyType.Default => (Brush)WpfApp.Current.FindResource("DarkSmoke"),
            _ => (Brush)WpfApp.Current.FindResource("DarkSmoke")
        };

        SnackbarForeground = type switch
        {
            NotifyType.Success => (Brush)WpfApp.Current.FindResource("Green"),
            NotifyType.Warning => (Brush)WpfApp.Current.FindResource("Orange"),
            NotifyType.Error => (Brush)WpfApp.Current.FindResource("Red"),
            NotifyType.Default => (Brush)WpfApp.Current.FindResource("Gray"),
            _ => (Brush)WpfApp.Current.FindResource("Gray")
        };

        SnackbarMessageQueue.Enqueue(message);
    }

    public void Receive(ShowNotificationMessage message)
    {
        ShowNotification(message.text, message.type);
    }
}
