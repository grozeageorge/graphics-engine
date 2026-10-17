namespace graphics_engine;

/// <summary>
/// Represents a 4-dimensional vector or homogeneous coordinate with single-precision floating point components.
/// </summary>
/// <param name="x">The initial X component.</param>
/// <param name="y">The initial Y component.</param>
/// <param name="z">The initial Z component.</param>
/// <param name="w">The initial W component (homogeneous coordinate).</param>
public struct Vector4(float x, float y, float z, float w)
{
    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public float X = x;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public float Y = y;

    /// <summary>
    /// The Z component of the vector.
    /// </summary>
    public float Z = z;

    /// <summary>
    /// The W component (homogeneous coordinate) of the vector.
    /// </summary>
    public float W = w;
    
    /// <summary>
    /// Gets a 4D vector with all components set to zero: (0, 0, 0, 0).
    /// </summary>
    public static Vector4 Zero => new(0f, 0f, 0f, 0f);

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector4"/> struct from a 3D vector and an optional W component.
    /// </summary>
    /// <param name="v">The 3D vector supplying X, Y, and Z coordinates.</param>
    /// <param name="w">The homogeneous coordinate component, defaulting to 1.0f (point representation).</param>
    public Vector4(Vector3 v, float w = 1.0f) : this(v.X, v.Y, v.Z, w) { }

    /// <summary>
    /// Adds two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector operand.</param>
    /// <param name="b">The second vector operand.</param>
    /// <returns>A new <see cref="Vector4"/> representing the sum of both vectors.</returns>
    public static Vector4 operator +(Vector4 a, Vector4 b)
    {
        return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }

    /// <summary>
    /// Multiplies each component of a 4D vector by a scalar value.
    /// </summary>
    /// <param name="v">The vector to scale.</param>
    /// <param name="scalar">The scalar multiplier.</param>
    /// <returns>A new <see cref="Vector4"/> with scaled components.</returns>
    public static Vector4 operator *(Vector4 v, float scalar)
    {
        return new Vector4(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);
    }

    /// <summary>
    /// Multiplies each component of a 4D vector by a scalar value.
    /// </summary>
    /// <param name="scalar">The scalar multiplier.</param>
    /// <param name="v">The vector to scale.</param>
    /// <returns>A new <see cref="Vector4"/> with scaled components.</returns>
    public static Vector4 operator *(float scalar, Vector4 v) => v * scalar;

    /// <summary>
    /// Converts this 4D vector back to a 3D vector by dropping the W component.
    /// </summary>
    /// <returns>A <see cref="Vector3"/> with the X, Y, and Z coordinates.</returns>
    public Vector3 ToVector3() => new(X, Y, Z);

    /// <summary>
    /// Returns a formatted string representation of the 4D vector components with 4 decimal places.
    /// </summary>
    /// <returns>A string in the format <c>(X, Y, Z, W)</c>.</returns>
    public override string ToString()
    {
        return $"({X:F4}, {Y:F4}, {Z:F4}, {W:F4})";
    }
}