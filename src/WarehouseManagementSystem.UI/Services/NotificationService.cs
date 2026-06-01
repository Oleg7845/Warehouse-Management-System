using CommunityToolkit.Mvvm.Messaging;
using WarehouseManagementSystem.Domain.Enums;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Messages;

namespace WarehouseManagementSystem.UI.Services;

public class NotificationService : INotificationService
{
    public void ShowNotification(string text, NotifyType type)
    {
        WeakReferenceMessenger.Default.Send(new ShowNotificationMessage(text, type));
    }
}
