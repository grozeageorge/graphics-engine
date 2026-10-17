using graphics_engine;

var transform = new Transform();
transform.Translate(2, 3, 0)
    .RotateOz(90);

var point = new Vector3(1, 0, 0);
var transformedPoint = transform.ApplyToPoint(point);

Console.WriteLine("Matrix:\n" + transform.Matrix);
Console.WriteLine("Transformed Point: " + transformedPoint);