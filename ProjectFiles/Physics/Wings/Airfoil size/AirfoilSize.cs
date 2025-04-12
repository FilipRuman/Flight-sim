using Godot;
[Tool, GlobalClass]
public partial class AirfoilSize : Resource {

    [Export] float span;
    /// <summary>
    ///  also a wing chord 
    /// </summary>
    [Export] float a;
    [Export] float b;
    [Export] float bOffset;
    [Export] bool displaySize;

    Color staticWingColor = Colors.LawnGreen;
    Color controlSurfaceColor = Colors.Orange;
    [Export] bool isControlSurface;

    public float area;
    public float standardMeanChord;

    [Export] bool mirrorDisplay;

    public void CalculateArea() {
        area = (a + b) * span / 2;

        standardMeanChord = area / span;
    }
    public void DisplaySize(Vector3 position, Basis basis) {
        if (!displaySize)
            return;
        Color color = isControlSurface ? controlSurfaceColor : staticWingColor;

        Vector3 aStart = position;
        Vector3 aEnd = aStart + basis.Z * a;

        float mirrorModifier = mirrorDisplay ? -1 : 1;
        Vector3 bStart = aStart + basis.X * mirrorModifier * span + basis.Z * bOffset;
        Vector3 bEnd = aStart + basis.X * mirrorModifier * span + basis.Z * (bOffset + b);

        DebugDraw3D.DrawLine(aStart, aEnd, color);
        DebugDraw3D.DrawLine(bEnd, bStart, color);

        DebugDraw3D.DrawLine(aStart, bStart, color);

        DebugDraw3D.DrawLine(aEnd, bEnd, color);




    }
}

