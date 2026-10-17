using graphics_engine;

// Create and configure a 3D transformation pipeline:
// 1. Translates by (+2 along X, +3 along Y, 0 along Z)
// 2. Rotates 90 degrees counter-clockwise around the Z-axis
var transform = new Transform();
transform.Translate(2, 3, 0)
    .RotateOz(90);

// Define a test point in model coordinates (1, 0, 0)
var point = new Vector3(1, 0, 0);

// Apply the accumulated affine transformation to the point
var transformedPoint = transform.ApplyToPoint(point);

// Output the accumulated transformation matrix and resulting transformed coordinate
Console.WriteLine("Matrix:\n" + transform.Matrix);
Console.WriteLine("Transformed Point: " + transformedPoint);
