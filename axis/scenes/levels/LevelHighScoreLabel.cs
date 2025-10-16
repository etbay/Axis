using Godot;
using System;

public partial class LevelHighScoreLabel : Label
{
    [Export] private int levelIndex;

    public override void _Ready()
    {
        if (PlayerData.EndlessScores.ContainsKey(levelIndex))
        {
            this.Text = "High Score: " + PlayerData.EndlessScores[levelIndex];
        }
        else
        {
            this.Text = "No High Score";
        }
    }
}