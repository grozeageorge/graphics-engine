using graphics_engine;

// Create and configure a 3D transformation pipeline:
// 1. Translates by (+2 along X, +3 along Y, 0 along Z)
// 2. Rotates 90 degrees counter-clockwise around the Z-axis
var transform = new Transform();
Console.WriteLine("Matrix:\n" + transform.Matrix);
transform.Translate(2, 3, 1);
Console.WriteLine("Matrix:\n" + transform.Matrix);
transform.RotateOz(90);
Console.WriteLine("Matrix:\n" + transform.Matrix);
transform.RotateOy(90);
Console.WriteLine("Matrix:\n" + transform.Matrix);
transform.RotateOz(90);
Console.WriteLine("Matrix:\n" + transform.Matrix);
