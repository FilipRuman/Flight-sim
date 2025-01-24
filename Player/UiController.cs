namespace Player
{
	using Godot;
	using System;
	public partial class UiController : CanvasLayer
	{
		[Export] private RigidBody3D rb;
		[Export] private WingsManager wingsManager;



		[ExportGroup("UI")]
		[Export] private Label speed;
		[Export] private Label AoA;


		public override void _Process(double delta)
		{
			AoA.Text = Math.Round(wingsManager.wings[0].angleOfAttack, 1).ToString();
			float velocity = (rb.GlobalBasis.Inverse() * rb.LinearVelocity).Z * 3.6f; /* in km/h */
			speed.Text = $"{(velocity < 20 ? 0 : Math.Round(velocity, 1))} km/h";
		}
	}
}