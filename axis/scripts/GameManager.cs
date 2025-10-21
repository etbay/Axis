using Godot;
using System;

public partial class GameManager : Node2D
{
    public override void _Ready()
    {
        PlayerData.Reset();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("escape"))
        {
            GetTree().ChangeSceneToPacked(GameData.MainMenu);
        }
    }
}
