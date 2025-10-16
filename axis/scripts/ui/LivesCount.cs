using Godot;
using System;

public partial class LivesCount : Label
{
    public override void _Process(double delta)
    {
        this.Text = "Lives: " + (3 - PlayerData.NumMisses);
    }
}
