namespace Player {
    using Godot;
    using System;
    public partial class UiController : CanvasLayer {
        [ExportGroup("Ref")]
        [Export] private RigidBody3D rb;
        [Export] private WingsManager wingsManager;
        [Export] WindController windController;


        [ExportGroup("UI")]
        [Export] private Label speed;
        [Export] private Label AoA;
        [Export] private Label altitude;
        [Export] private Label airDensity;
        [Export] public Label thrustOfPropeller;
        [Export] private Label fps;

        [Export] float FramesPerUpdate = 10;
        int frameIndex = 0;

        [ExportGroup("Prop")]
        [Export] bool displayPropeller;
        [Export] private Node3D propellor;
        [Export] private Sprite3D propAtHighSpeed;
        [Export] private float propRotationModifier = 300;
        [Export] public float maxPropRpmBeforeSprite = 1000;


        [ExportGroup("Controls")]
        [Export] private Panel controls;
        [Export] private Button controlsToggleButton;
        public override void _Ready() {
            controlsToggleButton.Pressed += () => controls.Visible = !controls.Visible;
            base._Ready();
        }


        public float thrustToDisplay;
        public float propellerRpm;
        public override void _Process(double delta) {
            fps.Text = $"fps: {Mathf.RoundToInt(Engine.GetFramesPerSecond())}";

            frameIndex++;
            float velocity = wingsManager.FrontalVelocity * 3.6f; /* in km/h */
            windController.Speed = velocity;

            UpdatePropeller();

            if (frameIndex < FramesPerUpdate)
                return;
            frameIndex = 0;

            /*throttle.Editable = false;
			 */
            var altitude = wingsManager.Altitude;
            this.altitude.Text = $"{Mathf.RoundToInt(altitude)} m";
            airDensity.Text = $"{Math.Round(Air.GetLocalAirDensity(altitude), 2)} kg/m^3";


            thrustOfPropeller.Text = $"thrust: {(int)thrustToDisplay}N";

            AoA.Text = $"{Math.Round(wingsManager.wings[0].angleOfAttack, 1)} °";

            speed.Text = $"{(velocity < 20 ? 0 : Math.Round(velocity))} km/h";
        }
        private void UpdatePropeller() {
            if (!displayPropeller)
                return;

            //So the graphic doesn't flicker
            if (!propellor.Visible && propellerRpm < maxPropRpmBeforeSprite * .9f) {
                propellor.Visible = true;
            } else if (propellor.Visible && propellerRpm > maxPropRpmBeforeSprite * 1.1f) {
                propellor.Visible = false;
            }
            bool ShowSprite = !propellor.Visible;

            if (!ShowSprite) {
                propellor.Rotate(Vector3.Forward, propRotationModifier * propellerRpm);
            }

            propAtHighSpeed.Visible = ShowSprite;
        }
    }
}
