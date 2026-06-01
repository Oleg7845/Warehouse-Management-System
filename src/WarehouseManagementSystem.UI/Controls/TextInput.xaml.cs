using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WarehouseManagementSystem.UI.Controls
{
    public partial class TextInput : UserControl
    {
        public TextInput()
        {
            InitializeComponent();

            this.PreviewMouseLeftButtonDown += (s, e) => {
                if (Command != null && Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            };
        }

        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(TextInput),
            new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(TextInput),
                new PropertyMetadata(null));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty LeftIconProperty =
            DependencyProperty.Register("LeftIcon", typeof(ImageSource), typeof(TextInput));
        public ImageSource LeftIcon
        {
            get => (ImageSource)GetValue(LeftIconProperty);
            set => SetValue(LeftIconProperty, value);
        }

        
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(TextInput));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextInput),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        
        public static readonly DependencyProperty IsPasswordModeProperty =
            DependencyProperty.Register("IsPasswordMode", typeof(bool), typeof(TextInput), new PropertyMetadata(false));
        public bool IsPasswordMode
        {
            get => (bool)GetValue(IsPasswordModeProperty);
            set => SetValue(IsPasswordModeProperty, value);
        }

        
        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register("IsPasswordVisible", typeof(bool), typeof(TextInput), new PropertyMetadata(false));
        public bool IsPasswordVisible
        {
            get => (bool)GetValue(IsPasswordVisibleProperty);
            set => SetValue(IsPasswordVisibleProperty, value);
        }

        private void UpdatePlaceholder()
        {
            if (plaseholder != null)
            {
                plaseholder.Visibility = string.IsNullOrEmpty(this.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void passwordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.Text != passwordInput.Password)
            {
                this.Text = passwordInput.Password;
            }
        }

        private void textInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Text = textInput.Text;

            if (IsPasswordMode && passwordInput.Password != textInput.Text)
            {
                passwordInput.Password = textInput.Text;
            }
        }

        private void togglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            IsPasswordVisible = !IsPasswordVisible;

            if (IsPasswordVisible)
            {
                textInput.Text = passwordInput.Password;
                textInput.Focus();
                textInput.SelectionStart = textInput.Text.Length;
            }
            else
            {
                passwordInput.Password = textInput.Text;
                passwordInput.Focus();
            }
        }

        public ICommand EnterCommand
        {
            get => (ICommand)GetValue(EnterCommandProperty);
            set => SetValue(EnterCommandProperty, value);
        }

        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.Register("EnterCommand", typeof(ICommand), typeof(TextInput));
    }
}