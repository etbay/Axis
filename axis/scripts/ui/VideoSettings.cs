using Godot;
using System;

public partial class VideoSettings : Control
{
    private CheckBox fullscreenCheckBox;

    public override void _Ready()
    {
        this.fullscreenCheckBox = GetNode<CheckBox>("Settings/FullscreenCheckBox");
        this.fullscreenCheckBox.ButtonPressed = this.GetTree().Root.Mode == Window.ModeEnum.Fullscreen;
        this.fullscreenCheckBox.Toggled += SetScreenMode;
    }

    private void SetScreenMode(bool toggledOn)
    {
        if (toggledOn)
            this.GetTree().Root.Mode = Window.ModeEnum.Fullscreen;
        else
            this.GetTree().Root.Mode = Window.ModeEnum.Windowed;
    }
}
