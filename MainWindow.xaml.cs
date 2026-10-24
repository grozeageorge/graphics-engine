using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace graphics_engine;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    private static readonly Brush ActiveBackgroundBrush = CreateFrozenBrush(Color.FromRgb(0x00, 0x7A, 0xCC));
    private static readonly Brush ActiveBorderBrush = CreateFrozenBrush(Color.FromRgb(0x00, 0xBC, 0xD4));
    private static readonly Brush InactiveBackgroundBrush = CreateFrozenBrush(Color.FromRgb(0x2A, 0x2A, 0x2A));
    private static readonly Brush InactiveBorderBrush = CreateFrozenBrush(Color.FromArgb(0x44, 0xFF, 0xFF, 0xFF));

    public MainWindow()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        KeyDown += OnWindowKeyDown;
        Viewport.SceneChanged += OnViewportSceneChanged;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateModeButtons();
        UpdateCentroidText();
        Viewport.Focus();
    }

    private void OnViewportSceneChanged(object? sender, EventArgs e)
    {
        UpdateModeButtons();
        UpdateCentroidText();
    }

    private void OnWindowKeyDown(object sender, KeyEventArgs e)
    {
        if (Viewport.HandleKeyDown(e.Key))
        {
            UpdateModeButtons();
            UpdateCentroidText();
            e.Handled = true;
        }
    }

    private void OnModeButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string modeStr &&
            Enum.TryParse<TransformMode>(modeStr, out var mode))
        {
            Viewport.Mode = mode;
            UpdateModeButtons();
            Viewport.Focus();
        }
    }

    private void UpdateModeButtons()
    {
        SetButtonVisual(BtnView, Viewport.Mode == TransformMode.None);
        SetButtonVisual(BtnTranslate, Viewport.Mode == TransformMode.Translate);
        SetButtonVisual(BtnRotate, Viewport.Mode == TransformMode.Rotate);
        SetButtonVisual(BtnScale, Viewport.Mode == TransformMode.Scale);
    }

    private static void SetButtonVisual(Button btn, bool isActive)
    {
        if (btn == null) return;

        btn.Background = isActive ? ActiveBackgroundBrush : InactiveBackgroundBrush;
        btn.BorderBrush = isActive ? ActiveBorderBrush : InactiveBorderBrush;
        btn.Foreground = isActive ? Brushes.White : CreateFrozenBrush(Color.FromRgb(0xE0, 0xE0, 0xE0));
        btn.FontWeight = isActive ? FontWeights.Bold : FontWeights.SemiBold;
    }

    private void UpdateCentroidText()
    {
        if (Viewport.Scene != null)
        {
            var c = Viewport.Scene.Centroid;
            CentroidText.Text = $"({c.X:F2}, {c.Y:F2}, {c.Z:F2})";
        }
        else
        {
            CentroidText.Text = "(0.00, 0.00, 0.00)";
        }
    }

    private static Brush CreateFrozenBrush(Color color)
    {
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }
}
