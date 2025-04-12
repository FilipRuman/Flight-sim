using Godot;
using System;

public partial class LightRef : DirectionalLight3D
{
    public static DirectionalLight3D light;

    public override void _Ready()
    {
        light = this;
        base._Ready();
    }

}
