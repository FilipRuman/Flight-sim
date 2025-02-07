using System.Collections.Generic;
using Godot;
[Tool]
public partial class WingsManager : Node3D
{
    [Export] public Wing[] wings = null;
    [Export] public Wing[] pitchWings;
    [Export] public Wing[] rollWings;
    [Export] public Wing[] yawWings;

    [Export] Vector3 forcesModifiers = Vector3.One;
    [Export] public RigidBody3D rb;

    public float Altitude => rb.GlobalPosition.Y;
    public float FrontalVelocity => (rb.GlobalBasis.Inverse() * rb.LinearVelocity).Z;




    public override void _PhysicsProcess(double delta)
    {
        ProjectSettings.SetSetting("physics/2d/default_gravity", ConstantsManager.gravity);
        CalculateAerodynamicForces(Air.wind, Air.GetLocalAirDensity(Altitude), out Vector3 forces, out Vector3 torque);
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
            Vector3 relativePosition = surface.GlobalPosition - rb.GlobalPosition + rb.CenterOfMass;
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
    private const string trimUp = "trimUp";
    private const string trimDown = "trimDown";

    private float trimVertical;
    [Export] private float trimStepSize = .1f;

    public override void _Process(double delta)
    {
        // DebugDraw3D.DrawSphere(, 1, Colors.DarkSeaGreen);

        if (Engine.IsEditorHint())
            return;
        float pitch = 0;
        float roll = 0;
        float yaw = 0;

        if (Input.IsActionJustPressed(trimUp))
            trimVertical = Mathf.Clamp(trimVertical + trimStepSize, -1, 1);
        if (Input.IsActionJustPressed(trimDown))
            trimVertical = Mathf.Clamp(trimVertical - trimStepSize, -1, 1);


        if (Input.IsActionPressed(pitchUp))
            pitch = -Input.GetActionStrength(pitchUp);

        if (Input.IsActionPressed(pitchDown))
            pitch = Input.GetActionStrength(pitchDown);

        if (Input.IsActionPressed(rollLeft))
            roll = Input.GetActionStrength(rollLeft);
        if (Input.IsActionPressed(rollRight))
            roll = -Input.GetActionStrength(rollRight);

        if (Input.IsActionPressed(yawLeft))
            yaw = -Input.GetActionStrength(yawLeft);

        if (Input.IsActionPressed(yawRight))

            yaw = Input.GetActionStrength(yawRight);

        foreach (var wing in pitchWings)
        {
            float flapAngleModifier = wing.flapAngleModifier;

            float angle = Mathf.Clamp((pitch + trimVertical) * flapAngleModifier, -flapAngleModifier, flapAngleModifier);
            SetControlSurface(wing, angle, yaw: false);
        }
        foreach (var wing in rollWings)
        {
            float angle = roll * wing.flapAngleModifier;
            SetControlSurface(wing, angle, yaw: false);
        }
        foreach (var wing in yawWings)
        {
            float angle = yaw * wing.flapAngleModifier;
            SetControlSurface(wing, angle, yaw: true);
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
            if (yaw)
                wing.RotationDegrees = new(wing.RotationDegrees.X, angle, wing.RotationDegrees.Z);
            else
                wing.RotationDegrees = new(angle, wing.RotationDegrees.Y, wing.RotationDegrees.Z);

        }
    }
}