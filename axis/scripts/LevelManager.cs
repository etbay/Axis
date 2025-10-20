using Godot;
using System;

public partial class LevelManager : GameManager
{
    [Export] private int levelIndex;
    [Export] private AudioStreamPlayer2D song;
    [Export] private int secondsToWait;
    [Export] private int levelKeySpeed;
    private KeyGenerator keyGenerator;
    private bool isSongPlaying = false;
    private int startTime;

    public override void _Ready()
    {
        base._Ready();  // init GameData (for debug mode)
        this.keyGenerator = GetNode<KeyGenerator>("KeyGenerator");
        this.keyGenerator.LevelEnd += this.UpdatePlayerData;
        this.keyGenerator.Song = this.song;
        Key.IsInLevel = true;
        GameData.KeySpeed = this.levelKeySpeed;
        this.startTime = (int)Time.GetTicksMsec();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);   // detects esc
        if (!isSongPlaying && ((int)Time.GetTicksMsec() - startTime) / 1000 >= secondsToWait)
        {
            song.Play();
            isSongPlaying = true;
        }
    }

    private void UpdatePlayerData()
    {
        if (PlayerData.LevelScores.ContainsKey(levelIndex))
        {
            int highScore = PlayerData.LevelScores[levelIndex];
            if (highScore < PlayerData.TotalScore) PlayerData.LevelScores[levelIndex] = PlayerData.TotalScore;
        }
        else
        {
            PlayerData.LevelScores[levelIndex] = PlayerData.TotalScore;
        }
        PlayerData.SaveData();
    }
}
