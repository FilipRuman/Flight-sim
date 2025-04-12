using Godot;
using System;

public partial class GEffectController : Node3D
{
    [Export] Gradient gradient;
    [Export] PlayerRbController rbController;



    public float PlayerGEffect = 0;
    [Export] float gEffectChangeSpeedModifier = .9f;
    [Export] Curve GEffectChangeBasedOnG;
    public override void _Process(double delta)
    {
        PlayerGEffect = Mathf.Clamp(PlayerGEffect + GEffectChangeBasedOnG.SampleBaked(rbController.currentGForce) * (float)delta * gEffectChangeSpeedModifier, 0, 10);
        var graphicalPlayerGEffect = Mathf.Clamp(PlayerGEffect, 0, 1);

        gradient.SetOffset(1, 1 - graphicalPlayerGEffect);
        gradient.SetColor(1, new Color(0, 0, 0, Mathf.Min(1, 3 * graphicalPlayerGEffect)));
        base._Process(delta);
    }



}
