using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace graphics_engine;

/// <summary>
/// A high-performance WPF viewport control rendering a 3D wireframe scene using low-level <see cref="DrawingVisual"/> and immediate-mode drawing.
/// </summary>
public class EngineViewport : FrameworkElement
{
    private const float DefaultCubeSize = 180f;
    private const float DefaultCubeZ = -400f;
    private const float GizmoLength = 60f;
    private const float KeyPanStep = 10f;
    private const float KeyZStep = 20f;
    private const float WheelZStep = 25f;

    private static readonly Brush BackgroundBrush = CreateFrozenBrush(Color.FromRgb(0x1E, 0x1E, 0x1E));
    private static readonly Pen WireframePen = CreateFrozenPen(Color.FromRgb(0x00, 0xBC, 0xD4), 1.5);
    private static readonly Pen XAxisPen = CreateFrozenPen(Colors.Red, 2.5);
    private static readonly Pen YAxisPen = CreateFrozenPen(Colors.LimeGreen, 2.5);
    private static readonly Pen ZAxisPen = CreateFrozenPen(Colors.DodgerBlue, 2.5);

    private readonly DrawingVisual _drawingVisual = new();
    private WireframeScene? _scene;

    private bool _isDragging;
    private Point _lastMousePosition;

    /// <summary>
    /// Occurs when the scene transforms change or the viewport re-renders, providing updated centroid information.
    /// </summary>
    public event EventHandler? SceneChanged;

