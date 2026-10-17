namespace graphics_engine;

/// <summary>
/// Represents a 3-dimensional vector with single-precision floating point components.
/// </summary>
/// <param name="x">The initial X coordinate.</param>
/// <param name="y">The initial Y coordinate.</param>
/// <param name="z">The initial Z coordinate.</param>
public struct Vector3(float x, float y, float z)
{
    /// <summary>
    /// The X coordinate of the vector.
    /// </summary>
    public float X = x;

    /// <summary>
    /// The Y coordinate of the vector.
    /// </summary>
    public float Y = y;

    /// <summary>
    /// The Z coordinate of the vector.
    /// </summary>
    public float Z = z;
    
    /// <summary>
    /// Initializes a new instance of <see cref="Vector3"/> from a <see cref="Vector4"/> by discarding the W component.
    /// </summary>
    /// <param name="v">The 4D vector source.</param>
    public Vector3(Vector4 v) : this(v.X, v.Y, v.Z) { }

    /// <summary>
    /// Gets a vector with all components set to zero: (0, 0, 0).
    /// </summary>
    public static Vector3 Zero => new(0f, 0f, 0f); 
    
    /// <summary>
    /// Adds two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector operand.</param>
    /// <param name="b">The second vector operand.</param>
    /// <returns>A new <see cref="Vector3"/> representing the sum of both vectors.</returns>
    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    /// <summary>
    /// Multiplies each component of a vector by a scalar value.
    /// </summary>
    /// <param name="v">The vector to scale.</param>
    /// <param name="scalar">The scalar factor.</param>
    /// <returns>A new <see cref="Vector3"/> with scaled components.</returns>
    public static Vector3 operator *(Vector3 v, float scalar)
    {
        return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }

    /// <summary>
    /// Multiplies each component of a vector by a scalar value.
    /// </summary>
    /// <param name="scalar">The scalar factor.</param>
    /// <param name="v">The vector to scale.</param>
    /// <returns>A new <see cref="Vector3"/> with scaled components.</returns>
    public static Vector3 operator *(float scalar, Vector3 v) => v * scalar;

    /// <summary>
    /// Returns a formatted string representation of the vector components with 4 decimal places.
    /// </summary>
    /// <returns>A string in the format <c>(X, Y, Z)</c>.</returns>
    public override string ToString()
    {
        return $"({X:F4}, {Y:F4}, {Z:F4})";
    }
}
