using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace graphics_engine.Tests;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class EngineViewportTests
{
    private const float Tolerance = 1e-4f;

    [Test]
    public void Viewport_InitializesWithDefaultModeAndVisualChild()
    {
        var viewport = new EngineViewport();

        Assert.That(viewport.Mode, Is.EqualTo(TransformMode.None));
        Assert.That(viewport.TransformMode, Is.EqualTo(TransformMode.None));
        Assert.That(viewport.DrawingVisual, Is.Not.Null);
    }

    [Test]
    public void InitializeScene_CreatesCubeCenteredInFrontOfPupil()
    {
        var viewport = new EngineViewport
        {
            Width = 800,
            Height = 600
        };

        viewport.InitializeScene();

        Assert.That(viewport.Scene, Is.Not.Null);
        var centroid = viewport.Scene!.Centroid;

        Assert.That(centroid.X, Is.EqualTo(400f).Within(Tolerance));
        Assert.That(centroid.Y, Is.EqualTo(300f).Within(Tolerance));
        Assert.That(centroid.Z, Is.EqualTo(-400f).Within(Tolerance));
        Assert.That(viewport.Scene.Edges.Count, Is.EqualTo(12));
    }

    [Test]
    public void RenderFrame_WithAllTransformModes_RendersSuccessfully()
    {
        var viewport = new EngineViewport
        {
            Width = 400,
            Height = 400
        };

        viewport.InitializeScene();

        // 1. None (View mode, no gizmo)
        viewport.Mode = TransformMode.None;
        viewport.RenderFrame();
        Assert.That(viewport.DrawingVisual.Drawing, Is.Not.Null);

        // 2. Translate mode (Gizmo rendered)
        viewport.Mode = TransformMode.Translate;
        viewport.RenderFrame();
        Assert.That(viewport.DrawingVisual.Drawing, Is.Not.Null);

        // 3. Rotate mode (Gizmo rendered)
        viewport.Mode = TransformMode.Rotate;
        viewport.RenderFrame();
        Assert.That(viewport.DrawingVisual.Drawing, Is.Not.Null);

        // 4. Scale mode (Gizmo rendered)
        viewport.Mode = TransformMode.Scale;
        viewport.RenderFrame();
        Assert.That(viewport.DrawingVisual.Drawing, Is.Not.Null);
    }

    [Test]
    public void ModeProperty_SyncsWithTransformModeProperty()
    {
        var viewport = new EngineViewport();

        viewport.TransformMode = TransformMode.Rotate;
        Assert.That(viewport.Mode, Is.EqualTo(TransformMode.Rotate));

        viewport.Mode = TransformMode.Scale;
        Assert.That(viewport.TransformMode, Is.EqualTo(TransformMode.Scale));
    }

    [Test]
    public void HandleKeyDown_Hotkeys_SwitchModesCorrectly()
    {
        var viewport = new EngineViewport();

        Assert.That(viewport.HandleKeyDown(Key.W), Is.True);
        Assert.That(viewport.Mode, Is.EqualTo(TransformMode.Translate));

        Assert.That(viewport.HandleKeyDown(Key.E), Is.True);
        Assert.That(viewport.Mode, Is.EqualTo(TransformMode.Rotate));

        Assert.That(viewport.HandleKeyDown(Key.R), Is.True);
        Assert.That(viewport.Mode, Is.EqualTo(TransformMode.Scale));

        Assert.That(viewport.HandleKeyDown(Key.Q), Is.True);
        Assert.That(viewport.Mode, Is.EqualTo(TransformMode.None));
    }

    [Test]
    public void HandleKeyDown_ArrowKeysAndPageKeys_TranslateScene()
    {
        var viewport = new EngineViewport
        {
            Width = 400,
            Height = 400
        };
        viewport.InitializeScene();

        var initialCentroid = viewport.Scene!.Centroid;

        // Arrow keys
        viewport.HandleKeyDown(Key.Right);
        Assert.That(viewport.Scene.Centroid.X, Is.EqualTo(initialCentroid.X + 10f).Within(Tolerance));

        viewport.HandleKeyDown(Key.Left);
        Assert.That(viewport.Scene.Centroid.X, Is.EqualTo(initialCentroid.X).Within(Tolerance));

        viewport.HandleKeyDown(Key.Down);
        Assert.That(viewport.Scene.Centroid.Y, Is.EqualTo(initialCentroid.Y + 10f).Within(Tolerance));

        viewport.HandleKeyDown(Key.Up);
        Assert.That(viewport.Scene.Centroid.Y, Is.EqualTo(initialCentroid.Y).Within(Tolerance));

        // Page keys
        viewport.HandleKeyDown(Key.PageUp);
        Assert.That(viewport.Scene.Centroid.Z, Is.EqualTo(initialCentroid.Z + 20f).Within(Tolerance));

        viewport.HandleKeyDown(Key.PageDown);
        Assert.That(viewport.Scene.Centroid.Z, Is.EqualTo(initialCentroid.Z).Within(Tolerance));
    }

    [Test]
    public void ApplyMouseDrag_TranslateMode_TranslatesScene()
    {
        var viewport = new EngineViewport
        {
            Width = 400,
            Height = 400,
            Mode = TransformMode.Translate
        };
        viewport.InitializeScene();

        var initialCentroid = viewport.Scene!.Centroid;
        viewport.ApplyMouseDrag(15f, -25f);

        Assert.That(viewport.Scene.Centroid.X, Is.EqualTo(initialCentroid.X + 15f).Within(Tolerance));
        Assert.That(viewport.Scene.Centroid.Y, Is.EqualTo(initialCentroid.Y - 25f).Within(Tolerance));
        Assert.That(viewport.Scene.Centroid.Z, Is.EqualTo(initialCentroid.Z).Within(Tolerance));
    }

    [Test]
    public void ApplyMouseDrag_RotateMode_RotatesAroundCentroid()
    {
        var viewport = new EngineViewport
        {
            Width = 400,
            Height = 400,
            Mode = TransformMode.Rotate
        };
        viewport.InitializeScene();

        var initialCentroid = viewport.Scene!.Centroid;
        viewport.ApplyMouseDrag(30f, 20f);

        // Rotation is around centroid, so centroid must remain invariant
        Assert.That(viewport.Scene.Centroid.X, Is.EqualTo(initialCentroid.X).Within(Tolerance));
        Assert.That(viewport.Scene.Centroid.Y, Is.EqualTo(initialCentroid.Y).Within(Tolerance));
        Assert.That(viewport.Scene.Centroid.Z, Is.EqualTo(initialCentroid.Z).Within(Tolerance));
    }

    [Test]
    public void ApplyMouseDrag_ScaleMode_ScalesAroundCentroid()
    {
        var viewport = new EngineViewport
        {
            Width = 400,
            Height = 400,
            Mode = TransformMode.Scale
        };
        viewport.InitializeScene();

        var initialCentroid = viewport.Scene!.Centroid;
        viewport.ApplyMouseDrag(20f, 0f);

        // Scaling is around centroid, so centroid must remain invariant
        Assert.That(viewport.Scene.Centroid.X, Is.EqualTo(initialCentroid.X).Within(Tolerance));
        Assert.That(viewport.Scene.Centroid.Y, Is.EqualTo(initialCentroid.Y).Within(Tolerance));
        Assert.That(viewport.Scene.Centroid.Z, Is.EqualTo(initialCentroid.Z).Within(Tolerance));
    }

    [Test]
    public void SceneChanged_FiresOnTransformations()
    {
        var viewport = new EngineViewport
        {
            Width = 400,
            Height = 400,
            Mode = TransformMode.Translate
        };
        viewport.InitializeScene();

        int eventCount = 0;
        viewport.SceneChanged += (_, _) => eventCount++;

        viewport.TranslateScene(5f, 5f, 0f);
        Assert.That(eventCount, Is.EqualTo(1));

        viewport.ApplyMouseDrag(10f, 10f);
        Assert.That(eventCount, Is.EqualTo(2));
    }
}
