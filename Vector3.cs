namespace graphics_engine;

public struct Vector3(float x, float y, float z)
{
    public float X = x;
    public float Y = y;
    public float Z = z;
    
    public static Vector3 Zero => new(0f, 0f, 0f); 
    
    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
    
    public override string ToString()
    {
        return $"({X:F4}, {Y:F4}, {Z:F4})";
    }
    
    public Vector3 ToVector3() => new(X, Y, Z);
    
    public static Vector3 operator *(Vector3 v, float scalar)
    {
        return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }
}