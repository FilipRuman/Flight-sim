using Godot;
using System;

public partial class WindController : Node {
    [Export] AudioStreamPlayer player;
    [Export] public float MaxSpeed;
    [Export] public float Speed;
    [Export] public Vector2 volumeMinMax;

    [Export] public PlayerRbController rbController;
    [Export] public Curve GSoundVolume;
    [Export] public GEffectController gEffectController;
    [Export] public Curve HighGEffectSoundVolume;
    public override void _Process(double delta) {
        var G = rbController.currentGForce;
        var HighGEffect = gEffectController.PlayerGEffect;
        player.VolumeDb = Mathf.Lerp(volumeMinMax.X, volumeMinMax.Y, MathF.Min(Speed * GSoundVolume.SampleBaked(G) * HighGEffectSoundVolume.SampleBaked(HighGEffect) / MaxSpeed, 1));
        //So I don't get errors I don't like'em
        player.PitchScale = MathF.Max(.001f, HighGEffectSoundVolume.SampleBaked(HighGEffect));
        base._Process(delta);
    }



}
