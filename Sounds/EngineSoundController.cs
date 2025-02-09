using Godot;
using System;

public partial class EngineSoundController : Node
{
    [Export] AudioStreamPlayer player;
    [Export] public float Throttle;
    [Export] public Vector2 pitchMinMax;
    [Export] public Vector2 volumeMinMax;

    [Export] public GEffectController gEffectController;
    [Export] public Curve HighGEffectSoundVolume;
    public override void _Process(double delta)
    {
        if (!player.Playing && Throttle != 0)
        {
            GD.Print("Start");
            player.StreamPaused = false;
        }
        if (player.Playing && Throttle == 0)
            player.StreamPaused = true;
        var HighGEffect = gEffectController.PlayerGEffect;

        player.VolumeDb = Mathf.Lerp(volumeMinMax.X, volumeMinMax.Y, Throttle * HighGEffectSoundVolume.SampleBaked(HighGEffect));
        player.PitchScale = Mathf.Lerp(pitchMinMax.X, pitchMinMax.Y, Throttle);


        base._Process(delta);
    }

}
