using Godot;
using System;

public partial class DriveTrain : Node {
    [Export] RigidBody3D rb;
    [Export] Player.UiController uiController;
    [Export] HUD hud;
    [Export] EngineSoundController engineSoundController;

    [Export] WingsManager wingsManager;
    [Export] Propeller propeller;
    [Export] Crankshaft crankshaft;
    [Export] public EngineController engine;

    [Export] public float momentOfInertia;

    [Export] float gearRatio;
    [Export] public float starterSpeed;
    [Export] float currentAngularVelocity /* rad/s */;

    public bool starterButtonPressed;

    [Export] CheckBox starterCheckbox;
    [Export] Label angleOfAttackOfTip;

    [Export] private float enginePhysicsUpdatesPerSecond;
    private TickSystem enginePhysicsTickSystem = new();
    [Export] private string starterInputAction;
    public override void _Ready() {
        enginePhysicsTickSystem.updatesPerSecond = enginePhysicsUpdatesPerSecond;
        enginePhysicsTickSystem.toCall.Clear();
        enginePhysicsTickSystem.toCall.Add(HandlePhysics);

        base._Ready();
    }
    public float propellerRPM => crankshaft.RevolutionsPerSecond * 60 * gearRatio;
    // Maybe later add drivetrainUI 
    public override void _Process(double delta) {
        if (Input.IsActionJustPressed(starterInputAction))
            starterButtonPressed = !starterButtonPressed;
        starterCheckbox.ButtonPressed = starterButtonPressed;
        hud.throttleToDisplay = engine.throttle;
        hud.thrustToDisplay = currentThrust;

        uiController.thrustToDisplay = currentThrust;
        uiController.propellerRpm = propellerRPM;

        engineSoundController.throttle = engine.throttle;
        engineSoundController.rpm = propellerRPM;
        base._Process(delta);
    }
    public float currentThrust;
    [Export] float dragModifier;
    [Export] bool enable;
    private void HandlePhysics(float delta) {
        if (!enable)
            return;
        engine.HandlePhysics(delta);
        engine.PhysicsProcessDataForLaterUI();

        Air.GetLocalAirStatistics(wingsManager.Altitude, out float airDensity, out float airTemperature);
        engine.ambientAirTemperature = airTemperature;
        engine.ambientAirDensity = airDensity;

        propeller.HandlePhysics(delta, wingsManager.FrontalVelocity, airDensity, currentAngularVelocity * gearRatio, out float thrust, out float propellerDrag, out string aoaDebug);
        angleOfAttackOfTip.Text = "angles of attack of propeller elements: \n" + aoaDebug;
        ApplyThrust(thrust);

        currentThrust = thrust;

        float drivetrainTorque = engine.currentTorque - propellerDrag * dragModifier * gearRatio;
        float deltaAngularMomentum = drivetrainTorque;
        float deltaAngularVelocity = deltaAngularMomentum / momentOfInertia;

        currentAngularVelocity += deltaAngularVelocity;
        currentAngularVelocity = Mathf.Max(currentAngularVelocity, 0);


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
