namespace Player
{
    using Godot;
    using System;
    public partial class UiController : CanvasLayer
    {


        [ExportGroup("Ref")]
        [Export] private RigidBody3D rb;
        [Export] private WingsManager wingsManager;
        [Export] private Thruster thruster;
        [Export] WindController windController;


        [ExportGroup("UI")]
        [Export] private Label speed;
        [Export] private Label AoA;
        [Export] private Label altitude;
        [Export] private Label airDensity;
        [Export] private Label thrustOfPropeller;
        [Export] private Label fps;

        [Export] float FramesPerUpdate = 10;
        int frameIndex = 0;

        [ExportGroup("Prop")]
        [Export] bool displayPropeller;
        [Export] private Node3D propellor;
        [Export] private Sprite3D propAtHighSpeed;
        [Export] private float propRotationModifier = 300;


        [ExportGroup("Controls")]
        [Export] private Panel controls;
        [Export] private Button controlsToggleButton;
        public override void _Ready()
        {
            controlsToggleButton.Pressed += () => controls.Visible = !controls.Visible;
            base._Ready();
        }
        public override void _Process(double delta)
        {
            fps.Text = $"fps: {Mathf.RoundToInt(Engine.GetFramesPerSecond())}";

            frameIndex++;
            float velocity = wingsManager.FrontalVelocity * 3.6f; /* in km/h */
            windController.Speed = velocity;

            UpdatePropeller(velocity);



            if (frameIndex < FramesPerUpdate)
                return;
            frameIndex = 0;

            /*throttle.Editable = false;
			 */
            var altitude = wingsManager.Altitude;
            this.altitude.Text = $"{Mathf.RoundToInt(altitude)} m";
            airDensity.Text = $"{Math.Round(Air.GetLocalAirDensity(altitude), 2)} kg/m^3";

            thrustOfPropeller.Text = $"{Math.Round(thruster.thrustOfPropeller)} N";

            AoA.Text = $"{Math.Round(wingsManager.wings[0].angleOfAttack, 1)} °";

            speed.Text = $"{(velocity < 20 ? 0 : Math.Round(velocity))} km/h";
        }

        private void UpdatePropeller(float velocity)
        {
            if (!displayPropeller)
                return;
            if (velocity < 1)
                velocity = 0;
            bool ShowSprite = velocity > 40;
            propellor.Visible = !ShowSprite;
            if (!ShowSprite)
            {
                propellor.Rotate(Vector3.Forward, propRotationModifier * (velocity + thruster.throttle * 10f));
            }

            propAtHighSpeed.Visible = ShowSprite;
        }
    }
}
