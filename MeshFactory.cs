namespace graphics_engine;

/// <summary>
/// Provides factory methods for generating standard 3D wireframe mesh models.
/// </summary>
public static class MeshFactory
{
    /// <summary>
    /// Generates a 3D wireframe cube centered at the specified coordinates with the given edge length.
    /// </summary>
    /// <param name="centerX">The X-coordinate of the cube's center.</param>
    /// <param name="centerY">The Y-coordinate of the cube's center.</param>
    /// <param name="centerZ">The Z-coordinate of the cube's center in world space (typically negative, behind the screen plane).</param>
    /// <param name="size">The edge length of the cube.</param>
    /// <returns>
    /// A tuple containing:
    /// <list type="bullet">
    ///   <item><description><c>Vertices</c>: 8 distinct <see cref="Vector3"/> corner vertices.</description></item>
    ///   <item><description><c>Edges</c>: 12 index pairs defining the wireframe edges of the cube.</description></item>
    /// </list>
    /// </returns>
    public static (List<Vector3> Vertices, List<(int StartIndex, int EndIndex)> Edges) CreateCube(
        float centerX,
        float centerY,
        float centerZ,
        float size)
    {
        var half = size / 2f;
        var minX = centerX - half;
        var maxX = centerX + half;
        var minY = centerY - half;
        var maxY = centerY + half;
        var minZ = centerZ - half;
        var maxZ = centerZ + half;

        var vertices = new List<Vector3>(8)
        {
            // Front face (minZ)
            new(minX, minY, minZ), // 0
            new(maxX, minY, minZ), // 1
            new(maxX, maxY, minZ), // 2
            new(minX, maxY, minZ), // 3

            // Back face (maxZ)
            new(minX, minY, maxZ), // 4
            new(maxX, minY, maxZ), // 5
            new(maxX, maxY, maxZ), // 6
            new(minX, maxY, maxZ), // 7
        };

        var edges = new List<(int StartIndex, int EndIndex)>(12)
        {
            // Front face loop
            (0, 1),
            (1, 2),
            (2, 3),
            (3, 0),

            // Back face loop
            (4, 5),
            (5, 6),
            (6, 7),
            (7, 4),

            // Connecting edges between front and back faces
            (0, 4),
            (1, 5),
            (2, 6),
            (3, 7),
        };

        return (vertices, edges);
    }
}
