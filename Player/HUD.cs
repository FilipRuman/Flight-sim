using Godot;
using System;

public partial class HUD : Node
{
    [Export] PlayerRbController playerRbController;
    [Export] RigidBody3D rb;
    [Export] WingsManager wingsManager;
    Camera3D currentCam;
    [Export] CameraController cameraController;
    [Export] private Thruster thruster;

    [Export] Control horizon;
    [Export] Control flightPathIndicator;
    [Export] Control planeNoseDirectionIndicator;
    [Export] Label Speed;
    [Export] Label Altitude;
    [Export] Label G;
    [Export] Label Match;
    [Export] Label AoA;
    [Export] Label Rotation;
    [Export] Label Heading;
    [Export] private Label thrustOfPropeller;
    [Export] private Slider throttle;
    [Export] float FramesPerUpdate = 10;
    int frameIndex = 0;

    public override void _Process(double delta)
    {
        currentCam = cameraController.currentCam;
        // 
        frameIndex++;
        DrawHorizon();
        flightPathIndicator.Position = currentCam.UnprojectPosition(currentCam.GlobalPosition + rb.LinearVelocity);
        flightPathIndicator.RotationDegrees = -rb.RotationDegrees.Z;
        planeNoseDirectionIndicator.Position = currentCam.UnprojectPosition(currentCam.GlobalPosition + rb.Basis.Z);
        throttle.Value = thruster.throttle;

        if (frameIndex < FramesPerUpdate)
        {
            base._Process(delta);
            return;
        }
        G.Text = $"G {Math.Round(playerRbController.currentGForce, 1)}";
        Match.Text = $"M {Math.Round(Air.GetMatchNumber(wingsManager.FrontalVelocity, wingsManager.Altitude), 2)}";
        frameIndex = 0;
        thrustOfPropeller.Text = $"{Math.Round(thruster.thrustOfPropeller / 1000)} kN";

        Speed.Text = AddDots(Mathf.RoundToInt(wingsManager.FrontalVelocity * 3.6f).ToString()); ;
        var altitude = Mathf.RoundToInt(wingsManager.Altitude).ToString();
        Altitude.Text = AddDots(altitude);
        AoA.Text = $"{Math.Round(wingsManager.wings[0].angleOfAttack, 1)}∝";
        Rotation.Text = $"{Math.Round(-rb.RotationDegrees.X, 1)}°R";
        Heading.Text = Math.Round(Mathf.RadToDeg(currentCam.GlobalRotation.Y) / 10).ToString();


    }
    static string AddDots(string input)
    {
        bool endsWithSymbol = input[^1] is '+' or '-';
        int indexToAddDotOn = 3 + (endsWithSymbol ? 1 : 0);
        return (input.Length > indexToAddDotOn) ? input.Insert(input.Length - indexToAddDotOn, ".") : input;
    }

    public void DrawHorizon()
    {
        horizon.Position = currentCam.UnprojectPosition(currentCam.GlobalPosition + Vector3.Forward.Rotated(Vector3.Up, currentCam.GlobalRotation.Y)) - horizon.Size / 2;
        horizon.RotationDegrees = currentCam.GlobalRotationDegrees.Z;


    }


}
