namespace graphics_engine;

/// <summary>
/// Represents a 3D wireframe object defined by an indexed vertex buffer and edge connections,
/// supporting affine transformations around its center of gravity and perspective projection.
/// </summary>
public class WireframeScene
{
    /// <summary>
    /// The position of the viewer's pupil (center of projection) in front of the screen.
    /// </summary>
    private readonly Vector3 _viewerPosition;

    private readonly Transform _transform = new();
    private readonly List<Vector3> _vertices;

    /// <summary>
    /// Gets the list of edge index pairs that connect vertices in the wireframe mesh.
    /// </summary>
    public List<(int StartIndex, int EndIndex)> Edges { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WireframeScene"/> class with the specified geometry.
    /// </summary>
    /// <param name="screenWidth">The width of the graphical display area in pixels (L).</param>
    /// <param name="screenHeight">The height of the graphical display area in pixels (H).</param>
    /// <param name="initialVertices">The distinct 3D vertices of the wireframe model.</param>
    /// <param name="initialEdges">Pairs of zero-based indices into <paramref name="initialVertices"/> defining the line segments.</param>
    public WireframeScene(float screenWidth, float screenHeight, List<Vector3> initialVertices, List<(int, int)> initialEdges)
    {
        _viewerPosition = new(screenWidth / 2f, screenHeight / 2f, 800f);
        _vertices = initialVertices;
        Edges = initialEdges;
    }

    /// <summary>
    /// Projects a single 3D point in world space onto the 2D screen plane (z = 0) using perspective projection,
    /// inverting the Y-axis to match standard top-left screen coordinates.
    /// </summary>
    /// <param name="point">The 3D point to project (expected to have z &lt; 0 behind the screen).</param>
    /// <returns>A <see cref="Vector3"/> containing the projected 2D screen coordinates (X', Y', 0).</returns>
    public Vector3 ProjectPointPerspective(Vector3 point)
    {
        var depthRatio = _viewerPosition.Z / (point.Z - _viewerPosition.Z);
        var xProjected = _viewerPosition.X - depthRatio * (point.X - _viewerPosition.X);
        var yProjected = _viewerPosition.Y + depthRatio * (point.Y - _viewerPosition.Y);
        
        return new Vector3(xProjected, yProjected, 0f);
    }

    /// <summary>
    /// Applies the accumulated 3D transformations to all scene vertices and computes their 2D perspective projections.
    /// </summary>
    /// <returns>A list of projected 2D coordinates corresponding to each vertex in the scene.</returns>
    public List<Vector3> ProjectScenePerspective()
    {
        var projectedVertices = new List<Vector3>(_vertices.Count);

        foreach (var vertex in _vertices)
        {
            var worldPoint = _transform.ApplyToPoint(vertex);
            projectedVertices.Add(ProjectPointPerspective(worldPoint));
        }

        return projectedVertices;
    }

    /// <summary>
    /// Computes the current center of gravity (centroid) of the transformed 3D vertices.
    /// </summary>
    /// <returns>The average 3D position of all vertices in current world space.</returns>
    private Vector3 GetCenterOfGravity()
    {
        if (_vertices.Count == 0)
        {
            return Vector3.Zero;
        }

        var sum = Vector3.Zero;
        foreach (var vertex in _vertices)
        {
            sum += _transform.ApplyToPoint(vertex);
        }

        return sum * (1f / _vertices.Count);
    }

    /// <summary>
    /// Translates the wireframe object in 3D space by the specified offsets.
    /// </summary>
    /// <param name="dx">Translation offset along the X-axis.</param>
    /// <param name="dy">Translation offset along the Y-axis.</param>
    /// <param name="dz">Translation offset along the Z-axis.</param>
    public void Translate(float dx, float dy, float dz)
    {
        _transform.Translate(dx, dy, dz);
    }

    /// <summary>
    /// Rotates the wireframe object counter-clockwise around the Z-axis relative to its center of gravity.
    /// </summary>
    /// <param name="alpha">Rotation angle in degrees.</param>
    public void RotateOz(float alpha)
    {
        var centroid = GetCenterOfGravity();

        _transform.Translate(-centroid.X, -centroid.Y, -centroid.Z)
            .RotateOz(alpha)
            .Translate(centroid.X, centroid.Y, centroid.Z);
    }

    /// <summary>
    /// Rotates the wireframe object around the X-axis relative to its center of gravity.
    /// </summary>
    /// <param name="beta">Rotation angle in degrees.</param>
    public void RotateOx(float beta)
    {
        var centroid = GetCenterOfGravity();

        _transform.Translate(-centroid.X, -centroid.Y, -centroid.Z)
            .RotateOx(beta)
            .Translate(centroid.X, centroid.Y, centroid.Z);
    }

    /// <summary>
    /// Rotates the wireframe object around the Y-axis relative to its center of gravity.
    /// </summary>
    /// <param name="gamma">Rotation angle in degrees.</param>
    public void RotateOy(float gamma)
    {
        var centroid = GetCenterOfGravity();

        _transform.Translate(-centroid.X, -centroid.Y, -centroid.Z)
            .RotateOy(gamma)
            .Translate(centroid.X, centroid.Y, centroid.Z);
    }

    /// <summary>
    /// Scales the wireframe object along the X, Y, and Z axes relative to its center of gravity.
    /// </summary>
    /// <param name="sx">Scale factor along the X-axis.</param>
    /// <param name="sy">Scale factor along the Y-axis.</param>
    /// <param name="sz">Scale factor along the Z-axis.</param>
    public void Scale(float sx, float sy, float sz)
    {
        var centroid = GetCenterOfGravity();

        _transform.Translate(-centroid.X, -centroid.Y, -centroid.Z)
            .Scale(sx, sy, sz)
            .Translate(centroid.X, centroid.Y, centroid.Z);
    }
}