using Godot;
using System;

public partial class LoadSceneButton : Button
{
    [Export] private PackedScene scene;

    public override void _Ready()
    {
        this.Pressed += OnButtonPress;
    }

    private void OnButtonPress()
    {
        GetTree().ChangeSceneToPacked(scene);
    }
}
