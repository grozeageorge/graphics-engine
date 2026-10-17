

namespace graphics_engine.Tests;

/// <summary>
/// State-based NUnit test suite for <see cref="Matrix4x4"/> covering construction, identity preservation,
/// 2D indexer read/write, boundary/exception validation, matrix-matrix multiplication, and column-vector transformation.
/// </summary>
[TestFixture]
public class Matrix4x4Tests
{
    private const float Tolerance = 1e-4f;

    [Test]
    public void Constructor_ExplicitComponents_SetsAll16CellsCorrectly()
    {
        var m = new Matrix4x4(
            1f, 2f, 3f, 4f,
            5f, 6f, 7f, 8f,
            9f, 10f, 11f, 12f,
            13f, 14f, 15f, 16f
        );

        Assert.That(m.M00, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(m.M01, Is.EqualTo(2f).Within(Tolerance));
        Assert.That(m.M02, Is.EqualTo(3f).Within(Tolerance));
        Assert.That(m.M03, Is.EqualTo(4f).Within(Tolerance));
        Assert.That(m.M10, Is.EqualTo(5f).Within(Tolerance));
        Assert.That(m.M11, Is.EqualTo(6f).Within(Tolerance));
        Assert.That(m.M12, Is.EqualTo(7f).Within(Tolerance));
        Assert.That(m.M13, Is.EqualTo(8f).Within(Tolerance));
        Assert.That(m.M20, Is.EqualTo(9f).Within(Tolerance));
        Assert.That(m.M21, Is.EqualTo(10f).Within(Tolerance));
        Assert.That(m.M22, Is.EqualTo(11f).Within(Tolerance));
        Assert.That(m.M23, Is.EqualTo(12f).Within(Tolerance));
        Assert.That(m.M30, Is.EqualTo(13f).Within(Tolerance));
        Assert.That(m.M31, Is.EqualTo(14f).Within(Tolerance));
        Assert.That(m.M32, Is.EqualTo(15f).Within(Tolerance));
        Assert.That(m.M33, Is.EqualTo(16f).Within(Tolerance));
    }

    [Test]
    public void Identity_HasOnesOnDiagonalAndZerosElsewhere()
    {
        var id = Matrix4x4.Identity;

        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                float expected = (r == c) ? 1.0f : 0.0f;
                Assert.That(id[r, c], Is.EqualTo(expected).Within(Tolerance), $"Identity element at [{r},{c}] invalid.");
            }
        }
    }

    [Test]
    public void Identity_Multiplication_PreservesOtherMatrices()
    {
        var m = new Matrix4x4(
            2f, -1f, 0f, 4f,
            3f, 5f, -2f, 1f,
            0f, 1f, 7f, -3f,
            4f, 2f, 1f, 8f);

        var rightMult = m * Matrix4x4.Identity;
        var leftMult = Matrix4x4.Identity * m;

        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                Assert.That(rightMult[r, c], Is.EqualTo(m[r, c]).Within(Tolerance), $"M * I failed at [{r},{c}].");
                Assert.That(leftMult[r, c], Is.EqualTo(m[r, c]).Within(Tolerance), $"I * M failed at [{r},{c}].");
            }
        }
    }

    [Test]
    public void Identity_Multiplication_PreservesVector()
    {
        var v = new Vector4(2.5f, -7.1f, 8.3f, 1.0f);
        var result = Matrix4x4.Identity * v;

        Assert.That(result.X, Is.EqualTo(v.X).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(v.Y).Within(Tolerance));
        Assert.That(result.Z, Is.EqualTo(v.Z).Within(Tolerance));
        Assert.That(result.W, Is.EqualTo(v.W).Within(Tolerance));
    }

    [Test]
    public void Indexer_ReadWrite_UpdatesAndReturnsAccurateValuesForAllValidCells()
    {
        var m = new Matrix4x4();

        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < 4; c++)
            {
                float testVal = r * 10f + c + 0.75f;
                m[r, c] = testVal;
                Assert.That(m[r, c], Is.EqualTo(testVal).Within(Tolerance), $"Cell [{r},{c}] read/write mismatch.");
            }
        }
    }

    [Test]
    [TestCase(-1, 0, TestName = "IndexerGet_NegativeRow")]
    [TestCase(4, 0, TestName = "IndexerGet_RowExceedingMax")]
    [TestCase(0, -1, TestName = "IndexerGet_NegativeCol")]
    [TestCase(0, 4, TestName = "IndexerGet_ColExceedingMax")]
    [TestCase(-5, -5, TestName = "IndexerGet_BothNegative")]
    [TestCase(10, 10, TestName = "IndexerGet_BothExceedingMax")]
    public void IndexerGet_OutOfBounds_ThrowsIndexOutOfRangeException(int row, int col)
    {
        var m = Matrix4x4.Identity;

        Assert.Throws<IndexOutOfRangeException>(() =>
        {
            _ = m[row, col];
        });
    }

    [Test]
    [TestCase(-1, 0, TestName = "IndexerSet_NegativeRow")]
    [TestCase(4, 0, TestName = "IndexerSet_RowExceedingMax")]
    [TestCase(0, -1, TestName = "IndexerSet_NegativeCol")]
    [TestCase(0, 4, TestName = "IndexerSet_ColExceedingMax")]
    [TestCase(99, 99, TestName = "IndexerSet_BothExceedingMax")]
    public void IndexerSet_OutOfBounds_ThrowsIndexOutOfRangeException(int row, int col)
    {
        var m = Matrix4x4.Identity;

        var ex = Assert.Throws<IndexOutOfRangeException>(() =>
        {
            m[row, col] = 1.0f;
        });

        Assert.That(ex!.Message, Does.Contain("Indices must be between 0 and 3"));
    }

    [Test]
    public void MatrixMatrixMultiplication_KnownInputs_YieldsCorrectProduct()
    {
        // Known matrix A
        var a = new Matrix4x4(
            1f, 2f, 0f, 0f,
            0f, 1f, 1f, 0f,
            2f, 0f, 1f, 3f,
            0f, 0f, 0f, 1f);

        // Known matrix B
        var b = new Matrix4x4(
            2f, 0f, 1f, 4f,
            1f, 3f, 0f, 1f,
            0f, 2f, 1f, 0f,
            1f, 1f, 0f, 2f);

        var product = a * b;

        // Row 0: [1*2 + 2*1, 1*0 + 2*3, 1*1 + 2*0, 1*4 + 2*1] = [4, 6, 1, 6]
        Assert.That(product[0, 0], Is.EqualTo(4f).Within(Tolerance));
        Assert.That(product[0, 1], Is.EqualTo(6f).Within(Tolerance));
        Assert.That(product[0, 2], Is.EqualTo(1f).Within(Tolerance));
        Assert.That(product[0, 3], Is.EqualTo(6f).Within(Tolerance));

        // Row 1: [0*2 + 1*1 + 1*0, 0 + 1*3 + 1*2, 0 + 1*0 + 1*1, 0 + 1*1 + 0] = [1, 5, 1, 1]
        Assert.That(product[1, 0], Is.EqualTo(1f).Within(Tolerance));
        Assert.That(product[1, 1], Is.EqualTo(5f).Within(Tolerance));
        Assert.That(product[1, 2], Is.EqualTo(1f).Within(Tolerance));
        Assert.That(product[1, 3], Is.EqualTo(1f).Within(Tolerance));

        // Row 2: [2*2 + 0 + 0 + 3*1, 0 + 0 + 1*2 + 3*1, 2*1 + 0 + 1*1 + 0, 2*4 + 0 + 0 + 3*2] = [7, 5, 3, 14]
        Assert.That(product[2, 0], Is.EqualTo(7f).Within(Tolerance));
        Assert.That(product[2, 1], Is.EqualTo(5f).Within(Tolerance));
        Assert.That(product[2, 2], Is.EqualTo(3f).Within(Tolerance));
        Assert.That(product[2, 3], Is.EqualTo(14f).Within(Tolerance));

        // Row 3: [1, 1, 0, 2]
        Assert.That(product[3, 0], Is.EqualTo(1f).Within(Tolerance));
        Assert.That(product[3, 1], Is.EqualTo(1f).Within(Tolerance));
        Assert.That(product[3, 2], Is.EqualTo(0f).Within(Tolerance));
        Assert.That(product[3, 3], Is.EqualTo(2f).Within(Tolerance));
    }

    [Test]
    public void MatrixVectorMultiplication_ColumnVectorTransformation_YieldsCorrectVector()
    {
        // Matrix M representing a scale and translation:
        // X' = 2*X + 5*W
        // Y' = 3*Y - 4*W
        // Z' = 4*Z + 2*W
        // W' = 1*W
        var m = new Matrix4x4(
            2f, 0f, 0f, 5f,
            0f, 3f, 0f, -4f,
            0f, 0f, 4f, 2f,
            0f, 0f, 0f, 1f);

        var p = new Vector4(1f, 2f, 3f, 1f);
        var pPrime = m * p;

        Assert.That(pPrime.X, Is.EqualTo(7f).Within(Tolerance));
        Assert.That(pPrime.Y, Is.EqualTo(2f).Within(Tolerance));
        Assert.That(pPrime.Z, Is.EqualTo(14f).Within(Tolerance));
        Assert.That(pPrime.W, Is.EqualTo(1f).Within(Tolerance));
    }

    [Test]
    public void ToString_FormatsWithFourAlignedRows()
    {
        var m = Matrix4x4.Identity;
        var s = m.ToString();

        Assert.That(s, Does.Contain("[  1.0000,   0.0000,   0.0000,   0.0000]"));
        var lines = s.Split('\n');
        Assert.That(lines.Length, Is.EqualTo(4));
    }
}
