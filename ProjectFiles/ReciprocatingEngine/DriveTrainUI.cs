using Godot;
public partial class DriveTrainUI : Node {
    [Export] private DriveTrain main;
    [Export] EngineSoundController engineSoundController;

    [Export] CheckBox starterCheckbox;
    [Export] public Label angleOfAttackOfTip;

    [Export] Player.UiController uiController;
    [Export] HUD hud;
    [Export] TextureProgressBar rpmPercentageBarHUD;
    [Export] Label temperatureLabelHUD;

    [Export] private string starterInputAction;

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed(starterInputAction)) {
            main.starterButtonPressed = !main.starterButtonPressed;
            main.engine.holdIdle = true;
        }
        starterCheckbox.ButtonPressed = main.starterButtonPressed;
        hud.throttleToDisplay = main.engine.throttle;
        hud.thrustToDisplay = main.currentThrust;

        uiController.thrustToDisplay = main.currentThrust;

        float engineRPM = main.engine.crankshaft.RevolutionsPerSecond * 60;
        float propellerRPM = engineRPM * main.gearRatio;
        engineSoundController.throttle = main.engine.throttle;
        engineSoundController.rpm = propellerRPM;

        uiController.propellerRpm = propellerRPM;

        rpmPercentageBarHUD.Value = engineRPM;
        temperatureLabelHUD.Text = $"{(uint)(main.engine.heatHandler.cylinderWallTemperature - 273)}";

        base._Process(delta);
    }
    public override void _Ready() {
        rpmPercentageBarHUD.MinValue = 0;
        rpmPercentageBarHUD.MaxValue = main.engine.rpmLimit;
        base._Ready();
    }
}
