using Godot;
using System;
using System.Threading.Tasks;

public partial class LevelManager : Node2D
{
    [Export] private int levelIndex;
    private AudioStreamPlayer2D levelSong;
    private KeyGenerator keyGenerator;
    private GameUserInterface gameUserInterface;

    public override void _Ready()
    {
        this.levelSong = GetNode<AudioStreamPlayer2D>("LevelSong");
        this.keyGenerator = GetNode<KeyGenerator>("KeyGenerator");
        this.keyGenerator.LevelEnd += this.UpdatePlayerData;
        this.keyGenerator.Song = this.levelSong;
        this.gameUserInterface = GetNode<GameUserInterface>("GameUI");

        Key.IsInLevel = true;
        _ = StartLevel();
    }

    private async Task StartLevel()
    {
        this.gameUserInterface.SetHitRatingText("3");
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        this.gameUserInterface.SetHitRatingText("2");
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        this.gameUserInterface.SetHitRatingText("1");
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        levelSong.Play();
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
