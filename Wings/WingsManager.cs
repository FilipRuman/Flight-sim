using System.Collections.Generic;
using Godot;
[Tool]
public partial class WingsManager : Node3D
{
    [Export] public float gravity;
    [Export] public Wing[] wings = null;
    [Export] public Wing[] pitchWings;
    [Export] public Wing[] rollWings;
    [Export] public Wing[] yawWings;

    [Export] Vector3 forcesModifiers = Vector3.One;
    [Export] public Vector3 wind = new(0, 0, 5);
    [Export] RigidBody3D rb;
    public override void _PhysicsProcess(double delta)
    {


        CalculateAerodynamicForces(wind, 1.2f, out Vector3 forces, out Vector3 torque);
        if (Engine.IsEditorHint())
            return;
        rb.ApplyForce(forces * (float)delta);
        rb.ApplyTorque(torque * (float)delta);
    }

    private void CalculateAerodynamicForces(Vector3 wind, float airDensity, out Vector3 forces, out Vector3 torque)
    {
        forces = new();
        torque = new();
        foreach (var surface in wings)
        {
            Vector3 relativePosition = surface.GlobalPosition - GlobalPosition + rb.CenterOfMass;
            surface.CalculateForces(wind - rb.LinearVelocity - rb.AngularVelocity.Cross(relativePosition), airDensity, relativePosition, forcesModifiers, out Vector3 _forces, out Vector3 _torque);
            forces += _forces;
            torque += _torque;
        }
    }

    private const string pitchUp = "pitchUp";
    private const string pitchDown = "pitchDown";
    private const string rollLeft = "rollLeft";
    private const string rollRight = "rollRight";
    private const string yawRight = "yawRight";
    private const string yawLeft = "yawLeft";

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;
        float pitch = 0;
        float roll = 0;
        float yaw = 0;

        if (Input.IsActionPressed(pitchUp))
        {
            pitch = -Input.GetActionStrength(pitchUp);
        }
        if (Input.IsActionPressed(pitchDown))
        {
            pitch = Input.GetActionStrength(pitchDown);
        }
        if (Input.IsActionPressed(rollLeft))
        {
            roll = Input.GetActionStrength(rollLeft);
        }
        if (Input.IsActionPressed(rollRight))
        {
            roll = -Input.GetActionStrength(rollRight);
        }
        if (Input.IsActionPressed(yawLeft))
        {
            yaw = -Input.GetActionStrength(yawLeft);
        }
        if (Input.IsActionPressed(yawRight))
        {
            yaw = Input.GetActionStrength(yawRight);
        }
        foreach (var wing in pitchWings)
        {
            float angle = pitch * wing.flapAngleModifier;
            SetControlSurface(wing, angle, false);
        }
        foreach (var wing in rollWings)
        {
            float angle = roll * wing.flapAngleModifier;
            SetControlSurface(wing, angle, false);
        }
        foreach (var wing in yawWings)
        {
            float angle = yaw * wing.flapAngleModifier;
            SetControlSurface(wing, angle, true);
        }

        base._Process(delta);
    }

    private static void SetControlSurface(Wing wing, float angle, bool yaw)
    {
        if (!wing.rotateWholeWing)
        {
            wing.flapAngle = angle;
        }
        else
        {
            wing.flapAngle = 0;
            wing.RotationDegrees = yaw ? new(wing.RotationDegrees.X, angle, wing.RotationDegrees.Z) : new(angle, wing.RotationDegrees.Y, wing.RotationDegrees.Z);
        }
    }
}