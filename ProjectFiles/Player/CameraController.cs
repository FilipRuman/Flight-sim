using Godot;
using System;

public partial class CameraController : Node
{
    private const string switchCam = "switchCam";
    private const string camUp = "camUp";
    private const string camDown = "camDown";
    private const string camLeft = "camLeft";
    private const string camRight = "camRight";
    [Export] Node3D thirdPersonPoint;
    [Export] Node3D firstPersonPoint;

    [Export] Node3D thirdPersonRotation;
    [Export] Node3D firstPersonRotation;
    public Node3D currentPoint;


    [Export] Curve horizontalCamRotationCurve;
    [Export] Curve verticalCamRotationCurve;
    Vector2 lastMousePosition;
    public override void _Ready()
    {
        currentPoint = firstPersonPoint;
        base._Ready();
    }
    public override void _Process(double delta)
    {

        if (Input.IsActionJustPressed(switchCam))
        {
            currentPoint = currentPoint == firstPersonPoint ? thirdPersonPoint : firstPersonPoint;
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
        (currentPoint == firstPersonPoint ? firstPersonRotation : thirdPersonRotation).RotationDegrees = camRotation;

        PlayerCam.Ref.cam.GlobalPosition = currentPoint.GlobalPosition;
        PlayerCam.Ref.cam.GlobalRotation = currentPoint.GlobalRotation;

        base._Process(delta);
    }
}
