using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class VideoSettings : Control
{
    private CheckBox fullscreenCheckBox;
    private OptionButton resolutionOptions;
    private Dictionary<int, Vector2I> screenResolutions = new Dictionary<int, Vector2I>
    {
        { 0, new Vector2I(720, 720) },
        { 1, new Vector2I(1280, 720) },
        { 2, new Vector2I(1920, 1080) },
        { 3, new Vector2I(2560, 1440) },
        { 4, new Vector2I(3840, 2160) }
    };

    public override void _Ready()
    {
        this.fullscreenCheckBox = GetNode<CheckBox>("Settings/FullscreenCheckBox");
        this.fullscreenCheckBox.ButtonPressed = this.GetTree().Root.Mode == Window.ModeEnum.Fullscreen;
        this.fullscreenCheckBox.Toggled += SetScreenMode;

        this.resolutionOptions = GetNode<OptionButton>("Settings/ResolutionOptions");
        this.resolutionOptions.ItemSelected += SetResolution;
        this.GetWindow().SizeChanged += UpdateResolutionSelection;

        InitializeSettings();
    }

    public override void _ExitTree()
    {
        this.GetWindow().SizeChanged -= UpdateResolutionSelection;
    }

    private void InitializeSettings()
    {
        resolutionOptions.Clear();

        foreach (KeyValuePair<int, Vector2I> resolutions in screenResolutions)
        {
            if (resolutions.Value <= DisplayServer.ScreenGetSize())
            {
                string text = resolutions.Value.X + "x" + resolutions.Value.Y;
                this.resolutionOptions.AddItem(text);
                int index = this.resolutionOptions.GetItemIndex(this.resolutionOptions.ItemCount - 1);
            }
        }

        UpdateResolutionSelection();
    }

    private void UpdateResolutionSelection()
    {
        bool foundResolution = false;
        foreach (KeyValuePair<int, Vector2I> resolutions in screenResolutions)
        {
            if (this.GetWindow().Size == resolutions.Value)
            {
                this.resolutionOptions.Select(resolutions.Key);
                foundResolution = true;
                break;
            }
        }

        if (!foundResolution)
        {
            GD.Print("Could not find " + this.GetWindow().Size + ". Adding to options");
            this.resolutionOptions.AddItem(this.GetWindow().Size.X + "x" + this.GetWindow().Size.Y);
            this.resolutionOptions.Select(this.resolutionOptions.ItemCount - 1);
        }
    }


    private void SetResolution(long index)
    {
        Vector2I size = screenResolutions[(int)index];
        GD.Print("Setting window size to " + size);
        if (size != DisplayServer.ScreenGetSize())
        {
            this.GetWindow().Mode = Window.ModeEnum.Windowed;
            this.fullscreenCheckBox.ButtonPressed = false;
        }
        else
        {
            this.GetWindow().Mode = Window.ModeEnum.Fullscreen;
            this.fullscreenCheckBox.ButtonPressed = true;
        }
        this.GetWindow().Size = size;

        InitializeSettings();
    }

    private void SetScreenMode(bool toggledOn)
    {
        if (toggledOn)
        {
            this.GetWindow().Mode = Window.ModeEnum.Fullscreen;
        }
        else
        {
            this.GetWindow().Mode = Window.ModeEnum.Windowed;
        }

        InitializeSettings();
    }
}
