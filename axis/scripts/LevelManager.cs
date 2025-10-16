using Godot;
using System;

public partial class LevelManager : Node2D
{
    [Export] private AudioStreamPlayer2D song;
    [Export] private int secondsToWait;
    [Export] private int levelKeySpeed;
    private bool isSongPlaying = false;
    private int startTime;

    public override void _Ready()
    {
        Key.IsInLevel = true;
        GameData.KeySpeed = this.levelKeySpeed;
        this.startTime = (int)Time.GetTicksMsec();
    }

    public override void _Process(double delta)
    {
        if (!isSongPlaying && ((int)Time.GetTicksMsec() - startTime) / 1000 >= secondsToWait)
        {
            song.Play();
            isSongPlaying = true;
        }
    }

}
