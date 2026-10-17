namespace graphics_engine;

public struct Vector4(float x, float y, float z, float w)
{
    public float X = x;
    public float Y = y;
    public float Z = z;
    public float W = w;
    
    public static Vector4 Zero => new(0f, 0f, 0f, 0f);
    
    public override string ToString()
    {
        return $"({X:F4}, {Y:F4}, {Z:F4}, {W:F4})";
    }
    
    public static Vector4 operator +(Vector4 a, Vector4 b)
    {
        return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }
    
    public Vector4(Vector3 v, float w = 1.0f) : this(v.X, v.Y, v.Z, w) { }
    
    public static Vector4 operator *(Vector4 v, float scalar)
    {
        return new Vector4(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);
    }
}