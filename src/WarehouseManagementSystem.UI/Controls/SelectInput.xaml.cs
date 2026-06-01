using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WarehouseManagementSystem.UI.Controls;

public partial class SelectInput : UserControl
{
    public SelectInput()
    {
        InitializeComponent();
    }

    // ItemsSource

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(SelectInput));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    // SelectedItem

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(SelectInput),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    // Enterprise template support

    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(SelectInput));

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    // Left icon

    public static readonly DependencyProperty LeftIconProperty =
        DependencyProperty.Register(
            nameof(LeftIcon),
            typeof(ImageSource),
            typeof(SelectInput));

    public ImageSource LeftIcon
    {
        get => (ImageSource)GetValue(LeftIconProperty);
        set => SetValue(LeftIconProperty, value);
    }

    // Placeholder

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(SelectInput));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    // Readonly
    public static readonly DependencyProperty IsReadOnlyProperty =
    DependencyProperty.Register(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(SelectInput),
        new PropertyMetadata(false));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }
}
