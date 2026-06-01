using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Pages.SupportTickets.SupportTicketList;

namespace WarehouseManagementSystem.UI.Pages.SupportPanel;

public partial class SupportPanelViewModel
    : ObservableObject,
    IPageInfo
{
    public string PageTitle => "Support Panel";

    [ObservableProperty]
    private SupportTicketListViewModel _ticketListViewModel;

    public SupportPanelViewModel(
        SupportTicketListViewModel ticketListViewModel)
    {
        _ticketListViewModel = ticketListViewModel;
    }
}
