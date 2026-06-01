using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketList;

namespace WarehouseManagementSystem.UI.Pages.SupportCenter;

public partial class SupportCenterViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Support Center";

    [ObservableProperty]
    private SupportTicketListViewModel _ticketListViewModel;

    public SupportCenterViewModel(
        SupportTicketListViewModel ticketListViewModel)
    {
        _ticketListViewModel = ticketListViewModel;

        // Set the user mode to restrict functionality
        _ticketListViewModel.SetUserMode();
    }
}
