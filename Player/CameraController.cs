using Godot;
using System;

public partial class CameraController : Node
{
    private const string switchCam = "switchCam";
    private const string camUp = "camUp";
    private const string camDown = "camDown";
    private const string camLeft = "camLeft";
    private const string camRight = "camRight";

    [Export] Node3D fpRotation;
    [Export] Node3D tpRotation;

    [Export] Camera3D fp;
    [Export] Camera3D tp;

    [Export] Curve horizontalCamRotationCurve;
    [Export] Curve verticalCamRotationCurve;
    Vector2 lastMousePosition;
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed(switchCam))
        {
            GD.Print("switchCam");
            fp.Current = !fp.Current;
        }

        Vector3 camRotation = new();
        if (Input.IsActionPressed(camUp))
            camRotation.X = -Input.GetActionStrength(camUp);
        if (Input.IsActionPressed(camDown))
            camRotation.X = Input.GetActionStrength(camDown);
        if (Input.IsActionPressed(camLeft))
            camRotation.Y = Input.GetActionStrength(camLeft);
        if (Input.IsActionPressed(camRight))
            camRotation.Y = -Input.GetActionStrength(camRight);

        camRotation = new Vector3(verticalCamRotationCurve.SampleBaked(camRotation.X), horizontalCamRotationCurve.SampleBaked(camRotation.Y), 0);

        (fp.Current ? fpRotation : tpRotation).RotationDegrees = camRotation;

        base._Process(delta);
    }
}
