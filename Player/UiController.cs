namespace Player
{
	using Godot;
	using System;
	public partial class UiController : CanvasLayer
	{
		[ExportGroup("Cameras")]
		[Export] Camera3D fp;
		[Export] Camera3D tp;
		private const string switchCam = "switchCam";
		[ExportGroup("Ref")]
		[Export] private RigidBody3D rb;
		[Export] private WingsManager wingsManager;
		[Export] private Thruster thruster;
		[Export] WindController windController;


		[ExportGroup("UI")]
		[Export] private Label speed;
		[Export] private Label AoA;
		[Export] private Slider throttle;
		[Export] private Label altitude;
		[Export] private Label airDensity;

		[Export] float FramesPerUpdate = 10;
		int frameIndex = 0;

		[ExportGroup("Prop")]
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
			if (Input.IsActionJustPressed(switchCam))
			{
				GD.Print("switchCam");
				fp.Current = !fp.Current;
				/* 				tp.Current = !tp.Current;
				 */
			}
			frameIndex++;
			float velocity = (rb.GlobalBasis.Inverse() * rb.LinearVelocity).Z * 3.6f; /* in km/h */
			windController.Speed = velocity;

			UpdateProp(velocity);



			if (frameIndex < FramesPerUpdate)
				return;
			frameIndex = 0;

			/* 			throttle.Editable = false;
			 */
			var altitude = wingsManager.Altitude;
			this.altitude.Text = $"{Mathf.RoundToInt(altitude)} m";
			airDensity.Text = $"{Math.Round(Air.GetLocalAirDensity(altitude), 2)} kg/m^3";

			throttle.Value = thruster.throttle;
			AoA.Text = $"{Math.Round(wingsManager.wings[0].angleOfAttack, 1)}°";

			speed.Text = $"{(velocity < 20 ? 0 : Math.Round(velocity))} km/h";
		}

		private void UpdateProp(float velocity)
		{
			bool ShowSprite = thruster.throttle > .2f || velocity > 20;
			propellor.Visible = !ShowSprite;
			if (!ShowSprite)
			{
				propellor.Rotate(Vector3.Forward, propRotationModifier * thruster.throttle);
			}

			propAtHighSpeed.Visible = ShowSprite;
		}
	}
}