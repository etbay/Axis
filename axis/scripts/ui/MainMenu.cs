using Godot;
using System;

public partial class MainMenu : Control
{
    private Button playButton;
    private Button endlessModeButton;
    private Button settingsButton;
    private Button exitButton;
    private PackedScene levelSelection = GD.Load<PackedScene>("res://scenes/levels/level_select.tscn");
    private PackedScene endlessMode = GD.Load<PackedScene>("res://scenes/levels/endless_mode.tscn");
    private PackedScene settings = GD.Load<PackedScene>("res://scenes/levels/settings.tscn");

    public override void _Ready()
    {
        this.playButton = GetNode<Button>("Buttons/PlayButton");
        this.playButton.Pressed += this.LoadLevelSelect;

        this.endlessModeButton = GetNode<Button>("Buttons/EndlessModeButton");
        this.endlessModeButton.Pressed += this.LoadEndlessMode;

        this.settingsButton = GetNode<Button>("Buttons/SettingsButton");
        this.settingsButton.Pressed += this.LoadSettings;

        this.exitButton = GetNode<Button>("Buttons/ExitButton");
        this.exitButton.Pressed += this.ExitGame;
    }

    private void LoadLevelSelect()
    {
        this.GetTree().ChangeSceneToPacked(this.levelSelection);
    }

    private void LoadEndlessMode()
    {
        this.GetTree().ChangeSceneToPacked(this.endlessMode);
    }

    private void LoadSettings()
    {
        this.GetTree().ChangeSceneToPacked(this.settings);
    }

    private void ExitGame()
    {
        this.GetTree().Quit();
    }

}
