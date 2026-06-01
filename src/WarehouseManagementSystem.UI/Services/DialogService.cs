using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementSystem.UI.Abstractions;
using MaterialDesignThemes.Wpf;

namespace WarehouseManagementSystem.UI.Services;

public class DialogService : IDialogService
{
    private readonly string _dialogIdentifier = "RootDialogHost";
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<object?> ShowDialogAsync<TViewModel>(Action<TViewModel>? configure = null) where TViewModel : class
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

        configure?.Invoke(viewModel);

        return await DialogHost.Show(viewModel, _dialogIdentifier);
    }

    public async Task CloseDialogAsync()
    {
        DialogHost.Close(_dialogIdentifier);
    }
}
