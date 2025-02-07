using Godot;
using System;

public partial class Flap : Node3D
{
	[Export] Wing wing;
	[Export] bool yaw;
	[Export] Vector3 rotationVector;
	Vector3 startRotation;
	public override void _Ready()
	{
		startRotation = RotationDegrees;

		base._Ready();
	}
	public override void _Process(double delta)
	{
		if (wing.rotateWholeWing)
		{
			if (yaw)
				RotationDegrees = startRotation + rotationVector * wing.RotationDegrees.Y;
			else
				RotationDegrees = startRotation + rotationVector * wing.RotationDegrees.X;
		}
		else
			RotationDegrees = startRotation + rotationVector * wing.flapAngle;
	}
}
