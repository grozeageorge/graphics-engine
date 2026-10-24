namespace graphics_engine;

/// <summary>
/// Specifies the editor transformation mode for the 3D viewport.
/// </summary>
public enum TransformMode
{
    /// <summary>
    /// Standard view mode with no transform gizmo rendered.
    /// </summary>
    None,

    /// <summary>
    /// Translation mode displaying a 3D transform gizmo.
    /// </summary>
    Translate,

    /// <summary>
    /// Rotation mode displaying a 3D transform gizmo.
    /// </summary>
    Rotate,

    /// <summary>
    /// Scaling mode displaying a 3D transform gizmo.
    /// </summary>
    Scale
}
