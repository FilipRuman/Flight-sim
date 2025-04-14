using Godot;
using System;

public partial class DriveTrain : Node {
    [Export] RigidBody3D rb;
    [Export] WingsManager wingsManager;
    [Export] Propeller propeller;
    [Export] Crankshaft crankshaft;
    [Export] public EngineController engine;
    [Export] public float momentOfInertia;

    [Export] float gearRatio;
    [Export] public float starterSpeed;
    [Export] float currentAngularVelocity /* rad/s */;

    public bool starterButtonPressed;

    [Export] private float enginePhysicsUpdatesPerSecond;
    private TickSystem enginePhysicsTickSystem = new();

    public override void _Ready() {
        enginePhysicsTickSystem.updatesPerSecond = enginePhysicsUpdatesPerSecond;
        enginePhysicsTickSystem.toCall.Clear();
        enginePhysicsTickSystem.toCall.Add(HandlePhysics);
        base._Ready();
    }

    private void HandlePhysics(float delta) {
        engine.HandlePhysics(delta);
        engine.PhysicsProcessDataForLaterUI();

        Air.GetLocalAirStatistics(wingsManager.Altitude, out float airDensity, out float airTemperature);
        engine.ambientAirTemperature = airTemperature;
        engine.ambientAirDensity = airDensity;

        propeller.HandlePhysics(wingsManager.FrontalVelocity, airDensity, currentAngularVelocity * gearRatio, out float thrust, out float propellerDrag);
        ApplyThrust(thrust);

        float drivetrainTorque = engine.currentTorque - propellerDrag * gearRatio;
        float deltaAngularMomentum = drivetrainTorque * delta;
        float deltaAngularVelocity = deltaAngularMomentum / momentOfInertia;

        currentAngularVelocity += deltaAngularVelocity;

        if (starterButtonPressed)
            currentAngularVelocity = starterSpeed;

        crankshaft.UpdateCrankshaftStatsBasedOnDrivetrain(currentAngularVelocity, delta);



    }
    private void ApplyThrust(float thrust) {
        if (float.IsNaN(thrust))
            return;
        rb.ApplyForce(rb.Transform.Basis.Z * thrust);
    }
    public override void _PhysicsProcess(double delta) {
        if (Engine.IsEditorHint())
            return;
        enginePhysicsTickSystem.Update((float)delta);


        base._PhysicsProcess(delta);
    }
}
