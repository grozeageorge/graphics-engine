
namespace graphics_engine.Tests;

/// <summary>
/// State-based NUnit test suite for <see cref="Vector4"/> testing addition, scalar multiplication (both orders),
/// zero vector properties, conversions between <see cref="Vector3"/> and <see cref="Vector4"/>, and formatting.
/// </summary>
[TestFixture]
public class Vector4Tests
{
    private const float Tolerance = 1e-4f;

    [Test]
    public void Constructor_WithFourComponents_InitializesCorrectly()
    {
        var v = new Vector4(1f, 2f, 3f, 4f);

        Assert.That(v.X, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(v.Y, Is.EqualTo(2f).Within(Tolerance));
        Assert.That(v.Z, Is.EqualTo(3f).Within(Tolerance));
        Assert.That(v.W, Is.EqualTo(4f).Within(Tolerance));
    }

    [Test]
    public void Constructor_FromVector3_DefaultsWToOne()
    {
        var v3 = new Vector3(10f, 20f, 30f);
        var v4 = new Vector4(v3);

        Assert.That(v4.X, Is.EqualTo(10f).Within(Tolerance));
        Assert.That(v4.Y, Is.EqualTo(20f).Within(Tolerance));
        Assert.That(v4.Z, Is.EqualTo(30f).Within(Tolerance));
        Assert.That(v4.W, Is.EqualTo(1.0f).Within(Tolerance));
    }

    [Test]
    [TestCase(1f, 2f, 3f, 0f)]
    [TestCase(4f, -5f, 6f, 2.5f)]
    [TestCase(0f, 0f, 0f, -1.0f)]
    public void Constructor_FromVector3WithCustomW_SetsAllComponents(float x, float y, float z, float w)
    {
        var v3 = new Vector3(x, y, z);
        var v4 = new Vector4(v3, w);

        Assert.That(v4.X, Is.EqualTo(x).Within(Tolerance));
        Assert.That(v4.Y, Is.EqualTo(y).Within(Tolerance));
        Assert.That(v4.Z, Is.EqualTo(z).Within(Tolerance));
        Assert.That(v4.W, Is.EqualTo(w).Within(Tolerance));
    }

    [Test]
    public void ToVector3_ExtractsXyzAndDiscardsW()
    {
        var v4 = new Vector4(12f, -15f, 18f, 1.0f);
        var v3 = v4.ToVector3();

        Assert.That(v3.X, Is.EqualTo(12f).Within(Tolerance));
        Assert.That(v3.Y, Is.EqualTo(-15f).Within(Tolerance));
        Assert.That(v3.Z, Is.EqualTo(18f).Within(Tolerance));
    }

    [Test]
    public void ZeroProperty_ReturnsAllZeros()
    {
        var zero = Vector4.Zero;

        Assert.That(zero.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(zero.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(zero.Z, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(zero.W, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void ZeroProperty_AdditionIdentity_PreservesVector()
    {
        var v = new Vector4(1f, -2f, 3f, 4f);
        var result = v + Vector4.Zero;

        Assert.That(result.X, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-2f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(3f).Within(Tolerance));
        Assert.That(result.W, Is.EqualTo(4f).Within(Tolerance));
    }

    [Test]
    public void Addition_ComponentWise_AddsAllFourComponents()
    {
        var a = new Vector4(1f, 2f, 3f, 4f);
        var b = new Vector4(10f, 20f, 30f, 40f);

        var sum = a + b;

        Assert.That(sum.X, Is.EqualTo(11f).Within(Tolerance));
        Assert.That(sum.Y, Is.EqualTo(22f).Within(Tolerance));
        Assert.That(sum.Z, Is.EqualTo(33f).Within(Tolerance));
        Assert.That(sum.W, Is.EqualTo(44f).Within(Tolerance));
    }

    [Test]
    [TestCase(1f, -2f, 3f, -4f, 3f, 3f, -6f, 9f, -12f)]
    [TestCase(5f, 5f, 5f, 5f, 0f, 0f, 0f, 0f, 0f)]
    [TestCase(2f, 4f, 6f, 8f, 0.5f, 1f, 2f, 3f, 4f)]
    public void ScalarMultiplication_VectorTimesScalar_ScalesAllComponents(
        float x, float y, float z, float w, float scalar,
        float ex, float ey, float ez, float ew)
    {
        var v = new Vector4(x, y, z, w);
        var result = v * scalar;

        Assert.That(result.X, Is.EqualTo(ex).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(ey).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(ez).Within(Tolerance));
        Assert.That(result.W, Is.EqualTo(ew).Within(Tolerance));
    }

    [Test]
    [TestCase(1f, -2f, 3f, -4f, 3f, 3f, -6f, 9f, -12f)]
    [TestCase(5f, 5f, 5f, 5f, 0f, 0f, 0f, 0f, 0f)]
    [TestCase(2f, 4f, 6f, 8f, 0.5f, 1f, 2f, 3f, 4f)]
    public void ScalarMultiplication_ScalarTimesVector_IsCommutativeAndScalesAllComponents(
        float x, float y, float z, float w, float scalar,
        float ex, float ey, float ez, float ew)
    {
        var v = new Vector4(x, y, z, w);
        var result = scalar * v;

        Assert.That(result.X, Is.EqualTo(ex).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(ey).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(ez).Within(Tolerance));
        Assert.That(result.W, Is.EqualTo(ew).Within(Tolerance));
    }

    [Test]
    public void ToString_FormatsWithFourDecimals()
    {
        var v = new Vector4(1.12345f, 2.0f, -3.5f, 0.0001f);
        Assert.That(v.ToString(), Is.EqualTo("(1.1235, 2.0000, -3.5000, 0.0001)"));
    }
}
