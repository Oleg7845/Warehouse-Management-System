using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;

namespace WarehouseManagementSystem.UI.Controls;

public partial class Pagination : UserControl
{
    public ObservableCollection<PageItem> PageItems { get; } = new();

    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(Pagination),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPaginationChanged));

    public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(nameof(TotalPages), typeof(int), typeof(Pagination),
            new PropertyMetadata(1, OnPaginationChanged));

    public static readonly DependencyProperty MaxVisiblePagesProperty =
        DependencyProperty.Register(nameof(MaxVisiblePages), typeof(int), typeof(Pagination),
            new PropertyMetadata(10, OnPaginationChanged));

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public int TotalPages
    {
        get => (int)GetValue(TotalPagesProperty);
        set => SetValue(TotalPagesProperty, value);
    }

    public int MaxVisiblePages
    {
        get => (int)GetValue(MaxVisiblePagesProperty);
        set => SetValue(MaxVisiblePagesProperty, value);
    }

    public Pagination()
    {
        InitializeComponent();
        Loaded += Pagination_Loaded;
    }

    private void Pagination_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= Pagination_Loaded;
        UpdatePagination();
    }

    private static void OnPaginationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination control)
        {
            control.UpdatePagination();
        }
    }

    private void UpdatePagination()
    {
        if (TotalPages < 1) return;

        PageItems.Clear();
        int max = Math.Max(7, MaxVisiblePages);

        if (TotalPages <= max)
        {
            for (int i = 1; i <= TotalPages; i++)
                AddPageItem(i);
        }
        else
        {
            int threshold = (max / 2) + 1;
            int radius = (max - 4) / 2;

            if (CurrentPage <= threshold)
            {
                for (int i = 1; i <= max - 1; i++) AddPageItem(i);
                AddEllipsis();
                AddPageItem(TotalPages);
            }
            else if (CurrentPage > TotalPages - threshold)
            {
                AddPageItem(1);
                AddEllipsis();
                for (int i = TotalPages - (max - 2); i <= TotalPages; i++) AddPageItem(i);
            }
            else
            {
                AddPageItem(1);
                AddEllipsis();
                for (int i = CurrentPage - radius; i <= CurrentPage + radius; i++) AddPageItem(i);
                AddEllipsis();
                AddPageItem(TotalPages);
            }
        }
    }

    private void AddPageItem(int number) =>
        PageItems.Add(new PageItem { DisplayText = number.ToString(), PageNumber = number, IsActive = number == CurrentPage });

    private void AddEllipsis() =>
        PageItems.Add(new PageItem { DisplayText = "...", PageNumber = null, IsActive = false });

    [RelayCommand]
    private void GoToPage(int? page)
    {
        if (page.HasValue && page.Value >= 1 && page.Value <= TotalPages)
            CurrentPage = page.Value;
    }

    [RelayCommand]
    private void NextPage() => GoToPage(CurrentPage + 1);

    [RelayCommand]
    private void PrevPage() => GoToPage(CurrentPage - 1);
}
