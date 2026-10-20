namespace graphics_engine;

/// <summary>
/// Manages affine 3D transformations by accumulating transformation matrices in pre-multiplication order.
/// </summary>
/// <remarks>
/// Supports method chaining for constructing compound transformations such as translation, rotation, scaling, and reflections.
/// </remarks>
public class Transform
{
    private Matrix4x4 _matrix = Matrix4x4.Identity;
    
    /// <summary>
    /// Gets the current accumulated 4x4 transformation matrix.
    /// </summary>
    public Matrix4x4 Matrix => _matrix;

    /// <summary>
    /// Applies a 3D translation by the specified offsets along X, Y, and Z axes.
    /// </summary>
    /// <param name="dx">Distance to translate along the X-axis.</param>
    /// <param name="dy">Distance to translate along the Y-axis.</param>
    /// <param name="dz">Distance to translate along the Z-axis.</param>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform Translate(float dx, float dy, float dz)
    {
        var trMatrix = Matrix4x4.Identity;
        trMatrix[0, 3] = dx;
        trMatrix[1, 3] = dy;
        trMatrix[2, 3] = dz;
        _matrix = trMatrix * _matrix;
        return this;
    }

    /// <summary>
    /// Applies a counter-clockwise rotation around the Z-axis by the specified angle in degrees.
    /// </summary>
    /// <param name="alpha">Rotation angle in degrees.</param>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform RotateOz(float alpha)
    {
        var rotMatrix = Matrix4x4.Identity;
        var rad = float.DegreesToRadians(alpha);
        
        rotMatrix[0, 0] = MathF.Cos(rad);
        rotMatrix[0, 1] = -MathF.Sin(rad);
        rotMatrix[1, 0] = MathF.Sin(rad);
        rotMatrix[1, 1] = MathF.Cos(rad);
        _matrix = rotMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Applies a rotation around the X-axis by the specified angle in degrees.
    /// </summary>
    /// <param name="beta">Rotation angle in degrees.</param>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform RotateOx(float beta)
    {
        var rotMatrix = Matrix4x4.Identity;
        var rad = float.DegreesToRadians(beta);
        
        rotMatrix[1, 1] = MathF.Cos(rad);
        rotMatrix[1, 2] = -MathF.Sin(rad);
        rotMatrix[2, 1] = MathF.Sin(rad);
        rotMatrix[2, 2] = MathF.Cos(rad);
        _matrix = rotMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Applies a rotation around the Y-axis by the specified angle in degrees.
    /// </summary>
    /// <param name="gamma">Rotation angle in degrees.</param>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform RotateOy(float gamma)
    {
        var rotMatrix = Matrix4x4.Identity;
        var rad = float.DegreesToRadians(gamma);
        
        rotMatrix[2, 0] = -MathF.Sin(rad);
        rotMatrix[2, 2] = MathF.Cos(rad);
        rotMatrix[0, 0] = MathF.Cos(rad);
        rotMatrix[0, 2] = MathF.Sin(rad);
        _matrix = rotMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Applies non-uniform scaling along the X, Y, and Z coordinate axes.
    /// </summary>
    /// <param name="sx">Scale factor along the X-axis.</param>
    /// <param name="sy">Scale factor along the Y-axis.</param>
    /// <param name="sz">Scale factor along the Z-axis.</param>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform Scale(float sx, float sy, float sz)
    {
        var scaleMatrix = Matrix4x4.Identity;
        scaleMatrix[0, 0] = sx;
        scaleMatrix[1, 1] = sy;
        scaleMatrix[2, 2] = sz;
        _matrix = scaleMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Applies a point reflection (inversion) through the coordinate origin (0, 0, 0).
    /// </summary>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform SymmetryOrigin()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        symMatrix[1, 1] = -1;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Applies an axial symmetry reflection across the X-axis (inverting Y and Z coordinates).
    /// </summary>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform SymmetryOx()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[1, 1] = -1;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Applies an axial symmetry reflection across the Y-axis (inverting X and Z coordinates).
    /// </summary>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform SymmetryOy()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Applies an axial symmetry reflection across the Z-axis (inverting X and Y coordinates).
    /// </summary>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform SymmetryOz()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        symMatrix[1, 1] = -1;
        _matrix = symMatrix * _matrix;
        return this;     
    }
    
    /// <summary>
    /// Applies a planar reflection across the X-Y coordinate plane (inverting the Z coordinate).
    /// </summary>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform SymmetryXoy()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }

    /// <summary>
    /// Applies a planar reflection across the Y-Z coordinate plane (inverting the X coordinate).
    /// </summary>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform SymmetryYoz()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }

    /// <summary>
    /// Applies a planar reflection across the X-Z coordinate plane (inverting the Y coordinate).
    /// </summary>
    /// <returns>The current <see cref="Transform"/> instance for fluent chaining.</returns>
    public Transform SymmetryXoz()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[1, 1] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    /// <summary>
    /// Transforms a 3D point by converting it to homogeneous coordinates (W = 1.0), multiplying by the accumulated matrix, and projecting back to 3D.
    /// </summary>
    /// <param name="vector">The 3D point in model coordinates.</param>
    /// <returns>The transformed 3D point in world coordinates.</returns>
    public Vector3 ApplyToPoint(Vector3 vector)
    {
        var homogenous = new Vector4(vector);
        var result = _matrix * homogenous;
        return new Vector3(result.X, result.Y, result.Z);
    }
}
