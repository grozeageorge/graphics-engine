namespace graphics_engine;

public struct Matrix4x4(
    float m00, float m01, float m02, float m03,
    float m10, float m11, float m12, float m13,
    float m20, float m21, float m22, float m23,
    float m30, float m31, float m32, float m33)
{
    public float M00 = m00, M01 = m01, M02 = m02, M03 = m03;
    public float M10 = m10, M11 = m11, M12 = m12, M13 = m13;                                                                
    public float M20 = m20, M21 = m21, M22 = m22, M23 = m23;
    public float M30 = m30, M31 = m31, M32 = m32, M33 = m33;

    public static Matrix4x4 Identity => new(
        1f, 0f, 0f, 0f,
        0f, 1f, 0f, 0f,
        0f, 0f, 1f, 0f,
        0f, 0f, 0f, 1f);

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

    public static Vector4 operator *(Matrix4x4 matrix, Vector4 point)
    {
         return new Vector4(
              x: matrix.M00 * point.X + matrix.M01 * point.Y + matrix.M02 * point.Z + matrix.M03 * point.W,
              y: matrix.M10 * point.X + matrix.M11 * point.Y + matrix.M12 * point.Z + matrix.M13 * point.W,
              z: matrix.M20 * point.X + matrix.M21 * point.Y + matrix.M22 * point.Z + matrix.M23 * point.W,
              w: matrix.M30 * point.X + matrix.M31 * point.Y + matrix.M32 * point.Z + matrix.M33 * point.W
         );
    }
    
    public override string ToString()
    {
         return $"[{M00,8:F4}, {M01,8:F4}, {M02,8:F4}, {M03,8:F4}]\n" +
                $"[{M10,8:F4}, {M11,8:F4}, {M12,8:F4}, {M13,8:F4}]\n" +
                $"[{M20,8:F4}, {M21,8:F4}, {M22,8:F4}, {M23,8:F4}]\n" +
                $"[{M30,8:F4}, {M31,8:F4}, {M32,8:F4}, {M33,8:F4}]";
    }
}