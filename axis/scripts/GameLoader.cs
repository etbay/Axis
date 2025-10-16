using Godot;
using System;

public partial class GameLoader : Node2D
{
    public override void _Ready()
    {
        PlayerData.LoadData();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("reset_save_data"))
        {
            PlayerData.ResetSaveData();
            PlayerData.SaveData();
        }
    }

}
