using System.Windows;
using WarehouseManagementSystem.UI.Abstractions;
using WarehouseManagementSystem.UI.Enums;

namespace WarehouseManagementSystem.UI.Pages.Main;

public partial class MainView : Window
{
    private bool _isChangingMode = false;

    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        this.SizeChanged += MainView_SizeChanged;

        viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainViewModel.WindowMode))
            {
                ApplyWindowMode(viewModel);
            }
            else if (e.PropertyName == nameof(MainViewModel.CurrentView))
            {
                ApplyWindowMode(viewModel);
            }
        };

        ApplyWindowMode(viewModel);
    }

    private void ApplyWindowMode(MainViewModel viewModel)
    {
        _isChangingMode = true;

        switch (viewModel.WindowMode)
        {
            case WindowMode.AutoSize:
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.NoResize;

                MinWidth = 0;
                MinHeight = 0;

                SizeToContent = SizeToContent.WidthAndHeight;
                break;

            case WindowMode.Resizable:
                SizeToContent = SizeToContent.Manual;
                ResizeMode = ResizeMode.CanResize;

                if (viewModel.CurrentView is IWindowSettings sized)
                {
                    MinWidth = sized.MinWindowWidth;
                    MinHeight = sized.MinWindowHeight;
                    Width = sized.MinWindowWidth;
                    Height = sized.MinWindowHeight;
                }
                break;
        }

        if (viewModel.WindowMode == WindowMode.Resizable)
        {
            UpdateLayout();
            CenterWindow();
            _isChangingMode = false;
        }
    }

    private void MainView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_isChangingMode)
        {
            CenterWindow();
            _isChangingMode = false;
        }
    }

    private void CenterWindow()
    {
        if (ActualWidth == 0 || ActualHeight == 0) return;

        Left = SystemParameters.WorkArea.Left +
               (SystemParameters.WorkArea.Width - ActualWidth) / 2;

        Top = SystemParameters.WorkArea.Top +
              (SystemParameters.WorkArea.Height - ActualHeight) / 2;
    }
}
