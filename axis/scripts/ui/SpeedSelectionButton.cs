using Godot;
using System;

public partial class SpeedSelectionButton : Button
{
    [Export] int speed;
    public static Action SpeedSelected;

    public override void _Ready()
    {
        this.Pressed += ChangeKeySpeed;
    }

    private void ChangeKeySpeed()
    {
        GameData.KeySpeed = this.speed;
        SpeedSelected?.Invoke();
    }
}
