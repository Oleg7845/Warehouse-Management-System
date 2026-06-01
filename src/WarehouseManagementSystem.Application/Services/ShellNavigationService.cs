using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementSystem.Application.Abstractions.Services;

namespace WarehouseManagementSystem.UI.Navigation;

public class ShellNavigationService : IShellNavigationService
{
    private readonly IServiceProvider _serviceProvider;
    public event Action<ObservableObject?>? CurrentViewChanged;

    public ShellNavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<T>() where T : ObservableObject
    {
        var viewModel = _serviceProvider.GetRequiredService<T>();
        CurrentViewChanged?.Invoke(viewModel);
    }
}
