using Godot;
using System;
using System.Collections.Generic;

public partial class DifficultySelect : Control
{
    public override void _Ready()
    {
        SpeedSelectionButton.SpeedSelected += RemoveMenu;
    }

    public override void _ExitTree()
    {
        SpeedSelectionButton.SpeedSelected -= RemoveMenu;
    }

    private void RemoveMenu()
    {
        this.QueueFree();
    }
}