    /// <summary>
    /// Identifies the <see cref="Mode"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
        nameof(Mode),
        typeof(TransformMode),
        typeof(EngineViewport),
        new FrameworkPropertyMetadata(TransformMode.None, FrameworkPropertyMetadataOptions.AffectsRender, OnModeChanged));

    /// <summary>
    /// Gets or sets the active editor transformation mode (e.g., None, Translate, Rotate, Scale).
    /// </summary>
    public TransformMode Mode
    {
        get => (TransformMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    /// <summary>
    /// Alias for <see cref="Mode"/> to support editor manipulation modes.
    /// </summary>
    public TransformMode TransformMode
    {
        get => Mode;
        set => Mode = value;
    }

    /// <summary>
    /// Gets or sets the active 3D wireframe scene rendered in the viewport.
    /// </summary>
    public WireframeScene? Scene
    {
        get => _scene;
        set
        {
            _scene = value;
            RenderFrame();
            SceneChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets the underlying <see cref="DrawingVisual"/> used for low-level visual rendering.
    /// </summary>
    public DrawingVisual DrawingVisual => _drawingVisual;

    /// <summary>
    /// Initializes a new instance of the <see cref="EngineViewport"/> class.
    /// </summary>
    public EngineViewport()
    {
        ClipToBounds = true;
        Focusable = true;

        AddVisualChild(_drawingVisual);
        AddLogicalChild(_drawingVisual);

        Loaded += OnLoaded;
        SizeChanged += OnSizeChanged;
    }

    /// <inheritdoc/>
    protected override int VisualChildrenCount => 1;

    /// <inheritdoc/>
    protected override Visual GetVisualChild(int index)
    {
        if (index != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _drawingVisual;
    }

    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        var width = double.IsInfinity(availableSize.Width) ? 800 : availableSize.Width;
        var height = double.IsInfinity(availableSize.Height) ? 600 : availableSize.Height;
        return new Size(width, height);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        EnsureSceneInitialized();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
        {
            if (_scene == null)
            {
                InitializeScene();
            }
            else
            {
                RenderFrame();
            }
        }
    }

    private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EngineViewport viewport)
        {
            viewport.RenderFrame();
            viewport.SceneChanged?.Invoke(viewport, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Handles keyboard hotkeys for mode switching and scene translation.
    /// </summary>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (HandleKeyDown(e.Key))
        {
            e.Handled = true;
        }
    }

    /// <summary>
    /// Processes a key press for hotkey switching and keyboard navigation.
    /// </summary>
    /// <param name="key">The key pressed.</param>
    /// <returns><c>true</c> if the key was handled; otherwise, <c>false</c>.</returns>
    public bool HandleKeyDown(Key key)
    {
        switch (key)
        {
            case Key.Q:
                Mode = TransformMode.None;
                return true;

            case Key.W:
                Mode = TransformMode.Translate;
                return true;

            case Key.E:
                Mode = TransformMode.Rotate;
                return true;

            case Key.R:
                Mode = TransformMode.Scale;
                return true;

            case Key.Left:
                TranslateScene(-KeyPanStep, 0f, 0f);
                return true;

            case Key.Right:
                TranslateScene(KeyPanStep, 0f, 0f);
                return true;

            case Key.Up:
                TranslateScene(0f, -KeyPanStep, 0f);
                return true;

            case Key.Down:
                TranslateScene(0f, KeyPanStep, 0f);
                return true;

            case Key.PageUp:
                TranslateScene(0f, 0f, KeyZStep);
                return true;

            case Key.PageDown:
                TranslateScene(0f, 0f, -KeyZStep);
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Begins mouse drag operations and captures mouse focus.
    /// </summary>
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        Focus();

        _isDragging = true;
        _lastMousePosition = e.GetPosition(this);
        CaptureMouse();
        e.Handled = true;
    }

    /// <summary>
    /// Ends mouse drag operations and releases mouse capture.
    /// </summary>
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);

        if (_isDragging)
        {
            _isDragging = false;
            ReleaseMouseCapture();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Handles mouse drag movement according to the active transform mode.
    /// </summary>
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (!_isDragging || _scene == null)
        {
            return;
        }

        var currentPosition = e.GetPosition(this);
        var delta = currentPosition - _lastMousePosition;
        _lastMousePosition = currentPosition;

        var dx = (float)delta.X;
        var dy = (float)delta.Y;

        ApplyMouseDrag(dx, dy);
        e.Handled = true;
    }

    /// <summary>
    /// Applies a mouse drag delta according to the current mode.
    /// </summary>
    /// <param name="dx">Delta X in pixels.</param>
    /// <param name="dy">Delta Y in pixels.</param>
    public void ApplyMouseDrag(float dx, float dy)
    {
        if (_scene == null)
        {
            return;
        }

        switch (Mode)
        {
            case TransformMode.Translate:
                _scene.Translate(dx, dy, 0f);
                RenderFrame();
                SceneChanged?.Invoke(this, EventArgs.Empty);
                break;

            case TransformMode.Rotate:
                _scene.RotateOy(dx * 0.5f);
                _scene.RotateOx(-dy * 0.5f);
                RenderFrame();
                SceneChanged?.Invoke(this, EventArgs.Empty);
                break;

            case TransformMode.Scale:
                // Dragging right/down scales up; dragging left/up scales down
                var delta = dx - dy;
                var factor = 1f + (delta * 0.01f);
                if (factor > 0.001f)
                {
                    _scene.Scale(factor, factor, factor);
                    RenderFrame();
                    SceneChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case TransformMode.None:
            default:
                break;
        }
    }

    /// <summary>
    /// Handles mouse wheel scrolling to adjust Z translation.
    /// </summary>
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        var dz = (e.Delta / 120f) * WheelZStep;
        TranslateScene(0f, 0f, dz);
        e.Handled = true;
    }

    /// <summary>
    /// Translates the scene along the given offsets and updates rendering and state.
    /// </summary>
    public void TranslateScene(float dx, float dy, float dz)
    {
        if (_scene == null)
        {
            return;
        }

        _scene.Translate(dx, dy, dz);
        RenderFrame();
        SceneChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Ensures a default wireframe scene is initialized centered in front of the viewer pupil.
    /// </summary>
    public void EnsureSceneInitialized()
    {
        if (_scene == null)
        {
            InitializeScene();
        }
    }

    /// <summary>
    /// Initializes or resets the <see cref="WireframeScene"/> with a default cube centered in front of the viewer pupil.
    /// </summary>
    public void InitializeScene()
    {
        var width = GetCurrentWidth();
        var height = GetCurrentHeight();

        if (width <= 0 || height <= 0)
        {
            return;
        }

        var centerX = width / 2f;
        var centerY = height / 2f;

        var (vertices, edges) = MeshFactory.CreateCube(centerX, centerY, DefaultCubeZ, DefaultCubeSize);
        _scene = new WireframeScene(width, height, vertices, edges);
        RenderFrame();
        SceneChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Renders the current frame immediately into the underlying <see cref="DrawingVisual"/>.
    /// </summary>
    public void RenderFrame()
    {
        var width = GetCurrentWidth();
        var height = GetCurrentHeight();

        if (width <= 0 || height <= 0)
        {
            return;
        }

        using var dc = _drawingVisual.RenderOpen();

        // a) Clear the background to a dark modern editor grey (#1E1E1E)
        dc.DrawRectangle(BackgroundBrush, null, new Rect(0, 0, width, height));

        if (_scene == null)
        {
            return;
        }

        // b) Project the scene vertices with _scene.ProjectScenePerspective()
        var projectedVertices = _scene.ProjectScenePerspective();

        // c) Draw all wireframe edges connecting the projected vertices
        foreach (var edge in _scene.Edges)
        {
            if (edge.StartIndex >= 0 && edge.StartIndex < projectedVertices.Count &&
                edge.EndIndex >= 0 && edge.EndIndex < projectedVertices.Count)
            {
                var p1 = projectedVertices[edge.StartIndex];
                var p2 = projectedVertices[edge.EndIndex];
                dc.DrawLine(WireframePen, new Point(p1.X, p1.Y), new Point(p2.X, p2.Y));
            }
        }

        // d) If mode is not None, project and draw a 3D Transform Gizmo at the centroid (Red = +X, Green = +Y, Blue = +Z)
        if (Mode != TransformMode.None)
        {
            DrawGizmo(dc, _scene);
        }
    }

    /// <summary>
    /// Projects and draws the 3D transform gizmo at the scene centroid.
    /// </summary>
    private static void DrawGizmo(DrawingContext dc, WireframeScene scene)
    {
        var centroid = scene.Centroid;

        var originProj = scene.ProjectPointPerspective(centroid);
        var xProj = scene.ProjectPointPerspective(new Vector3(centroid.X + GizmoLength, centroid.Y, centroid.Z));
        var yProj = scene.ProjectPointPerspective(new Vector3(centroid.X, centroid.Y + GizmoLength, centroid.Z));
        var zProj = scene.ProjectPointPerspective(new Vector3(centroid.X, centroid.Y, centroid.Z + GizmoLength));

        var originPoint = new Point(originProj.X, originProj.Y);
        var xPoint = new Point(xProj.X, xProj.Y);
        var yPoint = new Point(yProj.X, yProj.Y);
        var zPoint = new Point(zProj.X, zProj.Y);

        // Red = +X axis
        dc.DrawLine(XAxisPen, originPoint, xPoint);

        // Green = +Y axis
        dc.DrawLine(YAxisPen, originPoint, yPoint);

        // Blue = +Z axis
        dc.DrawLine(ZAxisPen, originPoint, zPoint);
    }

    private float GetCurrentWidth()
    {
        if (ActualWidth > 0)
        {
            return (float)ActualWidth;
        }

        if (!double.IsNaN(Width) && Width > 0)
        {
            return (float)Width;
        }

        return 0f;
    }

    private float GetCurrentHeight()
    {
        if (ActualHeight > 0)
        {
            return (float)ActualHeight;
        }

        if (!double.IsNaN(Height) && Height > 0)
        {
            return (float)Height;
        }

        return 0f;
    }

    private static Brush CreateFrozenBrush(Color color)
    {
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }

    private static Pen CreateFrozenPen(Color color, double thickness)
    {
        var brush = new SolidColorBrush(color);
        brush.Freeze();
        var pen = new Pen(brush, thickness);
        pen.Freeze();
        return pen;
    }
}
