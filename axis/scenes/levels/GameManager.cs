using Godot;
using System;

public partial class GameManager : Node2D
{
    public override void _Ready()
    {
        GameData.Initialize();
    }

}
