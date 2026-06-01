using CommunityToolkit.Mvvm.ComponentModel;

namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface IShellNavigationService
{
    event Action<ObservableObject?> CurrentViewChanged;

    void NavigateTo<T>() where T : ObservableObject;
}
