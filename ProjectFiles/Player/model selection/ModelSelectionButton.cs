using Godot;
using System;

public partial class ModelSelectionButton : Button
{
    [Export] ModelSelector modelSelector;
    [Export] PackedScene model;
    public override void _Ready()
    {
        Pressed += OnClick;

        base._Ready();
    }
    void OnClick()
    {
        GD.Print("OnClick button");

        modelSelector.OnSelection(model);
    }

}
