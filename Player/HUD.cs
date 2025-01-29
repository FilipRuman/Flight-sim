using Godot;
using System;

public partial class HUD : Node
{
    [Export] PlayerRbController playerRbController;
    [Export] RigidBody3D rb;
    [Export] WingsManager wingsManager;
    [Export] Camera3D cam;
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
        // 
        frameIndex++;
        DrawHorizon();
        flightPathIndicator.Position = cam.UnprojectPosition(cam.GlobalPosition + rb.LinearVelocity);
        flightPathIndicator.RotationDegrees = -rb.RotationDegrees.Z;
        planeNoseDirectionIndicator.Position = cam.UnprojectPosition(cam.GlobalPosition + rb.Basis.Z);
        throttle.Value = thruster.throttle;

        if (frameIndex < FramesPerUpdate)
        {
            base._Process(delta);
            return;
        }
        G.Text = $"G {Math.Round(playerRbController.CurrentGForce, 1)}";
        Match.Text = $"M {Math.Round(Air.GetMatchNumber(wingsManager.FrontalVelocity, wingsManager.Altitude), 2)}";
        frameIndex = 0;
        thrustOfPropeller.Text = $"{Math.Round(thruster.thrustOfPropeller / 1000)} MN";

        Speed.Text = Mathf.RoundToInt(wingsManager.FrontalVelocity * 3.6f).ToString();
        var altitude = Mathf.RoundToInt(wingsManager.Altitude).ToString();
        Altitude.Text = altitude.Length > 3 ? altitude.Insert(altitude.Length - 3, ".") : altitude;
        AoA.Text = $"{Math.Round(wingsManager.wings[0].angleOfAttack, 1)}∝";
        Rotation.Text = $"{Math.Round(-rb.RotationDegrees.X, 1)}°R";
        Heading.Text = Math.Round(Mathf.RadToDeg(cam.GlobalRotation.Y) / 10).ToString();


    }

    public void DrawHorizon()
    {
        horizon.Position = cam.UnprojectPosition(cam.GlobalPosition + Vector3.Forward.Rotated(Vector3.Up, cam.GlobalRotation.Y)) - horizon.Size / 2;
        horizon.RotationDegrees = cam.GlobalRotationDegrees.Z;


    }


}
