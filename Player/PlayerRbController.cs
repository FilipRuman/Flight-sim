using Godot;
using System;

public partial class PlayerRbController : RigidBody3D
{
    [Export] public float currentGForce = 0;
    // Gravity constant
    private const float GRAVITY = 9.81f;
    private Vector3 previousVelocity = Vector3.Zero;
    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        // Calculate the change in velocity (acceleration)
        Vector3 currentVelocity = state.LinearVelocity;
        Vector3 acceleration = (currentVelocity - previousVelocity) / state.Step;

        // Update previous velocity for the next frame
        previousVelocity = currentVelocity;

        // Calculate the G-force
        currentGForce = acceleration.Length() / GRAVITY;
    }


}
