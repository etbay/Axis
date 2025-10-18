using Godot;
using System;

public partial class LevelHighScoreLabel : Label
{
    [Export] private int levelIndex;

    public override void _Ready()
    {
        if (PlayerData.LevelScores.ContainsKey(levelIndex))
        {
            this.Text = "High Score: " + PlayerData.LevelScores[levelIndex];
        }
        else
        {
            this.Text = "No High Score";
        }
    }
}