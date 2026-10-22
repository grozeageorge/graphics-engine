namespace graphics_engine.Tests;

[TestFixture]
public class WireframeSceneTests
{
    private const float Tolerance = 1e-4f;
    
    [Test]
    public void Constructor_InitializesAllParameters()
    {
        var vertices = new List<Vector3> { new(50f, 50f, -200f), new(0f, 0f, 0f) };
        var edges = new List<(int, int)> { new(0, 1) };
        
        var scene = new WireframeScene(100f, 100f, vertices, edges);
        
        Assert.That(scene.Edges, Is.EqualTo(edges));

        var projected = scene.ProjectScenePerspective();
        
        Assert.That(projected.Count, Is.EqualTo(2));
        Assert.That(projected[0].X, Is.EqualTo(50f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(50f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void ProjectPointPerspective_ProjectsCorrectly()
    {
        var vertices = new List<Vector3> { new(50f, 50f, -200f), new(0f, 0f, 0f) };
        var edges = new List<(int, int)> { new(0, 1) };
        
        var scene = new WireframeScene(200f, 200f, vertices, edges);
        var point = new Vector3(100f, 100f, -400f);
        
        var projected = scene.ProjectPointPerspective(point);
        
        Assert.That(projected.X, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected.Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected.Z, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void ProjectScenePerspective_ProjectsAllVertices()
    {
        var vertices = new List<Vector3> { new(100f, 100f, -400f), new(0f, 0f, 0f) };
        var edges = new List<(int, int)> { new(0, 1) };
        
        var scene = new WireframeScene(200f, 200f, vertices, edges);
        
        var projected = scene.ProjectScenePerspective();
        
        Assert.That(projected.Count, Is.EqualTo(vertices.Count));
        Assert.That(projected[0].X, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));
    }
    
    [Test]
    public void ProjectScenePerspective_EmptyVertices_ReturnsEmptyList()
    {
        var vertices = new List<Vector3>();
        var edges = new List<(int, int)>();
        
        var scene = new WireframeScene(200f, 200f, vertices, edges);
        
        var projected = scene.ProjectScenePerspective();
        
        Assert.That(projected.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void ProjectScenePerspective_TransformedVertices_ProjectCorrectly()
    {
        var vertices = new List<Vector3> { new(100f, 100f, -400f), new(0f, 0f, 0f) };
        var edges = new List<(int, int)> { new(0, 1) };
        
        var scene = new WireframeScene(200f, 200f, vertices, edges);
        
        // Apply a translation transformation to the scene: (100, 100, -400) -> (150, 150, -400)
        scene.Translate(50f, 50f, 0f);
        
        var projected = scene.ProjectScenePerspective();
        
        Assert.That(projected.Count, Is.EqualTo(vertices.Count));
        // Viewer pupil: (100, 100, 800)
        // depthRatio = 800 / (-400 - 800) = -2/3
        // xProjected = 100 - (-2/3) * (150 - 100) = 400/3 (~133.3333f)
        // yProjected = 100 + (-2/3) * (150 - 100) = 200/3 (~66.6667f)
        Assert.That(projected[0].X, Is.EqualTo(400f / 3f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(200f / 3f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));
    }

    #region Centroid-Based Rotation Tests

    [Test]
    public void RotateOz_RotatesAroundCentroid_DoesNotOrbitWorldOrigin()
    {
        // Define a vertical segment centered at (100, 100, -400), away from world origin (0, 0, 0)
        var vertices = new List<Vector3>
        {
            new(100f, 60f, -400f),
            new(100f, 140f, -400f)
        };
        var edges = new List<(int, int)> { (0, 1) };
        var scene = new WireframeScene(200f, 200f, vertices, edges);

        // Rotate 90 degrees CCW around Z relative to its centroid (100, 100, -400).
        // Vertical segment should become horizontal:
        // V0 (100, 60, -400) -> (140, 100, -400)
        // V1 (100, 140, -400) -> (60, 100, -400)
        scene.RotateOz(90f);

        var projected = scene.ProjectScenePerspective();

        Assert.That(projected.Count, Is.EqualTo(2));
        // Pupil is at (100, 100, 800), depthRatio = -2/3
        // V0: x = 100 - (-2/3)*(140 - 100) = 380/3, y = 100 + (-2/3)*(0) = 100
        Assert.That(projected[0].X, Is.EqualTo(380f / 3f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));

        // V1: x = 100 - (-2/3)*(60 - 100) = 220/3, y = 100 + (-2/3)*(0) = 100
        Assert.That(projected[1].X, Is.EqualTo(220f / 3f).Within(Tolerance));
        Assert.That(projected[1].Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[1].Z, Is.EqualTo(0f).Within(Tolerance));

        // Centroid projected position remains invariant at (100, 100, 0) rather than orbiting origin
        var centroidX = (projected[0].X + projected[1].X) / 2f;
        var centroidY = (projected[0].Y + projected[1].Y) / 2f;
        Assert.That(centroidX, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(centroidY, Is.EqualTo(100f).Within(Tolerance));
    }

    [Test]
    public void RotateOx_RotatesAroundCentroid_DoesNotOrbitWorldOrigin()
    {
        // Define a vertical segment centered at (100, 100, -400)
        var vertices = new List<Vector3>
        {
            new(100f, 140f, -400f),
            new(100f, 60f, -400f)
        };
        var edges = new List<(int, int)> { (0, 1) };
        var scene = new WireframeScene(200f, 200f, vertices, edges);

        // Rotate 180 degrees around X relative to its centroid (100, 100, -400):
        // Relative dy: +40 -> -40, -40 -> +40
        // V0 (100, 140, -400) -> (100, 60, -400)
        // V1 (100, 60, -400) -> (100, 140, -400)
        scene.RotateOx(180f);

        var projected = scene.ProjectScenePerspective();

        Assert.That(projected.Count, Is.EqualTo(2));
        // V0 (100, 60, -400): y = 100 + (-2/3)*(60 - 100) = 380/3
        Assert.That(projected[0].X, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(380f / 3f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));

        // V1 (100, 140, -400): y = 100 + (-2/3)*(140 - 100) = 220/3
        Assert.That(projected[1].X, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[1].Y, Is.EqualTo(220f / 3f).Within(Tolerance));
        Assert.That(projected[1].Z, Is.EqualTo(0f).Within(Tolerance));

        // Centroid projected position remains invariant at (100, 100, 0)
        var centroidY = (projected[0].Y + projected[1].Y) / 2f;
        Assert.That(centroidY, Is.EqualTo(100f).Within(Tolerance));
    }

    [Test]
    public void RotateOy_RotatesAroundCentroid_DoesNotOrbitWorldOrigin()
    {
        // Define a horizontal segment centered at (100, 100, -400)
        var vertices = new List<Vector3>
        {
            new(140f, 100f, -400f),
            new(60f, 100f, -400f)
        };
        var edges = new List<(int, int)> { (0, 1) };
        var scene = new WireframeScene(200f, 200f, vertices, edges);

        // Rotate 180 degrees around Y relative to its centroid (100, 100, -400):
        // Relative dx: +40 -> -40, -40 -> +40
        // V0 (140, 100, -400) -> (60, 100, -400)
        // V1 (60, 100, -400) -> (140, 100, -400)
        scene.RotateOy(180f);

        var projected = scene.ProjectScenePerspective();

        Assert.That(projected.Count, Is.EqualTo(2));
        // V0 (60, 100, -400): x = 100 - (-2/3)*(60 - 100) = 220/3
        Assert.That(projected[0].X, Is.EqualTo(220f / 3f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));

        // V1 (140, 100, -400): x = 100 - (-2/3)*(140 - 100) = 380/3
        Assert.That(projected[1].X, Is.EqualTo(380f / 3f).Within(Tolerance));
        Assert.That(projected[1].Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[1].Z, Is.EqualTo(0f).Within(Tolerance));

        // Centroid projected position remains invariant at (100, 100, 0)
        var centroidX = (projected[0].X + projected[1].X) / 2f;
        Assert.That(centroidX, Is.EqualTo(100f).Within(Tolerance));
    }

    #endregion

    #region Centroid-Based Scaling Tests

    [Test]
    public void Scale_Uniform_ScalesRelativeToCentroid()
    {
        // Define a horizontal segment centered at (100, 100, -400)
        var vertices = new List<Vector3>
        {
            new(80f, 100f, -400f),
            new(120f, 100f, -400f)
        };
        var edges = new List<(int, int)> { (0, 1) };
        var scene = new WireframeScene(200f, 200f, vertices, edges);

        // Uniform scaling by factor 2 relative to centroid (100, 100, -400):
        // Relative dx: -20 * 2 = -40 -> V0' = (60, 100, -400)
        // Relative dx: +20 * 2 = +40 -> V1' = (140, 100, -400)
        scene.Scale(2f, 2f, 2f);

        var projected = scene.ProjectScenePerspective();

        Assert.That(projected.Count, Is.EqualTo(2));
        // V0' (60, 100, -400): x = 100 - (-2/3)*(60 - 100) = 220/3
        Assert.That(projected[0].X, Is.EqualTo(220f / 3f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));

        // V1' (140, 100, -400): x = 100 - (-2/3)*(140 - 100) = 380/3
        Assert.That(projected[1].X, Is.EqualTo(380f / 3f).Within(Tolerance));
        Assert.That(projected[1].Y, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(projected[1].Z, Is.EqualTo(0f).Within(Tolerance));

        // Centroid projected position remains invariant at (100, 100, 0)
        var centroidX = (projected[0].X + projected[1].X) / 2f;
        Assert.That(centroidX, Is.EqualTo(100f).Within(Tolerance));
    }

    [Test]
    public void Scale_NonUniform_ScalesCoordinatesIndependentlyRelativeToCentroid()
    {
        // Define vertices centered at (100, 100, -400)
        var vertices = new List<Vector3>
        {
            new(90f, 80f, -400f),
            new(110f, 120f, -400f)
        };
        var edges = new List<(int, int)> { (0, 1) };
        var scene = new WireframeScene(200f, 200f, vertices, edges);

        // Non-uniform scaling (sx=3, sy=0.5, sz=1) relative to centroid (100, 100, -400):
        // V0: dx = -10 * 3 = -30 -> x = 70; dy = -20 * 0.5 = -10 -> y = 90; z = -400
        // V1: dx = +10 * 3 = +30 -> x = 130; dy = +20 * 0.5 = +10 -> y = 110; z = -400
        scene.Scale(3f, 0.5f, 1f);

        var projected = scene.ProjectScenePerspective();

        Assert.That(projected.Count, Is.EqualTo(2));
        // V0' (70, 90, -400):
        // x = 100 - (-2/3)*(70 - 100) = 80
        // y = 100 + (-2/3)*(90 - 100) = 320/3
        Assert.That(projected[0].X, Is.EqualTo(80f).Within(Tolerance));
        Assert.That(projected[0].Y, Is.EqualTo(320f / 3f).Within(Tolerance));
        Assert.That(projected[0].Z, Is.EqualTo(0f).Within(Tolerance));

        // V1' (130, 110, -400):
        // x = 100 - (-2/3)*(130 - 100) = 120
        // y = 100 + (-2/3)*(110 - 100) = 280/3
        Assert.That(projected[1].X, Is.EqualTo(120f).Within(Tolerance));
        Assert.That(projected[1].Y, Is.EqualTo(280f / 3f).Within(Tolerance));
        Assert.That(projected[1].Z, Is.EqualTo(0f).Within(Tolerance));

        // Centroid projected position remains invariant at (100, 100, 0)
        var centroidX = (projected[0].X + projected[1].X) / 2f;
        var centroidY = (projected[0].Y + projected[1].Y) / 2f;
        Assert.That(centroidX, Is.EqualTo(100f).Within(Tolerance));
        Assert.That(centroidY, Is.EqualTo(100f).Within(Tolerance));
    }

    #endregion

    #region Edge Handling Tests

    [Test]
    public void Edges_ReturnsExactIndexPairsPassedToConstructor()
    {
        var vertices = new List<Vector3>
        {
            new(0f, 0f, -100f),
            new(10f, 0f, -100f),
            new(10f, 10f, -100f),
            new(0f, 10f, -100f)
        };
        var expectedEdges = new List<(int, int)>
        {
            (0, 1),
            (1, 2),
            (2, 3),
            (3, 0),
            (0, 2)
        };

        var scene = new WireframeScene(200f, 200f, vertices, expectedEdges);

        Assert.That(scene.Edges, Is.EqualTo(expectedEdges));
        Assert.That(scene.Edges.Count, Is.EqualTo(5));
        for (int i = 0; i < expectedEdges.Count; i++)
        {
            Assert.That(scene.Edges[i].StartIndex, Is.EqualTo(expectedEdges[i].Item1));
            Assert.That(scene.Edges[i].EndIndex, Is.EqualTo(expectedEdges[i].Item2));
        }
    }

    [Test]
    public void Edges_EmptyList_ReturnsEmptyEdges()
    {
        var vertices = new List<Vector3> { new(10f, 10f, -100f) };
        var emptyEdges = new List<(int, int)>();

        var scene = new WireframeScene(200f, 200f, vertices, emptyEdges);

        Assert.That(scene.Edges, Is.Empty);
    }

    #endregion
}
