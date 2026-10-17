

namespace graphics_engine.Tests;

/// <summary>
/// State-based NUnit test suite for <see cref="Vector3"/> testing addition, scalar multiplication (both orders),
/// zero vector properties, conversions, and formatting.
/// </summary>
[TestFixture]
public class Vector3Tests
{
    private const float Tolerance = 1e-4f;

    [Test]
    public void Constructor_WithComponents_InitializesCorrectly()
    {
        var v = new Vector3(1.5f, -2.5f, 3.25f);

        Assert.That(v.X, Is.EqualTo(1.5f).Within(Tolerance));
        Assert.That(v.Y, Is.EqualTo(-2.5f).Within(Tolerance));
        Assert.That(v.Z, Is.EqualTo(3.25f).Within(Tolerance));
    }

    [Test]
    public void Constructor_FromVector4_ExtractsXyzAndDiscardsW()
    {
        var v4 = new Vector4(10f, 20f, 30f, 40f);
        var v3 = new Vector3(v4);

        Assert.That(v3.X, Is.EqualTo(10f).Within(Tolerance));
        Assert.That(v3.Y, Is.EqualTo(20f).Within(Tolerance));
        Assert.That(v3.Z, Is.EqualTo(30f).Within(Tolerance));
    }

    [Test]
    public void ZeroProperty_ReturnsAllZeros()
    {
        var zero = Vector3.Zero;

        Assert.That(zero.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(zero.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(zero.Z, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void ZeroProperty_AdditionIdentity_PreservesVector()
    {
        var v = new Vector3(3f, -4f, 5f);
        var result = v + Vector3.Zero;

        Assert.That(result.X, Is.EqualTo(3f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-4f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(5f).Within(Tolerance));
    }

    [Test]
    [TestCase(1f, 2f, 3f, 4f, 5f, 6f, 5f, 7f, 9f)]
    [TestCase(-1f, 2.5f, -3f, 1f, -2.5f, 3f, 0f, 0f, 0f)]
    public void Addition_ComponentWise_YieldsExpectedSum(
        float ax, float ay, float az,
        float bx, float by, float bz,
        float ex, float ey, float ez)
    {
        var a = new Vector3(ax, ay, az);
        var b = new Vector3(bx, by, bz);

        var sum = a + b;

        Assert.That(sum.X, Is.EqualTo(ex).Within(Tolerance));
        Assert.That(sum.Y, Is.EqualTo(ey).Within(Tolerance));
        Assert.That(sum.Z, Is.EqualTo(ez).Within(Tolerance));
    }

    [Test]
    [TestCase(2f, -3f, 4.5f, 2f, 4f, -6f, 9f)]
    [TestCase(1f, 2f, 3f, 0f, 0f, 0f, 0f)]
    [TestCase(1f, 2f, 3f, -1f, -1f, -2f, -3f)]
    public void ScalarMultiplication_VectorTimesScalar_ScalesComponents(
        float x, float y, float z, float scalar,
        float ex, float ey, float ez)
    {
        var v = new Vector3(x, y, z);
        var result = v * scalar;

        Assert.That(result.X, Is.EqualTo(ex).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(ey).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(ez).Within(Tolerance));
    }

    [Test]
    [TestCase(2f, -3f, 4.5f, 2f, 4f, -6f, 9f)]
    [TestCase(1f, 2f, 3f, 0f, 0f, 0f, 0f)]
    [TestCase(1f, 2f, 3f, -1f, -1f, -2f, -3f)]
    public void ScalarMultiplication_ScalarTimesVector_IsCommutativeAndScalesComponents(
        float x, float y, float z, float scalar,
        float ex, float ey, float ez)
    {
        var v = new Vector3(x, y, z);
        var result = scalar * v;

        Assert.That(result.X, Is.EqualTo(ex).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(ey).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(ez).Within(Tolerance));
    }

    [Test]
    public void ToString_FormatsWithFourDecimals()
    {
        var v = new Vector3(1.23456f, -2.34567f, 3.0f);
        Assert.That(v.ToString(), Is.EqualTo("(1.2346, -2.3457, 3.0000)"));
    }
}
