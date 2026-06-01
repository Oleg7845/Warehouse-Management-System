namespace WarehouseManagementSystem.UI.Abstractions;

public interface IDialogService
{
    Task<object?> ShowDialogAsync<TViewModel>(Action<TViewModel>? configure = null) where TViewModel : class;

    Task CloseDialogAsync();
}
