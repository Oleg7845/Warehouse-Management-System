using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WarehouseManagementSystem.UI.Controls
{
    public partial class SolidButton : Button
    {
        public SolidButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SolidButton), new PropertyMetadata("Button"));
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SolidButton), new PropertyMetadata(new CornerRadius(0)));
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        public static readonly DependencyProperty LeftIconProperty =
            DependencyProperty.Register("LeftIcon", typeof(ImageSource), typeof(SolidButton), new PropertyMetadata(null));
        public ImageSource LeftIcon { get => (ImageSource)GetValue(LeftIconProperty); set => SetValue(LeftIconProperty, value); }

        public static readonly DependencyProperty RightIconProperty =
            DependencyProperty.Register("RightIcon", typeof(ImageSource), typeof(SolidButton), new PropertyMetadata(null));
        public ImageSource RightIcon { get => (ImageSource)GetValue(RightIconProperty); set => SetValue(RightIconProperty, value); }

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(Brush), typeof(SolidButton), new PropertyMetadata(null));
        public Brush IconColor { get => (Brush)GetValue(IconColorProperty); set => SetValue(IconColorProperty, value); }

        public static readonly DependencyProperty InnerOrientationProperty =
            DependencyProperty.Register("InnerOrientation", typeof(HorizontalAlignment), typeof(SolidButton), new PropertyMetadata(HorizontalAlignment.Center));
        public HorizontalAlignment InnerOrientation { get => (HorizontalAlignment)GetValue(InnerOrientationProperty); set => SetValue(InnerOrientationProperty, value); }

        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(SolidButton), new PropertyMetadata(null));
        public Brush HoverBackground { get => (Brush)GetValue(HoverBackgroundProperty); set => SetValue(HoverBackgroundProperty, value); }

        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(SolidButton), new PropertyMetadata(null));
        public Brush PressedBackground { get => (Brush)GetValue(PressedBackgroundProperty); set => SetValue(PressedBackgroundProperty, value); }

        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(SolidButton), new PropertyMetadata(null));
        public Brush HoverForeground { get => (Brush)GetValue(HoverForegroundProperty); set => SetValue(HoverForegroundProperty, value); }

        public static readonly DependencyProperty HoverIconColorProperty =
            DependencyProperty.Register("HoverIconColor", typeof(Brush), typeof(SolidButton), new PropertyMetadata(null));
        public Brush HoverIconColor { get => (Brush)GetValue(HoverIconColorProperty); set => SetValue(HoverIconColorProperty, value); }
    }
}
