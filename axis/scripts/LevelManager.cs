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
        base._Ready();
        keyGenerator = GetChild<KeyGenerator>(1);
        keyGenerator.LevelEnd += UpdatePlayerData;
        keyGenerator.Song = this.song;
        Key.IsInLevel = true;
        GameData.KeySpeed = this.levelKeySpeed;
        this.startTime = (int)Time.GetTicksMsec();
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


    public override void _Process(double delta)
    {
        if (!isSongPlaying && ((int)Time.GetTicksMsec() - startTime) / 1000 >= secondsToWait)
        {
            song.Play();
            isSongPlaying = true;
        }
    }

}
