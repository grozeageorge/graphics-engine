namespace graphics_engine;

public class Transform
{
    private Matrix4x4 _matrix = Matrix4x4.Identity;
    
    public Matrix4x4 Matrix => _matrix;

    public Transform Translate(float dx, float dy, float dz)
    {
        var trMatrix = Matrix4x4.Identity;
        trMatrix[0, 3] = dx;
        trMatrix[1, 3] = dy;
        trMatrix[2, 3] = dz;
        _matrix = trMatrix * _matrix;
        return this;
    }

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
    
    public Transform Scale(float sx, float sy, float sz)
    {
        var scaleMatrix = Matrix4x4.Identity;
        scaleMatrix[0, 0] = sx;
        scaleMatrix[1, 1] = sy;
        scaleMatrix[2, 2] = sz;
        _matrix = scaleMatrix * _matrix;
        return this;
    }
    
    public Transform SymmetryOrigin()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        symMatrix[1, 1] = -1;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    public Transform SymmetryOx()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[1, 1] = -1;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    public Transform SymmetryOy()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    public Transform SymmetryOz()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        symMatrix[1, 1] = -1;
        _matrix = symMatrix * _matrix;
        return this;     
    }
    
    public Transform SymmetryXoy()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[2, 2] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }

    public Transform SymmetryYoz()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[0, 0] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }

    public Transform SymmetryXoz()
    {
        var symMatrix = Matrix4x4.Identity;
        symMatrix[1, 1] = -1;
        _matrix = symMatrix * _matrix;
        return this;
    }
    
    public Vector3 ApplyToPoint(Vector3 point)
    {
        var homogenous = new Vector4(point.X, point.Y, point.Z, 1);
        var result = _matrix * homogenous;
        return new Vector3(result.X, result.Y, result.Z);
    }

}