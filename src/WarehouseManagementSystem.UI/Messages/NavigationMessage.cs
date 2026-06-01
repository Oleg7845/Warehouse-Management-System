using WarehouseManagementSystem.Domain.Enums;

namespace WarehouseManagementSystem.UI.Messages;

/// <summary>
/// A messaging record dispatched to the Shell or MainViewModel to trigger a view switch.
/// </summary>
/// <param name="Page">The target page to display.</param>
public record NavigationMessage(NavigationPage Page);
