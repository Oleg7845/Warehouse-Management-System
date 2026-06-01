using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.UI.Messages;

public record ShowNotificationMessage(string text, NotifyType type);
