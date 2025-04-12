using Godot;
using System;

public partial class PlayerCam : Node3D
{
    [Export] public Camera3D cam;
    [Export] RayTracingHandler rayTracingHandler;
    public static PlayerCam Ref;
    public override void _Ready()
    {
        Ref = this;
        base._Ready();
    }
}
