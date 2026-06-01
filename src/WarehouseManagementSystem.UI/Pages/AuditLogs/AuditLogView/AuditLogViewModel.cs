using CommunityToolkit.Mvvm.ComponentModel;
using WarehouseManagementSystem.Domain.Models;

namespace WarehouseManagementSystem.UI.Pages.AuditLogs.AuditLogView;

public partial class AuditLogViewModel : ObservableObject
{
    [ObservableProperty]
    private AuditLog? _currentAuditLog;

    [ObservableProperty]
    private Dictionary<string, string>? _auditLogDetails;

    public AuditLogViewModel() { }

    public void Initialize(AuditLog log)
    {
        CurrentAuditLog = log;
    }

    partial void OnCurrentAuditLogChanged(AuditLog? value)
    {
        if(value != null) UpdateAuditLogDetails(value);
    }

    private void UpdateAuditLogDetails(AuditLog value)
    {
        if (string.IsNullOrEmpty(value?.DetailsJson))
        {
            AuditLogDetails = null;
            return;
        }

        try
        {
            AuditLogDetails = value.GetDetails<Dictionary<string, string>>();
        }
        catch
        {
            AuditLogDetails = new Dictionary<string, string>
            {
                { "Data", value.DetailsJson }
            };
        }
    }
}
