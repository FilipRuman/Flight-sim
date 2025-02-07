using Godot;
using System;

public partial class ModelSelector : Node
{
    [Export] Node3D spawnPoint;
    [Export] TerrainGenController terrainGenController;
    public void OnSelection(PackedScene model)
    {

        var node = model.Instantiate();
        var node3D = node as Node3D;
        node3D.Position = Vector3.Up * 5000;
        /*         (node3D as RigidBody3D).LinearVelocity = Vector3.Forward * 100;
         */
        spawnPoint.AddChild(node);

        terrainGenController.Player = node3D;
        foreach (var item in GetChildren())
        {
            item.QueueFree();
        }
        Dispose();
    }

}
