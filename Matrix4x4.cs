namespace graphics_engine;

/// <summary>
/// Represents a 4x4 matrix with single-precision floating point elements in row-major order.
/// </summary>
/// <remarks>
/// Elements are named using <c>M[row][column]</c> convention from <c>M00</c> to <c>M33</c>.
/// Elements are stored in row-major order (M[row][column]), designed for 
/// column-vector left-multiplication (P' = M * P).
/// </remarks>
public struct Matrix4x4(
    float m00, float m01, float m02, float m03,
    float m10, float m11, float m12, float m13,
    float m20, float m21, float m22, float m23,
    float m30, float m31, float m32, float m33)
{
    /// <summary>Value at row 0, column 0.</summary>
    public float M00 = m00;
    /// <summary>Value at row 0, column 1.</summary>
    public float M01 = m01;
    /// <summary>Value at row 0, column 2.</summary>
    public float M02 = m02;
    /// <summary>Value at row 0, column 3.</summary>
    public float M03 = m03;

    /// <summary>Value at row 1, column 0.</summary>
    public float M10 = m10;
    /// <summary>Value at row 1, column 1.</summary>
    public float M11 = m11;
    /// <summary>Value at row 1, column 2.</summary>
    public float M12 = m12;
    /// <summary>Value at row 1, column 3.</summary>
    public float M13 = m13;

    /// <summary>Value at row 2, column 0.</summary>
    public float M20 = m20;
    /// <summary>Value at row 2, column 1.</summary>
    public float M21 = m21;
    /// <summary>Value at row 2, column 2.</summary>
    public float M22 = m22;
    /// <summary>Value at row 2, column 3.</summary>
    public float M23 = m23;

    /// <summary>Value at row 3, column 0.</summary>
    public float M30 = m30;
    /// <summary>Value at row 3, column 1.</summary>
    public float M31 = m31;
    /// <summary>Value at row 3, column 2.</summary>
    public float M32 = m32;
    /// <summary>Value at row 3, column 3.</summary>
    public float M33 = m33;

    /// <summary>
    /// Gets the 4x4 identity matrix with 1s along the main diagonal and 0s elsewhere.
    /// </summary>
    public static Matrix4x4 Identity => new(
        1f, 0f, 0f, 0f,
        0f, 1f, 0f, 0f,
        0f, 0f, 1f, 0f,
        0f, 0f, 0f, 1f);

    /// <summary>
    /// Gets or sets the matrix element at the specified zero-based row and column.
    /// </summary>
    /// <param name="row">The zero-based row index (0 to 3).</param>
    /// <param name="col">The zero-based column index (0 to 3).</param>
    /// <returns>The floating-point value stored at the given cell.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="row"/> or <paramref name="col"/> is not between 0 and 3.</exception>
    public float this[int row, int col]
    {
        get => (row, col) switch
        {
            (0, 0) => M00,
            (0, 1) => M01,
            (0, 2) => M02,
            (0, 3) => M03,
            (1, 0) => M10,
            (1, 1) => M11,
            (1, 2) => M12,
            (1, 3) => M13,
            (2, 0) => M20,
            (2, 1) => M21,
            (2, 2) => M22,
            (2, 3) => M23,
            (3, 0) => M30,
            (3, 1) => M31,
            (3, 2) => M32,
            (3, 3) => M33,
            _ => throw new IndexOutOfRangeException("Indices must be between 0 and 3")
        };
        set
        {
            switch (row, col)
            {
                case (0, 0): M00 = value; break;
                case (0, 1): M01 = value; break;
                case (0, 2): M02 = value; break;
                case (0, 3): M03 = value; break;
                case (1, 0): M10 = value; break;
                case (1, 1): M11 = value; break;
                case (1, 2): M12 = value; break;
                case (1, 3): M13 = value; break;
                case (2, 0): M20 = value; break;
                case (2, 1): M21 = value; break;
                case (2, 2): M22 = value; break;
                case (2, 3): M23 = value; break;
                case (3, 0): M30 = value; break;
                case (3, 1): M31 = value; break;
                case (3, 2): M32 = value; break;
                case (3, 3): M33 = value; break;
                default: throw new IndexOutOfRangeException("Indices must be between 0 and 3");
            }
        }
    }
    
    /// <summary>
    /// Computes the matrix multiplication of two 4x4 matrices (<paramref name="matrix1"/> * <paramref name="matrix2"/>).
    /// </summary>
    /// <param name="matrix1">The left-hand matrix operand.</param>
    /// <param name="matrix2">The right-hand matrix operand.</param>
    /// <returns>The product resulting from multiplying the two matrices.</returns>
    public static Matrix4x4 operator *(Matrix4x4 matrix1, Matrix4x4 matrix2)
    {
        return new Matrix4x4(
            m00: matrix1.M00 * matrix2.M00 + matrix1.M01 * matrix2.M10 + matrix1.M02 * matrix2.M20 +
                 matrix1.M03 * matrix2.M30,
            m01: matrix1.M00 * matrix2.M01 + matrix1.M01 * matrix2.M11 + matrix1.M02 * matrix2.M21 +
                 matrix1.M03 * matrix2.M31,
            m02: matrix1.M00 * matrix2.M02 + matrix1.M01 * matrix2.M12 + matrix1.M02 * matrix2.M22 +
                 matrix1.M03 * matrix2.M32,
            m03: matrix1.M00 * matrix2.M03 + matrix1.M01 * matrix2.M13 + matrix1.M02 * matrix2.M23 +
                 matrix1.M03 * matrix2.M33,
            m10: matrix1.M10 * matrix2.M00 + matrix1.M11 * matrix2.M10 + matrix1.M12 * matrix2.M20 +
                 matrix1.M13 * matrix2.M30,
            m11: matrix1.M10 * matrix2.M01 + matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 +
                 matrix1.M13 * matrix2.M31,
            m12: matrix1.M10 * matrix2.M02 + matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 +
                 matrix1.M13 * matrix2.M32,
            m13: matrix1.M10 * matrix2.M03 + matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 +
                 matrix1.M13 * matrix2.M33,
            m20: matrix1.M20 * matrix2.M00 + matrix1.M21 * matrix2.M10 + matrix1.M22 * matrix2.M20 +
                 matrix1.M23 * matrix2.M30,
            m21: matrix1.M20 * matrix2.M01 + matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 +
                 matrix1.M23 * matrix2.M31,
            m22: matrix1.M20 * matrix2.M02 + matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 +
                 matrix1.M23 * matrix2.M32,
            m23: matrix1.M20 * matrix2.M03 + matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 +
                 matrix1.M23 * matrix2.M33,
            m30: matrix1.M30 * matrix2.M00 + matrix1.M31 * matrix2.M10 + matrix1.M32 * matrix2.M20 +
                 matrix1.M33 * matrix2.M30,
            m31: matrix1.M30 * matrix2.M01 + matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 +
                 matrix1.M33 * matrix2.M31,
            m32: matrix1.M30 * matrix2.M02 + matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 +
                 matrix1.M33 * matrix2.M32,
            m33: matrix1.M30 * matrix2.M03 + matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 +
                 matrix1.M33 * matrix2.M33
        );
    }

    /// <summary>
    /// Multiplies a 4x4 matrix by a 4D column vector (<paramref name="matrix"/> * <paramref name="vector4"/>).
    /// </summary>
    /// <param name="matrix">The transformation matrix operand.</param>
    /// <param name="vector4">The 4D vector/homogeneous coordinate operand.</param>
    /// <returns>The resulting transformed 4D vector.</returns>
    public static Vector4 operator *(Matrix4x4 matrix, Vector4 vector4)
    {
         return new Vector4(
              x: matrix.M00 * vector4.X + matrix.M01 * vector4.Y + matrix.M02 * vector4.Z + matrix.M03 * vector4.W,
              y: matrix.M10 * vector4.X + matrix.M11 * vector4.Y + matrix.M12 * vector4.Z + matrix.M13 * vector4.W,
              z: matrix.M20 * vector4.X + matrix.M21 * vector4.Y + matrix.M22 * vector4.Z + matrix.M23 * vector4.W,
              w: matrix.M30 * vector4.X + matrix.M31 * vector4.Y + matrix.M32 * vector4.Z + matrix.M33 * vector4.W
         );
    }
    
    /// <summary>
    /// Formats the 4x4 matrix as a multi-line string with aligned columns.
    /// </summary>
    /// <returns>A string representation of the 4 rows and 4 columns.</returns>
    public override string ToString()
    {
         return $"[{M00,8:F4}, {M01,8:F4}, {M02,8:F4}, {M03,8:F4}]\n" +
                $"[{M10,8:F4}, {M11,8:F4}, {M12,8:F4}, {M13,8:F4}]\n" +
                $"[{M20,8:F4}, {M21,8:F4}, {M22,8:F4}, {M23,8:F4}]\n" +
                $"[{M30,8:F4}, {M31,8:F4}, {M32,8:F4}, {M33,8:F4}]";
    }
}
