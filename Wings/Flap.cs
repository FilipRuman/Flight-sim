using Godot;
using System;

public partial class Flap : Node3D
{
	[Export] Wing wing;

	public override void _Process(double delta)
	{
		if (wing.rotateWholeWing)
			RotationDegrees = Vector3.Right * wing.RotationDegrees.X;
		else
			RotationDegrees = Vector3.Right * wing.flapAngle;
	}
}
