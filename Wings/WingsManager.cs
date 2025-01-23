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


    [Export] public Vector3 wind = new(0, 0, 5);
    [Export] RigidBody3D rb;
    public override void _PhysicsProcess(double delta)
    {


        CalculateAerodynamicForces(wind, 1.2f, out Vector3 forces, out Vector3 torque);
        if (Engine.IsEditorHint())
            return;
        rb.ApplyForce(forces);
        rb.ApplyTorque(torque);
    }

    private void CalculateAerodynamicForces(Vector3 wind, float airDensity, out Vector3 forces, out Vector3 torque)
    {
        forces = new();
        torque = new();
        foreach (var surface in wings)
        {
            Vector3 relativePosition = surface.GlobalPosition - GlobalPosition + rb.CenterOfMass;
            surface.CalculateForces(wind - rb.LinearVelocity, airDensity, relativePosition, out Vector3 _forces, out Vector3 _torque);
            forces += _forces;
            torque += _torque;
        }
    }

    private const string pitchUp = "pitchUp";
    private const string pitchDown = "pitchDown";
    private const string rollLeft = "rollLeft";
    private const string rollRight = "rollRight";
    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;
        float pitch = 0;
        float roll = 0;
        float yaw = 0;

        if (Input.IsActionPressed(pitchUp))
        {
            pitch = Input.GetActionStrength(pitchUp);
        }
        if (Input.IsActionPressed(pitchDown))
        {
            pitch = -Input.GetActionStrength(pitchDown);
        }
        if (Input.IsActionPressed(rollLeft))
        {
            roll = Input.GetActionStrength(rollLeft);
        }
        if (Input.IsActionPressed(rollRight))
        {
            roll = -Input.GetActionStrength(rollRight);
        }

        foreach (var wing in pitchWings)
        {
            wing.flapAngle = pitch * wing.flapAngleModifier;
        }
        foreach (var wing in rollWings)
        {
            wing.flapAngle = roll * wing.flapAngleModifier;
        }
        foreach (var wing in yawWings)
        {
            wing.flapAngle = yaw * wing.flapAngleModifier;
        }

        base._Process(delta);
    }
}