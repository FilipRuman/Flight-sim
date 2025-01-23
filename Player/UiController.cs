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
			speed.Text = $"{Math.Round((rb.Basis.Z * rb.LinearVelocity).Z, 1)} m/s";
		}
	}
}