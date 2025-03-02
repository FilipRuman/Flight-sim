using Godot;
using System;

public partial class CloudsPositionHandler : Node3D
{
    [Export] RayTracingHandler cloudsHandler;
    [Export] Node3D playerCam;
    [Export] Node3D cloudsBox;


    public override void _Process(double delta)
    {
        Vector3 newCloudsPosition = new(playerCam.GlobalPosition.X, cloudsBox.GlobalPosition.Y, playerCam.GlobalPosition.Z);
        cloudsBox.GlobalPosition = newCloudsPosition;
        cloudsHandler.cloudsOffset = newCloudsPosition;
        base._Process(delta);
    }

}
