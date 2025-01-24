namespace Player
{
	using Godot;
	using System;
	public partial class UiController : CanvasLayer
	{
		[Export] Camera3D fp;
		[Export] Camera3D tp;

		[Export] private RigidBody3D rb;
		[Export] private WingsManager wingsManager;
		[Export] private Thruster thruster;


		[ExportGroup("UI")]
		[Export] private Label speed;
		[Export] private Label AoA;
		[Export] private Slider throttle;

		[Export] float FramesPerUpdate = 10;

		int frameIndex = 0;
		private const string switchCam = "switchCam";

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

			if (frameIndex < FramesPerUpdate)
				return;
			frameIndex = 0;

			/* 			throttle.Editable = false;
			 */
			throttle.Value = thruster.throttle;
			AoA.Text = $"{Math.Round(wingsManager.wings[0].angleOfAttack, 1)}°";
			float velocity = (rb.GlobalBasis.Inverse() * rb.LinearVelocity).Z * 3.6f; /* in km/h */
			speed.Text = $"{(velocity < 20 ? 0 : Math.Round(velocity))} km/h";
		}
	}
}