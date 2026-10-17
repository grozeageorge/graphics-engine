

namespace graphics_engine.Tests;

/// <summary>
/// State-based NUnit test suite for <see cref="Transform"/> verifying translation, 3-axis Euler rotations,
/// scaling, symmetry reflections, strict left-multiplication composition order, and fluent chaining.
/// </summary>
[TestFixture]
public class TransformTests
{
    private const float Tolerance = 1e-4f;
    private Transform _transform = null!;

    [SetUp]
    public void SetUp()
    {
        _transform = new Transform();
    }

    [Test]
    public void InitialMatrix_IsStrictlyIdentity()
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                float expected = (r == c) ? 1.0f : 0.0f;
                Assert.That(_transform.Matrix[r, c], Is.EqualTo(expected).Within(Tolerance));
            }
        }
    }

    #region Translation Tests

    [Test]
    public void Translate_MovesOriginByOffset()
    {
        _transform.Translate(3f, -4f, 5.5f);

        var result = _transform.ApplyToPoint(Vector3.Zero);

        Assert.That(result.X, Is.EqualTo(3f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-4f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(5.5f).Within(Tolerance));
    }

    [Test]
    public void Translate_MovesArbitraryPointByOffset()
    {
        _transform.Translate(2.5f, -1.5f, 4f);

        var pt = new Vector3(1f, 2f, 3f);
        var result = _transform.ApplyToPoint(pt);

        Assert.That(result.X, Is.EqualTo(3.5f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(0.5f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(7.0f).Within(Tolerance));
    }

    #endregion

    #region Rotation Tests (Ox, Oy, Oz Basis Vectors, Stability, and Angles)

    [Test]
    public void RotateOx_90Degrees_VerifiesRightHandOrientationAndAxisStability()
    {
        var ex = new Vector3(1f, 0f, 0f);
        var ey = new Vector3(0f, 1f, 0f);
        var ez = new Vector3(0f, 0f, 1f);

        _transform.RotateOx(90f);

        // Axis stability: rotating around X must not alter the X unit vector
        var resEx = _transform.ApplyToPoint(ex);
        Assert.That(resEx.X, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(resEx.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEx.Z, Is.EqualTo(0f).Within(Tolerance));

        // +90 deg around X: Ey (0, 1, 0) -> (0, 0, 1)
        var resEy = _transform.ApplyToPoint(ey);
        Assert.That(resEy.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEy.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEy.Z, Is.EqualTo(1f).Within(Tolerance));

        // +90 deg around X: Ez (0, 0, 1) -> (0, -1, 0)
        var resEz = _transform.ApplyToPoint(ez);
        Assert.That(resEz.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEz.Y, Is.EqualTo(-1f).Within(Tolerance));
        Assert.That(resEz.Z, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void RotateOx_180Degrees_InvertsPerpendicularAxes()
    {
        _transform.RotateOx(180f);

        var pt = new Vector3(1f, 2f, 3f);
        var result = _transform.ApplyToPoint(pt);

        Assert.That(result.X, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-2f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(-3f).Within(Tolerance));
    }

    [Test]
    public void RotateOy_90Degrees_VerifiesRightHandOrientationAndAxisStability()
    {
        var ex = new Vector3(1f, 0f, 0f);
        var ey = new Vector3(0f, 1f, 0f);
        var ez = new Vector3(0f, 0f, 1f);

        _transform.RotateOy(90f);

        // Axis stability: rotating around Y must not alter the Y unit vector
        var resEy = _transform.ApplyToPoint(ey);
        Assert.That(resEy.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEy.Y, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(resEy.Z, Is.EqualTo(0f).Within(Tolerance));

        // +90 deg around Y: Ez (0, 0, 1) -> (1, 0, 0)
        var resEz = _transform.ApplyToPoint(ez);
        Assert.That(resEz.X, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(resEz.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEz.Z, Is.EqualTo(0f).Within(Tolerance));

        // +90 deg around Y: Ex (1, 0, 0) -> (0, 0, -1)
        var resEx = _transform.ApplyToPoint(ex);
        Assert.That(resEx.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEx.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEx.Z, Is.EqualTo(-1f).Within(Tolerance));
    }

    [Test]
    public void RotateOy_180Degrees_InvertsPerpendicularAxes()
    {
        _transform.RotateOy(180f);

        var pt = new Vector3(1f, 2f, 3f);
        var result = _transform.ApplyToPoint(pt);

        Assert.That(result.X, Is.EqualTo(-1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(2f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(-3f).Within(Tolerance));
    }

    [Test]
    public void RotateOz_90Degrees_VerifiesRightHandOrientationAndAxisStability()
    {
        var ex = new Vector3(1f, 0f, 0f);
        var ey = new Vector3(0f, 1f, 0f);
        var ez = new Vector3(0f, 0f, 1f);

        _transform.RotateOz(90f);

        // Axis stability: rotating around Z must not alter the Z unit vector
        var resEz = _transform.ApplyToPoint(ez);
        Assert.That(resEz.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEz.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEz.Z, Is.EqualTo(1f).Within(Tolerance));

        // +90 deg around Z: Ex (1, 0, 0) -> (0, 1, 0)
        var resEx = _transform.ApplyToPoint(ex);
        Assert.That(resEx.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEx.Y, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(resEx.Z, Is.EqualTo(0f).Within(Tolerance));

        // +90 deg around Z: Ey (0, 1, 0) -> (-1, 0, 0)
        var resEy = _transform.ApplyToPoint(ey);
        Assert.That(resEy.X, Is.EqualTo(-1f).Within(Tolerance));
        Assert.That(resEy.Y, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(resEy.Z, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void RotateOz_180Degrees_InvertsPerpendicularAxes()
    {
        _transform.RotateOz(180f);

        var pt = new Vector3(1f, 2f, 3f);
        var result = _transform.ApplyToPoint(pt);

        Assert.That(result.X, Is.EqualTo(-1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-2f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(3f).Within(Tolerance));
    }

    #endregion

    #region Scaling Tests

    [Test]
    public void Scale_Uniform_MultipliesAllComponentsEqually()
    {
        _transform.Scale(2.5f, 2.5f, 2.5f);

        var pt = new Vector3(2f, -3f, 4f);
        var result = _transform.ApplyToPoint(pt);

        Assert.That(result.X, Is.EqualTo(5.0f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-7.5f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(10.0f).Within(Tolerance));
    }

    [Test]
    public void Scale_NonUniform_ScalesCoordinatesIndependently()
    {
        _transform.Scale(2f, 3f, -4f);

        var pt = new Vector3(1.5f, 2.0f, 3.0f);
        var result = _transform.ApplyToPoint(pt);

        Assert.That(result.X, Is.EqualTo(3.0f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(6.0f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(-12.0f).Within(Tolerance));
    }

    #endregion

    #region Symmetry Operations Tests

    [Test]
    public void SymmetryOrigin_InvertsAllCoordinates()
    {
        _transform.SymmetryOrigin();

        var result = _transform.ApplyToPoint(new Vector3(3f, -4f, 5f));

        Assert.That(result.X, Is.EqualTo(-3f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(4f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(-5f).Within(Tolerance));
    }

    [Test]
    public void SymmetryOx_InvertsYAndZCoordinates()
    {
        _transform.SymmetryOx();

        var result = _transform.ApplyToPoint(new Vector3(1f, 2f, 3f));

        Assert.That(result.X, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-2f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(-3f).Within(Tolerance));
    }

    [Test]
    public void SymmetryOy_InvertsXAndZCoordinates()
    {
        _transform.SymmetryOy();

        var result = _transform.ApplyToPoint(new Vector3(1f, 2f, 3f));

        Assert.That(result.X, Is.EqualTo(-1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(2f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(-3f).Within(Tolerance));
    }

    [Test]
    public void SymmetryOz_InvertsXAndYCoordinates()
    {
        _transform.SymmetryOz();

        var result = _transform.ApplyToPoint(new Vector3(1f, 2f, 3f));

        Assert.That(result.X, Is.EqualTo(-1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-2f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(3f).Within(Tolerance));
    }

    [Test]
    public void SymmetryXoy_InvertsZCoordinate()
    {
        _transform.SymmetryXoy();

        var result = _transform.ApplyToPoint(new Vector3(2f, 4f, 6f));

        Assert.That(result.X, Is.EqualTo(2f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(4f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(-6f).Within(Tolerance));
    }

    [Test]
    public void SymmetryYoz_InvertsXCoordinate()
    {
        _transform.SymmetryYoz();

        var result = _transform.ApplyToPoint(new Vector3(2f, 4f, 6f));

        Assert.That(result.X, Is.EqualTo(-2f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(4f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(6f).Within(Tolerance));
    }

    [Test]
    public void SymmetryXoz_InvertsYCoordinate()
    {
        _transform.SymmetryXoz();

        var result = _transform.ApplyToPoint(new Vector3(2f, 4f, 6f));

        Assert.That(result.X, Is.EqualTo(2f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-4f).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(6f).Within(Tolerance));
    }

    #endregion

    #region Accumulation & Composition Order Tests

    [Test]
    public void CompositionOrder_StrictLeftMultiplication_YieldsExpectedPoint()
    {
        // Pipeline specification:
        // Translate(2, 3, 0) followed by RotateOz(90)
        // With left-multiplication: T_accum = R * T
        // Applied to point P = (1, 0, 0):
        // P' = R * (T * P)
        // Step 1: T * (1, 0, 0, 1)^T = (3, 3, 0, 1)^T
        // Step 2: R_z(90) * (3, 3, 0, 1)^T = (-3, 3, 0, 1)^T
        _transform.Translate(2f, 3f, 0f)
                  .RotateOz(90f);

        var result = _transform.ApplyToPoint(new Vector3(1f, 0f, 0f));

        Assert.That(result.X, Is.EqualTo(-3f).Within(Tolerance), "Strict left-multiplication X mismatch.");
        Assert.That(result.Y, Is.EqualTo(3f).Within(Tolerance), "Strict left-multiplication Y mismatch.");
        Assert.That(result.Z, Is.EqualTo(0f).Within(Tolerance), "Strict left-multiplication Z mismatch.");
    }

    [Test]
    public void Chaining_AllMethodsReturnSameUpdatedInstance()
    {
        Assert.That(_transform.Translate(1, 1, 1), Is.SameAs(_transform));
        Assert.That(_transform.RotateOx(45), Is.SameAs(_transform));
        Assert.That(_transform.RotateOy(45), Is.SameAs(_transform));
        Assert.That(_transform.RotateOz(45), Is.SameAs(_transform));
        Assert.That(_transform.Scale(2, 2, 2), Is.SameAs(_transform));
        Assert.That(_transform.SymmetryOrigin(), Is.SameAs(_transform));
        Assert.That(_transform.SymmetryOx(), Is.SameAs(_transform));
        Assert.That(_transform.SymmetryOy(), Is.SameAs(_transform));
        Assert.That(_transform.SymmetryOz(), Is.SameAs(_transform));
        Assert.That(_transform.SymmetryXoy(), Is.SameAs(_transform));
        Assert.That(_transform.SymmetryYoz(), Is.SameAs(_transform));
        Assert.That(_transform.SymmetryXoz(), Is.SameAs(_transform));
    }

    #endregion
}
