using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.UI.Abstractions;

public interface INotificationService
{
    void ShowNotification(string text, NotifyType type);
}
