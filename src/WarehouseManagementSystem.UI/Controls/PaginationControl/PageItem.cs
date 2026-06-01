namespace WarehouseManagementSystem.UI.Controls;

public class PageItem
{
    public string DisplayText { get; set; } = string.Empty;
    public int? PageNumber { get; set; }
    public bool IsActive { get; set; }
    public bool IsEllipsis => PageNumber == null;
}
