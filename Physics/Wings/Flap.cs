using Godot;
using System;

public partial class Flap : Node3D
{
	[Export] Wing wing;
	[Export] bool yaw;
	public override void _Process(double delta)
	{
		if (wing.rotateWholeWing)
		{
			if (yaw)
				RotationDegrees = Vector3.Up * wing.RotationDegrees.Y;
			else
				RotationDegrees = Vector3.Right * wing.RotationDegrees.X;
		}
		else
			RotationDegrees = Vector3.Right * wing.flapAngle;
	}
}
