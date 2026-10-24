namespace graphics_engine.Tests;

[TestFixture]
public class MeshFactoryTests
{
    private const float Tolerance = 1e-4f;

    [Test]
    public void CreateCube_ReturnsEightDistinctVerticesAndTwelveEdges()
    {
        float cx = 100f;
        float cy = 200f;
        float cz = -400f;
        float size = 180f;

        var (vertices, edges) = MeshFactory.CreateCube(cx, cy, cz, size);

        // 1. Assert exactly 8 vertices and 12 edges
        Assert.That(vertices.Count, Is.EqualTo(8));
        Assert.That(edges.Count, Is.EqualTo(12));

        // 2. Assert all 8 vertices are distinct
        var distinctCount = vertices.Distinct().Count();
        Assert.That(distinctCount, Is.EqualTo(8));

        // 3. Assert centroid matches (cx, cy, cz)
        var sumX = vertices.Sum(v => v.X);
        var sumY = vertices.Sum(v => v.Y);
        var sumZ = vertices.Sum(v => v.Z);

        Assert.That(sumX / 8f, Is.EqualTo(cx).Within(Tolerance));
        Assert.That(sumY / 8f, Is.EqualTo(cy).Within(Tolerance));
        Assert.That(sumZ / 8f, Is.EqualTo(cz).Within(Tolerance));

        // 4. Assert edge valid index ranges and non-degeneracy
        foreach (var (start, end) in edges)
        {
            Assert.That(start, Is.InRange(0, 7));
            Assert.That(end, Is.InRange(0, 7));
            Assert.That(start, Is.Not.EqualTo(end));
        }

        // 5. Assert bounding box extents
        float half = size / 2f;
        Assert.That(vertices.Min(v => v.X), Is.EqualTo(cx - half).Within(Tolerance));
        Assert.That(vertices.Max(v => v.X), Is.EqualTo(cx + half).Within(Tolerance));
        Assert.That(vertices.Min(v => v.Y), Is.EqualTo(cy - half).Within(Tolerance));
        Assert.That(vertices.Max(v => v.Y), Is.EqualTo(cy + half).Within(Tolerance));
        Assert.That(vertices.Min(v => v.Z), Is.EqualTo(cz - half).Within(Tolerance));
        Assert.That(vertices.Max(v => v.Z), Is.EqualTo(cz + half).Within(Tolerance));
    }
}
