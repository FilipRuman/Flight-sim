using Godot;
using System;

public partial class Thruster : Node3D {

    [Export] private RigidBody3D rb;
    private const string throttleUpAction = "throttleUp";
    private const string throttleDownAction = "throttleDown";

    public float throttle = 0;
    private double throttleChangeSpeedModifier = .2;
    [Export] private EngineSoundController engineSoundController;


    [Export] private WingsManager wingsManager;
    public override void _Process(double delta) {
        if (Input.IsActionPressed(throttleUpAction)) {
            ChangeThrottle(Input.GetActionStrength(throttleUpAction), delta);
        }
        if (Input.IsActionPressed(throttleDownAction)) {
            ChangeThrottle(-Input.GetActionStrength(throttleDownAction), delta);
        }
    }

    private void ChangeThrottle(float amount, double delta) {
        throttle = (float)Mathf.Clamp(throttle + amount * delta * throttleChangeSpeedModifier, 0, 1);
        engineSoundController.Throttle = throttle;
    }
    [Export] public float enginePower = 100 /* W */;
    [Export] public float propellerEfficiency = 0.85f;
    [Export] public float PropellerRadius = 2;/* m */

    private double CalculateThrustOfPropeller(float initialFrontalVelocity, float airDensity) {
        float enginePower = EnginePowerOutput;

        double propellerDiskArea = Math.PI * Math.Pow(PropellerRadius, 2);

        double inducedAirVelocity = Math.Sqrt(2 * enginePower * propellerEfficiency / (airDensity * propellerDiskArea));
        // GD.Print($"{inducedAirVelocity} {enginePower} {airDensity} {propellerDiskArea}");
        double finalVelocity = initialFrontalVelocity + 2 * inducedAirVelocity;

        double thrust = 2 * enginePower * propellerEfficiency / (initialFrontalVelocity + finalVelocity);

        return thrust;
    }

    float EnginePowerOutput => throttle * enginePower * 1000;// Converting kW to W

    public float thrustOfPropeller = 0;

    public override void _PhysicsProcess(double delta) {
        thrustOfPropeller = (float)CalculateThrustOfPropeller(wingsManager.FrontalVelocity, Air.GetLocalAirDensity(wingsManager.Altitude));
        if (float.IsNaN(thrustOfPropeller))
            return;
        rb.ApplyForce(rb.Transform.Basis.Z * thrustOfPropeller);
        base._PhysicsProcess(delta);
    }
}
