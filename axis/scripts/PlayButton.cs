using Godot;
using System;

public partial class PlayButton : Button
{
    [Export] PackedScene level1;

    public override void _Ready()
    {
        this.Pressed += OnButtonPress;
    }

    private void OnButtonPress()
    {
        GetTree().ChangeSceneToPacked(level1);
    }
}
