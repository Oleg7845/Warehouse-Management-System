using CommunityToolkit.Mvvm.ComponentModel;

namespace WarehouseManagementSystem.Application.Abstractions.Services;

public interface INavigationService
{
    event Action<ObservableObject?> CurrentViewChanged;

    void NavigateTo<T>() where T : ObservableObject;
}
